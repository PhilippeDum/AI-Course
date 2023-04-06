using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Vector3 center;

    private SelectionManager _selectionManager;

    [Header("Production Properties")]
    [SerializeField] private int nbUnitsToProduct = 0;
    [SerializeField] private int maxUnitsArray = 10;
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

    [SerializeField] private List<Vector3> points = new List<Vector3>();

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
        _selectionManager = FindObjectOfType<SelectionManager>();

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

        //HandleArmy();
    }

    #region Production

    private void HandleProduction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nbUnitsToProduct < maxUnitsArray)
        {
            nbUnitsToProduct++;
        }

        if (nbUnitsToProduct > 0 && !inProduction)
        {
            StartCoroutine(StartProduction());
        }
    }

    public void AddUnitToProduction()
    {
        if (nbUnitsToProduct < maxUnitsArray)
        {
            nbUnitsToProduct++;
        }

        _selectionManager.UpdateText(nbUnitsToProduct.ToString());
    }

    private IEnumerator StartProduction()
    {
        inProduction = true;

        while (nbUnitsToProduct > 0)
        {
            yield return new WaitForSeconds(timeOfProduction);

            Unit unit = Instantiate(unitPrefab, unitParent).GetComponent<Unit>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            nbUnitsToProduct--;
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