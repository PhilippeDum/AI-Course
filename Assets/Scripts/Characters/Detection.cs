using UnityEngine;

public class Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitManager unitDetected = other.GetComponent<UnitManager>();

            GetComponentInParent<Ability>().AddUnit(unitDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitManager unitDetected = other.GetComponent<UnitManager>();

            GetComponentInParent<Ability>().RemoveUnit(unitDetected);
        }
    }
}