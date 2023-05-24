using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawnUnitMovementPrefabs;
    [SerializeField] private List<GameObject> riderUnitMovementPrefabs;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private Vector3 center;
    [SerializeField] private Slider productionSlider;

    [Header("Production Properties")]
    [SerializeField] private int pawnsToProduct = 0;
    [SerializeField] private int ridersToProduct = 0;
    [SerializeField] private float timeOfPawnProduction = 3f;
    [SerializeField] private float timeOfRiderProduction = 3f;
    [SerializeField] private List<UnitMovement> units;
    [SerializeField] private List<UnitMovement> selectedUnitMovements;

    private float timeRemaining = 0f;
    private bool inProduction;
    private bool canStartProduction;
    private bool canProduce;
    private UnitMovement.UnitType unitTypeToProduct;

    // Formation
    private FormationBase _formation;

    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        inProduction = false;
        canStartProduction = false;
        canProduce = false;

        center = gathering.position;
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        if (GetComponent<UnitStats>().IsDead) GameManager.instance.EndGame(true);

        HandleProduction();
    }

    #region Production

    public void StartProduction()
    {
        // Active Production
        canProduce = true;
    }

    private void HandleProduction()
    {
        if (!canProduce) return;

        // Active UnitMovement Production
        if ((pawnsToProduct > 0 || ridersToProduct > 0) && !inProduction) canStartProduction = true;

        // Handle Setup Start Production
        if (canStartProduction)
        {
            canStartProduction = false;

            if (pawnsToProduct > 0 && !inProduction)
            {
                inProduction = true;

                timeRemaining = timeOfPawnProduction;
                unitTypeToProduct = UnitMovement.UnitType.Pawn;
            }

            if (ridersToProduct > 0 && !inProduction)
            {
                inProduction = true;

                timeRemaining = timeOfRiderProduction;
                unitTypeToProduct = UnitMovement.UnitType.Rider;
            }

            productionSlider.maxValue = timeRemaining;
        }

        // Handle Timer + UnitMovement produced
        if (inProduction)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                productionSlider.value = timeRemaining;
            }
            else
            {
                if (unitTypeToProduct == UnitMovement.UnitType.Pawn)
                {
                    int randomPawn = Random.Range(0, pawnUnitMovementPrefabs.Count);
                    UnitMovement unit = Instantiate(pawnUnitMovementPrefabs[randomPawn], unitParent).GetComponent<UnitMovement>();

                    unit.MoveToPosition(gathering.position);

                    units.Add(unit);

                    pawnsToProduct--;

                    HandleArmy();
                }
                else if (unitTypeToProduct == UnitMovement.UnitType.Rider)
                {
                    int randomRider = Random.Range(0, riderUnitMovementPrefabs.Count);
                    UnitMovement unit = Instantiate(riderUnitMovementPrefabs[randomRider], unitParent).GetComponent<UnitMovement>();

                    unit.MoveToPosition(gathering.position);

                    units.Add(unit);

                    ridersToProduct--;

                    HandleArmy();
                }

                inProduction = false;
            }
        }
    }

    /*private void HandleProduction()
    {
        if ((pawnsToProduct > 0 || ridersToProduct > 0) && !inProduction && canStartProduction)
        {
            StartCoroutine(StartingProduction());
        }

        productionSlider.value = Mathf.Lerp(productionSlider.value, 0f, lerp * Time.deltaTime);
    }

    public void StartProduction()
    {
        Debug.Log($"EnemyManager starting to produce enemies");

        canStartProduction = true;

        StartCoroutine(StartingProduction());
    }

    private IEnumerator StartingProduction()
    {
        inProduction = true;

        while (pawnsToProduct > 0)
        {
            productionSlider.maxValue = timeOfPawnProduction;
            productionSlider.value = timeOfPawnProduction;

            yield return new WaitForSeconds(timeOfPawnProduction);

            int randomPawn = Random.Range(0, pawnUnitMovementPrefabs.Count);
            UnitMovement unit = Instantiate(pawnUnitMovementPrefabs[randomPawn], unitParent).GetComponent<UnitMovement>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            pawnsToProduct--;

            HandleArmy();
        }

        while (ridersToProduct > 0)
        {
            productionSlider.maxValue = timeOfRiderProduction;
            productionSlider.value = timeOfRiderProduction;

            yield return new WaitForSeconds(timeOfRiderProduction);

            int randomRider = Random.Range(0, riderUnitMovementPrefabs.Count);
            UnitMovement unit = Instantiate(riderUnitMovementPrefabs[randomRider], unitParent).GetComponent<UnitMovement>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            ridersToProduct--;

            HandleArmy();
        }

        inProduction = false;
    }*/

    #endregion

    public void HandleArmy()
    {
        int totalUnitMovements = selectedUnitMovements.Count;

        if (totalUnitMovements == 0) return;

        points = Formation.EvaluatePoints(totalUnitMovements, center);

        for (int i = 0; i < selectedUnitMovements.Count; i++)
        {
            if (selectedUnitMovements[i] != null)
                selectedUnitMovements[i].MoveToPosition(points[i]);
        }
    }
}