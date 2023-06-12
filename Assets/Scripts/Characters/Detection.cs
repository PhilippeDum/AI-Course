using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private bool ability = false;
    [SerializeField] private bool upgrade = false;

    private void OnTriggerEnter(Collider other)
    {
        HandleDetection(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleDetection(other, false);
    }

    private void HandleDetection(Collider other, bool triggerEnter)
    {
        UnitManager unitDetected = other.GetComponent<UnitManager>();

        if (unitDetected != null)
        {
            Unit.UnitTeam currentTeam = GetComponentInParent<UnitManager>().UnitData.TeamUnit;

            // Trigger Enter
            if (triggerEnter)
            {
                if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                    GetComponentInParent<Ability>().AddUnit(unitDetected);
                else if (unitDetected.UnitData.TeamUnit != currentTeam)
                    GetComponentInParent<UnitManager>().Enemies.Add(unitDetected);
                else if (unitDetected.UnitData.TeamUnit != currentTeam && upgrade)
                    GetComponentInParent<UpgradeBuilding>().Units.Add(unitDetected);
            }
            // Trigger Exit
            else
            {
                if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                    GetComponentInParent<Ability>().RemoveUnit(unitDetected);
                else if (unitDetected.UnitData.TeamUnit != currentTeam)
                    GetComponentInParent<UnitManager>().Enemies.Remove(unitDetected);
                else if (unitDetected.UnitData.TeamUnit != currentTeam && upgrade)
                    GetComponentInParent<UpgradeBuilding>().Units.Remove(unitDetected);
            }
        }
    }
}