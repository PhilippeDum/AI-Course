using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject selection;
    [SerializeField] private UnitType unitType;

    [Header("Movement")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float stoppingDistance = 1f;

    [Header("Flocking")]
    [SerializeField] private float distance;

    public enum UnitType
    {
        Pawn
    }

    #region Getters / Setters

    public GameObject Selection
    {
        get { return selection; }
        set { selection = value; }
    }

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        
    }

    #region Movement

    #endregion

    #region Flocking

    #endregion
}