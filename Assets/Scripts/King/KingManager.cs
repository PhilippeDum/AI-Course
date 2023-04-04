using System.Collections;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pawnSoldierPrefab;
    [SerializeField] private Transform pawnsParent;

    [Header("Production Properties")]
    [SerializeField] private int nbUnitsToProduct = 0;
    [SerializeField] private int maxUnitsArray = 10;
    [SerializeField] private float timeOfProduction = 3f;

    private bool inProduction;

    #region Getters / Setters

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

            GameObject pawnSoldier = Instantiate(pawnSoldierPrefab, pawnsParent);

            nbUnitsToProduct--;

            Debug.Log($"{pawnSoldier.name} ready !");
        }

        inProduction = false;

        Debug.Log($"Production finished !");
    }

    #endregion
}