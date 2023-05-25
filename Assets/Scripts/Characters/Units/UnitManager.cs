using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private Unit unitData;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject selection;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider otherSlider;
    [SerializeField] private bool isDead;

    [Header("Enemies Detected")]
    [SerializeField] private List<UnitManager> enemies;

    private Vector2 defaultRangeAttacks;
    private float defaultSpeed;
    private float timeRemaining;
    private bool canAttack = false;
    private bool inAttack = false;
    private bool boostActive = false;

    #region Getters / Setters

    public Unit UnitData 
    { 
        get { return unitData; }
        set { unitData = value; }
    }

    public GameObject Selection
    {
        get { return selection; }
        set { selection = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public List<UnitManager> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    #endregion

    private void Start()
    {
        if (UnitData.TypeUnit != Unit.UnitType.King && UnitData.TypeUnit != Unit.UnitType.Tower)
        {
            InitializeMovement();
        }

        InitializeStats();
    }

    private void Update()
    {
        //if (GameManager.instance.GameFinished) return;

        UpdateStats();
    }

    #region Custom Methods

    #region Movement

    private void InitializeMovement()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = unitData.Speed;
        agent.stoppingDistance = unitData.StoppingDistance;
    }

    public void MoveToPosition(Vector3 position, float stoppingDistance = -1)
    {
        if (agent == null) return;

        if (stoppingDistance != -1)
            agent.stoppingDistance = stoppingDistance;

        agent.SetDestination(position);
    }

    #endregion

    #region Stats

    private void InitializeStats()
    {
        //unit = GetComponent<UnitManager>();

        unitData.Health = unitData.MaxHealth;

        healthSlider.maxValue = unitData.Health;
        healthSlider.value = unitData.Health;

        defaultRangeAttacks = unitData.RangeTimeBetweenAttacks;
        defaultSpeed = unitData.Speed;
    }

    private void UpdateStats()
    {
        HandleHealth();

        if (!isDead)
        {
            HandleEnemies();

            Clean();

            HandleAttack();
        }
    }

    private void HandleEnemies()
    {
        if (enemies.Count > 0)
        {
            UnitManager enemy = enemies[0];

            if (enemy == null || enemy.isDead) return;

            MoveToPosition(enemy.transform.position, unitData.DistanceToAttack);

            canAttack = true;
        }
    }

    private void Clean()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) enemies.RemoveAt(i);
        }
    }

    private void HandleAttack()
    {
        if (canAttack && !inAttack)
        {
            canAttack = false;

            timeRemaining = Random.Range(unitData.RangeTimeBetweenAttacks.x, unitData.RangeTimeBetweenAttacks.y);

            inAttack = true;
        }

        if (inAttack)
        {
            if (enemies.Count <= 0) return;

            UnitManager enemy = enemies[0];

            if (enemy == null || enemy.isDead) return;

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                transform.LookAt(enemy.transform.position);

                enemy.TakeDamage(enemy.UnitData.Damage);

                inAttack = false;
            }
        }
    }

    #region Handle Unit Stats

    private void HandleHealth()
    {
        if (unitData.Health <= 0)
        {
            unitData.Health = 0;

            isDead = false;

            Destroy(gameObject/*, unitData.timeDeath*/);
        }

        healthSlider.transform.parent.LookAt(Camera.main.transform.position);
        healthSlider.value = unitData.Health;
    }

    public void Heal(int value)
    {
        unitData.Health += value;
    }

    public void TakeDamage(int damage)
    {
        unitData.Health -= damage;
    }

    public void Boost(float value)
    {
        // Reset Boost
        if (value == -1 && boostActive)
        {
            unitData.Speed = defaultSpeed;
            unitData.RangeTimeBetweenAttacks = defaultRangeAttacks;

            boostActive = false;

            return;
        }
        else if (!boostActive)
        {
            boostActive = true;

            unitData.Speed *= value;
            unitData.RangeTimeBetweenAttacks /= 2;
        }
    }

    #endregion

    #endregion

    #endregion
}