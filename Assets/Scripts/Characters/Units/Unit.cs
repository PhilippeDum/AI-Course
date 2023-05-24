using UnityEngine;

[System.Serializable]
public class Unit
{
    public enum UnitType
    {
        Pawn,
        Rider,
        Tower,
        Queen,
        King
    }

    public enum UnitTeam
    {
        Player,
        Enemy
    }

    [Header("Datas")]
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private UnitType unitType;
    [SerializeField] private UnitTeam unitTeam;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance = 1f;

    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private Vector2 rangeTimeBetweenAttacks = new Vector2(1f, 3f);
    [SerializeField] private float distanceToAttack = 2f;

    #region Getters / Setters

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public UnitType TypeUnit 
    { 
        get { return unitType; } 
        set { unitType = value; }
    }

    public UnitTeam TeamUnit
    {
        get { return unitTeam; }
        set { unitTeam = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float StoppingDistance
    {
        get { return stoppingDistance; }
        set { stoppingDistance = value; }
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

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector2 RangeTimeBetweenAttacks
    {
        get { return rangeTimeBetweenAttacks; }
        set { rangeTimeBetweenAttacks = value; }
    }

    public float DistanceToAttack
    {
        get { return distanceToAttack; }
        set { distanceToAttack = value; }
    }

    #endregion
}