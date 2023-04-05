using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;

    private Vector3 movePosition;

    [Header("Production Properties")]
    [SerializeField] private int nbUnitsToProduct = 0;
    [SerializeField] private int maxUnitsArray = 10;
    [SerializeField] private float timeOfProduction = 3f;
    [SerializeField] private List<Unit> units;
    [SerializeField] private List<Unit> selectedUnits;

    private bool inProduction;

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

    #endregion

    private void Start()
    {
        inProduction = false;
    }

    private void Update()
    {
        HandleProduction();

        HandleMovement();
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

        if (Input.GetMouseButtonDown(1) && movePosition != null)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].MoveToPosition(movePosition);
            }
        }
    }

    #endregion
}