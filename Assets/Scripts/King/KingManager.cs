using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform unitParent;

    [Header("Production Properties")]
    [SerializeField] private int nbUnitsToProduct = 0;
    [SerializeField] private int maxUnitsArray = 10;
    [SerializeField] private float timeOfProduction = 3f;
    [SerializeField] private List<Unit> units;

    private bool inProduction;

    #region Getters / Setters

    public List<Unit> Units
    {
        get { return units; }
        set { units = value; }
    }

    #endregion

    private void Start()
    {
        inProduction = false;
    }

    private void Update()
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

    #region Production

    private IEnumerator StartProduction()
    {
        inProduction = true;

        Debug.Log($"Production in progress...");

        while (nbUnitsToProduct > 0)
        {
            yield return new WaitForSeconds(timeOfProduction);

            GameObject unit = Instantiate(unitPrefab, unitParent);

            units.Add(unit.GetComponent<Unit>());

            nbUnitsToProduct--;

            Debug.Log($"{unit.name} ready !");
        }

        inProduction = false;

        Debug.Log($"Production finished !");
    }

    #endregion
}