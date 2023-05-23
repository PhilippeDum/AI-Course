using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Team teamEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitStats>())
        {
            UnitStats unitStats = other.GetComponent<UnitStats>();

            if (unitStats.GetTeam == teamEnemy)
            {
                GetComponentInParent<UnitStats>().Enemies.Add(unitStats);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<UnitStats>())
        {
            UnitStats unitStats = other.GetComponent<UnitStats>();

            if (unitStats.GetTeam == teamEnemy)
            {
                GetComponentInParent<UnitStats>().Enemies.Remove(unitStats);
            }
        }
    }
}