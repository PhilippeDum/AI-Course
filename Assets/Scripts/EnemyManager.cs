using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawnUnitPrefabs;
    [SerializeField] private List<GameObject> riderUnitPrefabs;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private Vector3 center;
    [SerializeField] private Slider productionSlider;

    [Header("Production Properties")]
    [SerializeField] private int pawnsToProduct = 0;
    [SerializeField] private int ridersToProduct = 0;
    [SerializeField] private float timeOfPawnProduction = 3f;
    [SerializeField] private float timeOfRiderProduction = 3f;
    [SerializeField] private List<Unit> units;
    [SerializeField] private List<Unit> selectedUnits;

    private float timeRemaining = 0f;
    private bool inProduction;
    private bool canStartProduction;
    private bool canProduce;
    private Unit.UnitType unitTypeToProduct;

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

        if (GetComponent<Stats>().IsDead) GameManager.instance.EndGame(true);

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

        // Active Unit Production
        if ((pawnsToProduct > 0 || ridersToProduct > 0) && !inProduction) canStartProduction = true;

        // Handle Setup Start Production
        if (canStartProduction)
        {
            canStartProduction = false;

            if (pawnsToProduct > 0 && !inProduction)
            {
                inProduction = true;

                timeRemaining = timeOfPawnProduction;
                unitTypeToProduct = Unit.UnitType.Pawn;
            }

            if (ridersToProduct > 0 && !inProduction)
            {
                inProduction = true;

                timeRemaining = timeOfRiderProduction;
                unitTypeToProduct = Unit.UnitType.Rider;
            }

            productionSlider.maxValue = timeRemaining;
        }

        // Handle Timer + Unit produced
        if (inProduction)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                productionSlider.value = timeRemaining;
            }
            else
            {
                if (unitTypeToProduct == Unit.UnitType.Pawn)
                {
                    int randomPawn = Random.Range(0, pawnUnitPrefabs.Count);
                    Unit unit = Instantiate(pawnUnitPrefabs[randomPawn], unitParent).GetComponent<Unit>();

                    unit.MoveToPosition(gathering.position);

                    units.Add(unit);

                    pawnsToProduct--;

                    HandleArmy();
                }
                else if (unitTypeToProduct == Unit.UnitType.Rider)
                {
                    int randomRider = Random.Range(0, riderUnitPrefabs.Count);
                    Unit unit = Instantiate(riderUnitPrefabs[randomRider], unitParent).GetComponent<Unit>();

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

            int randomPawn = Random.Range(0, pawnUnitPrefabs.Count);
            Unit unit = Instantiate(pawnUnitPrefabs[randomPawn], unitParent).GetComponent<Unit>();

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

            int randomRider = Random.Range(0, riderUnitPrefabs.Count);
            Unit unit = Instantiate(riderUnitPrefabs[randomRider], unitParent).GetComponent<Unit>();

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
        int totalUnits = selectedUnits.Count;

        if (totalUnits == 0) return;

        points = Formation.EvaluatePoints(totalUnits, center);

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] != null)
                selectedUnits[i].MoveToPosition(points[i]);
        }
    }
}