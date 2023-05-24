using UnityEditor;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<OutpostTower>().AddUnit(unitDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<OutpostTower>().RemoveUnit(unitDetected);
        }
    }
}