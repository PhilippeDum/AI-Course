using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Unit.UnitTeam teamEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitManager>())
        {
            UnitManager unitStats = other.GetComponent<UnitManager>();

            if (unitStats.UnitData.TeamUnit == teamEnemy)
            {
                GetComponentInParent<UnitManager>().Enemies.Add(unitStats);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<UnitManager>())
        {
            UnitManager unitStats = other.GetComponent<UnitManager>();

            if (unitStats.UnitData.TeamUnit == teamEnemy)
            {
                GetComponentInParent<UnitManager>().Enemies.Remove(unitStats);
            }
        }
    }
}