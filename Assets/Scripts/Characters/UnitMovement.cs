using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private GameObject selection;
    [SerializeField] private UnitType unitType;

    [Header("Movement")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float stoppingDistance = 1f;

    public enum UnitType
    {
        Pawn,
        Rider
    }

    #region Getters / Setters

    public UnitType GetUnitType
    {
        get { return unitType; }
        set { unitType = value; }
    }

    public GameObject Selection
    {
        get { return selection; }
        set { selection = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    #endregion

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
    }

    #region Movement

    public void MoveToPosition(Vector3 position, float stoppingDistance = -1)
    {
        if (agent == null) return;

        if (stoppingDistance != -1)
            agent.stoppingDistance = stoppingDistance;

        agent.SetDestination(position);
    }

    #endregion
}