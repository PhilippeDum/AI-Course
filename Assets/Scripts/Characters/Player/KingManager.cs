using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawnUnitMovementPrefabs;
    [SerializeField] private List<GameObject> riderUnitMovementPrefabs;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Vector3 center;
    [SerializeField] private GameObject canvasWorld;
    [SerializeField] private Slider productionSlider;

    private GameObject currentCanvasWorld;

    private UIManager uiManager;

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

    #region Getters / Setters

    public List<UnitMovement> UnitMovements
    {
        get { return units; }
        set { units = value; }
    }

    public List<UnitMovement> SelectedUnitMovements
    {
        get { return selectedUnitMovements; }
        set { selectedUnitMovements = value; }
    }

    public Vector3 Center
    {
        get { return center; }
        set { center = value; }
    }

    #endregion

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        inProduction = false;
        canStartProduction = false;

        center = gathering.position;
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        if (GetComponent<UnitStats>().IsDead) GameManager.instance.EndGame(true);

        HandleProduction();

        Move();

        Clear();
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentCanvasWorld != null) Destroy(currentCanvasWorld);

            HandleMovement();
        }
    }

    private void Clear()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null) units.Remove(units[i]);
        }

        for (int i = 0; i < selectedUnitMovements.Count; i++)
        {
            if (selectedUnitMovements[i] == null) selectedUnitMovements.Remove(selectedUnitMovements[i]);
        }
    }

    #region Production

    public void AddPawnToProduction()
    {
        pawnsToProduct++;

        UpdateKingText();
    }

    public void AddRiderToProduction()
    {
        ridersToProduct++;

        UpdateKingText();
    }

    public void UpdateKingText()
    {
        Element element = GetComponent<Element>();

        string text = $"{element.ElementDescription}\nIn Production : pawns ({pawnsToProduct}) & riders ({ridersToProduct})";

        uiManager.UpdateText(text);
    }

    private void HandleProduction()
    {
        // Active Production
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

                    GameManager.instance.CountPawns++;

                    pawnsToProduct--;
                }
                else if (unitTypeToProduct == UnitMovement.UnitType.Rider)
                {
                    int randomRider = Random.Range(0, riderUnitMovementPrefabs.Count);
                    UnitMovement unit = Instantiate(riderUnitMovementPrefabs[randomRider], unitParent).GetComponent<UnitMovement>();

                    unit.MoveToPosition(gathering.position);

                    units.Add(unit);

                    GameManager.instance.CountRiders++;

                    ridersToProduct--;
                }

                inProduction = false;
            }
        }
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            Vector3 hitPos = hit.point;
            hitPos.y += 0.1f;

            currentCanvasWorld = Instantiate(canvasWorld, hitPos, canvasWorld.transform.rotation);

            Destroy(currentCanvasWorld, 0.5f);

            center = hit.point;
            MoveSelectedUnitMovement(hit.point);
        }
    }

    private void MoveSelectedUnitMovement(Vector3 position)
    {
        for (int i = 0; i < selectedUnitMovements.Count; i++)
        {
            selectedUnitMovements[i].MoveToPosition(position);
        }

        HandleArmy();
    }

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