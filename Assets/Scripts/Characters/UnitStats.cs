using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private bool isDead;
    [SerializeField] private float timeDeath = 1f;
    [SerializeField] private Slider healthSlider;

    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("Attack")]
    [SerializeField] private Team team;
    [SerializeField] private Vector2 rangeTimeBetweenAttacks = new Vector2(1f, 3f);
    [SerializeField] private float distanceToAttack = 2f;
    [SerializeField] private List<UnitStats> enemies;

    private Vector2 defaultRangeAttacks;
    private float defaultSpeed;
    private bool boostActive = false;

    private float timeRemaining = 0;
    private bool canAttack = false;
    private bool inAttack = false;

    private UnitMovement unit;

    #region Getters / Setters

    public List<UnitStats> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    public Team GetTeam
    {
        get { return team; }
        set { team = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public bool BoostActive
    {
        get { return boostActive; }
        set { boostActive = value; }
    }

    #endregion

    private void Start()
    {
        unit = GetComponent<UnitMovement>();

        health = maxHealth;

        healthSlider.maxValue = health;
        healthSlider.value = health;

        defaultRangeAttacks = rangeTimeBetweenAttacks;
        defaultSpeed = unit.Speed;
    }

    private void Update()
    {
        //if (GameManager.instance.GameFinished) return;

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
            UnitStats enemy = enemies[0];

            if (enemy == null || enemy.isDead) return;

            unit.MoveToPosition(enemy.transform.position, distanceToAttack);

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

            timeRemaining = Random.Range(rangeTimeBetweenAttacks.x, rangeTimeBetweenAttacks.y);

            inAttack = true;
        }

        if (inAttack)
        {
            if (enemies.Count <= 0) return;

            UnitStats enemy = enemies[0];

            if (enemy == null || enemy.isDead) return;

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                transform.LookAt(enemy.transform.position);

                enemy.TakeDamage(enemy.damage);

                inAttack = false;
            }
        }
    }

    #region Handle UnitMovement Stats

    private void HandleHealth()
    {
        if (health <= 0)
        {
            health = 0;

            isDead = false;

            Destroy(gameObject, timeDeath);
        }

        healthSlider.transform.parent.LookAt(Camera.main.transform.position);
        healthSlider.value = health;
    }

    public void Heal(int value)
    {
        health += value;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public void Boost(float value)
    {
        // Reset Boost
        if (value == -1 && boostActive)
        {
            unit.Speed = defaultSpeed;
            rangeTimeBetweenAttacks = defaultRangeAttacks;

            boostActive = false;

            return;
        }
        else if (!boostActive)
        {
            boostActive = true;

            unit.Speed *= value;
            rangeTimeBetweenAttacks /= 2;
        }
    }

    #endregion
}