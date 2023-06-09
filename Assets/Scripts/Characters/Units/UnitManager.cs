using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class UnitManager : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private Unit unitData;
    [SerializeField] private Unit unitDataUpgraded;

    private Unit currentData;

    [Header("References")]
    [SerializeField] private Defogger defoggerMesh;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject selection;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider otherSlider;
    [SerializeField] private bool isDead;
    [SerializeField] private bool refreshFog = false;

    [Header("Enemies Detected")]
    [SerializeField] private List<UnitManager> enemies;

    private Vector2 defaultRangeAttacks;
    private float defaultSpeed;
    private float timeRemaining;
    private bool canAttack = false;
    private bool inAttack = false;
    private bool boostActive = false;
    private bool isWorking = false;

    private GameManager gameManager;

    #region Getters / Setters

    public Unit UnitData
    {
        get { return unitData; }
        set { unitData = value; }
    }

    public Unit UnitDataUpgraded
    { 
        get { return unitDataUpgraded; }
        set { unitDataUpgraded = value; }
    }

    public Defogger DefoggerMesh
    {
        get { return defoggerMesh; }
        set { defoggerMesh = value; }
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

    public bool IsWorking
    {
        get { return isWorking; }
        set { isWorking = value; }
    }

    public List<UnitManager> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    #endregion

    private void Start()
    {
        gameManager = GameManager.instance;

        if (UnitData.TypeUnit != Unit.UnitType.King && UnitData.TypeUnit != Unit.UnitType.Tower)
        {
            InitializeMovement();
        }

        SetUnitData(unitData);

        InitializeStats();

        defoggerMesh.Unhide();
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        UpdateStats();
    }

    #region Custom Methods

    public void SetUnitData(Unit unitData)
    {
        if (currentData != null) currentData.UnitModel.SetActive(false);

        currentData = unitData;
        defoggerMesh = currentData.DefoggerMesh;

        if (currentData != null) currentData.UnitModel.SetActive(true);
    }

    #region Movement

    private void InitializeMovement()
    {
        if (agent == null) return;

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
        currentData.Health = currentData.MaxHealth;

        healthSlider.maxValue = currentData.Health;
        healthSlider.value = currentData.Health;

        defaultRangeAttacks = currentData.RangeTimeBetweenAttacks;
        defaultSpeed = currentData.Speed;
    }

    private void UpdateStats()
    {
        HandleHealth();

        if (!isDead)
        {
            if (currentData.TypeUnit != Unit.UnitType.Worker)
            {
                HandleEnemies();

                Clean();

                HandleAttack();
            }
        }
    }

    #region Attacking Unit

    private void HandleEnemies()
    {
        if (enemies.Count > 0)
        {
            UnitManager enemy = enemies[0];

            if (enemy == null || enemy.isDead) return;

            MoveToPosition(enemy.transform.position, currentData.DistanceToAttack);

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

            timeRemaining = Random.Range(currentData.RangeTimeBetweenAttacks.x, currentData.RangeTimeBetweenAttacks.y);

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

    #endregion

    #region Worker Unit

    public void StartWork(ResourceGathering resource)
    {
        if (!isWorking)
        {
            isWorking = true;

            StartCoroutine(Working(resource));
        }
    }

    private IEnumerator Working(ResourceGathering resource)
    {
        while (gameManager.Distance(transform.position, resource.transform.position) > currentData.DistanceToAttack)
        {
            MoveToPosition(resource.transform.position, currentData.StoppingDistance);

            yield return new WaitForSeconds(0.1f);
        }

        resource.Collect(this);
    }

    #endregion

    #region Handle Unit Stats

    private void HandleHealth()
    {
        if (currentData.Health <= 0)
        {
            currentData.Health = 0;

            isDead = false;

            Destroy(gameObject);
        }

        healthSlider.transform.parent.LookAt(Camera.main.transform.position);
        healthSlider.value = currentData.Health;
    }

    public void Heal(int value)
    {
        currentData.Health += value;
    }

    public void TakeDamage(int damage)
    {
        currentData.Health -= damage;
    }

    public void Boost(float value)
    {
        // Reset Boost
        if (value == -1 && boostActive)
        {
            currentData.Speed = defaultSpeed;
            currentData.RangeTimeBetweenAttacks = defaultRangeAttacks;

            boostActive = false;

            return;
        }
        else if (!boostActive)
        {
            boostActive = true;

            currentData.Speed *= value;
            currentData.RangeTimeBetweenAttacks /= 2;
        }
    }

    #endregion

    #endregion

    #endregion
}