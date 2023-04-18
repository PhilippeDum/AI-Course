using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pawnUnitPrefab;
    [SerializeField] private GameObject riderUnitPrefab;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Vector3 center;

    private UIManager uiManager;

    [Header("Production Properties")]
    [SerializeField] private int pawnsToProduct = 0;
    [SerializeField] private int ridersToProduct = 0;
    [SerializeField] private float timeOfProduction = 3f;
    [SerializeField] private List<Unit> units;
    [SerializeField] private List<Unit> selectedUnits;

    private bool inProduction;

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

        center = gathering.position;
    }

    private void Update()
    {
        HandleProduction();

        if (Input.GetMouseButtonDown(1))
        {
            HandleMovement();
        }
    }

    #region Production

    private void HandleProduction()
    {
        if ((pawnsToProduct > 0 || ridersToProduct > 0) && !inProduction)
        {
            StartCoroutine(StartProduction());
        }
    }

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

    private IEnumerator StartProduction()
    {
        inProduction = true;

        while (pawnsToProduct > 0)
        {
            yield return new WaitForSeconds(timeOfProduction);

            Unit unit = Instantiate(pawnUnitPrefab, unitParent).GetComponent<Unit>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            pawnsToProduct--;
        }

        while (ridersToProduct > 0)
        {
            yield return new WaitForSeconds(timeOfProduction);

            Unit unit = Instantiate(riderUnitPrefab, unitParent).GetComponent<Unit>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            ridersToProduct--;
        }

        inProduction = false;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            // Add if condition to detect enemy for attack

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