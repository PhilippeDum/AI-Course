using UnityEngine;

public class Detection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UnitMovement"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<Ability>().AddUnitMovement(unitDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UnitMovement"))
        {
            UnitStats unitDetected = other.GetComponent<UnitStats>();

            GetComponentInParent<Ability>().RemoveUnitMovement(unitDetected);
        }
    }
}