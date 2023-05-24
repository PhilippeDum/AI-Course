using UnityEngine;

public class Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<Ability>().AddUnit(unitDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<Ability>().AddUnit(unitDetected);
        }
    }
}