using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private bool isDead;
    [SerializeField] private float timeDeath = 4f;
    [SerializeField] private Slider healthSlider;

    [Header("Damage")]
    [SerializeField] private int damage;

    [Header("Attack")]
    [SerializeField] private Team team;
    [SerializeField] private float timeBetweenAttacks = 3f;
    [SerializeField] private float distanceToAttack = 2f;
    [SerializeField] private List<Stats> enemies;

    #region Getters / Setters

    public List<Stats> Enemies
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

    #endregion

    private void Start()
    {
        health = maxHealth;

        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        HandleHealth();

        HandleEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) enemies.RemoveAt(i);
        }
    }

    private void HandleEnemies()
    {
        if (enemies.Count > 0)
        {
            Stats enemy = enemies[0];

            Unit unit = GetComponent<Unit>();

            if (enemy == null || enemy.isDead) return;

            unit.MoveToPosition(enemies[0].transform.position, distanceToAttack);

            StartCoroutine(Attack(enemies[0]));
        }
    }

    private IEnumerator Attack(Stats enemy)
    {
        while (!enemy.IsDead)
        {
            enemy.TakeDamage(damage);

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    #region Handle Stats

    private void HandleHealth()
    {
        if (health <= 0)
        {
            health = 0;

            Destroy(gameObject, timeDeath);
        }

        healthSlider.transform.parent.LookAt(Camera.main.transform.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthSlider.value = health;
    }

    #endregion
}