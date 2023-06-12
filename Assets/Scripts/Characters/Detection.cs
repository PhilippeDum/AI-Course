using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField] private bool ability = false;
    [SerializeField] private bool upgrade = false;

    private Unit.UnitTeam currentTeam;

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

        if (unitDetected == null) return;

        if (GetComponentInParent<UnitManager>())
            currentTeam = GetComponentInParent<UnitManager>().UnitData.TeamUnit;
        else if (GetComponentInParent<Building>())
            currentTeam = GetComponentInParent<Building>().Team;

        // Trigger Enter
        if (triggerEnter)
        {
            if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                GetComponentInParent<Ability>().AddUnit(unitDetected);
            else if (unitDetected.UnitData.TeamUnit == currentTeam && upgrade)
                GetComponentInParent<UpgradeBuilding>().Units.Add(unitDetected);
            else if (unitDetected.UnitData.TeamUnit != currentTeam)
                GetComponentInParent<UnitManager>().Enemies.Add(unitDetected);
        }
        // Trigger Exit
        else
        {
            if (unitDetected.UnitData.TeamUnit == currentTeam && ability)
                GetComponentInParent<Ability>().RemoveUnit(unitDetected);
            else if (unitDetected.UnitData.TeamUnit == currentTeam && upgrade)
                GetComponentInParent<UpgradeBuilding>().Units.Remove(unitDetected);
            else if (unitDetected.UnitData.TeamUnit != currentTeam)
                GetComponentInParent<UnitManager>().Enemies.Remove(unitDetected);
        }
    }
}