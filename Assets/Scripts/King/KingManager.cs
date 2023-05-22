using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawnUnitPrefabs;
    [SerializeField] private List<GameObject> riderUnitPrefabs;
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
    [SerializeField] private List<Unit> units;
    [SerializeField] private List<Unit> selectedUnits;

    private float timeRemaining = 0f;
    private bool inProduction;
    private bool canStartProduction;
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

    #region Getters / Setters

    public List<Unit> Units
    {
        get { return units; }
        set { units = value; }
    }

    public List<Unit> SelectedUnits
    {
        get { return selectedUnits; }
        set { selectedUnits = value; }
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

        if (GetComponent<Stats>().IsDead) GameManager.instance.EndGame(true);

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

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] == null) selectedUnits.Remove(selectedUnits[i]);
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

                    GameManager.instance.CountPawns++;

                    pawnsToProduct--;
                }
                else if (unitTypeToProduct == Unit.UnitType.Rider)
                {
                    int randomRider = Random.Range(0, riderUnitPrefabs.Count);
                    Unit unit = Instantiate(riderUnitPrefabs[randomRider], unitParent).GetComponent<Unit>();

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
            MoveSelectedUnit(hit.point);
        }
    }

    private void MoveSelectedUnit(Vector3 position)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].MoveToPosition(position);
        }

        HandleArmy();
    }

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