using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Team teamEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            Stats stats = other.GetComponent<Stats>();

            if (stats.GetTeam == teamEnemy)
            {
                GetComponentInParent<Stats>().Enemies.Add(stats);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Stats>())
        {
            Stats stats = other.GetComponent<Stats>();

            if (stats.GetTeam == teamEnemy)
            {
                GetComponentInParent<Stats>().Enemies.Remove(stats);
            }
        }
    }
}