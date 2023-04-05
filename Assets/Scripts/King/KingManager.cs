using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;

    private Vector3 movePosition;
    private Vector3 center;

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
        inProduction = false;

        center = gathering.position;
    }

    private void Update()
    {
        HandleProduction();

        HandleMovement();

        HandleArmy();
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

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Add if condition to detect enemy for attack

            movePosition = hit.point;
        }

        if (Input.GetMouseButtonDown(1))
        {
            center = movePosition;

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].MoveToPosition(movePosition);
            }
        }
    }

    #endregion

    private void HandleArmy()
    {
        if (units.Count == 0) return;

        //int sqrtUnitsCount = (int)Mathf.Sqrt(units.Count);

        Debug.Log($"{units.Count} => {Mathf.RoundToInt(Mathf.Sqrt(units.Count))}");

        int round = Mathf.RoundToInt(Mathf.Sqrt(units.Count));

        int width = units.Count - round;
        int depth = round;

        //Debug.Log($"{units.Count} => sqrt = {sqrtUnitsCount}");
        Debug.Log($"{units.Count} => width = {width} & depth = {depth}");

        points = Formation.EvaluatePoints(width, depth).ToList();

        Debug.Log($"points = {points.Count}");

        /*for (int i = 0; i < units.Count; i++)
        {
            if (units[i] != null)
                units[i].transform.position = Vector3.MoveTowards(units[i].transform.position, center + points[i], units[i].Speed * Time.deltaTime);
        }*/
    }
}