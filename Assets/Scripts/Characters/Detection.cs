using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private bool ability = false;

    private void OnTriggerEnter(Collider other)
    {
        UnitManager unitDetected = other.GetComponent<UnitManager>();

        if (unitDetected != null)
        {
            Unit.UnitTeam currentTeam = GetComponentInParent<UnitManager>().UnitData.TeamUnit;

            if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                GetComponentInParent<Ability>().AddUnit(unitDetected);
            else if (unitDetected.UnitData.TeamUnit != currentTeam)
                GetComponentInParent<UnitManager>().Enemies.Add(unitDetected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnitManager unitDetected = other.GetComponent<UnitManager>();

        if (unitDetected != null)
        {
            Unit.UnitTeam currentTeam = GetComponentInParent<UnitManager>().UnitData.TeamUnit;

            if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                GetComponentInParent<Ability>().RemoveUnit(unitDetected);
            else if (unitDetected.UnitData.TeamUnit != currentTeam)
                GetComponentInParent<UnitManager>().Enemies.Remove(unitDetected);
        }
    }
}