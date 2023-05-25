using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<UnitManager> units = new List<UnitManager>();
    [SerializeField] private Role role;
    [SerializeField] private Slider slider;
    [SerializeField] private float timeOfAbility = 5f;
    [SerializeField] private float value = 10;

    private bool abilityInUse = false;
    private bool canUseAbility = false;
    private float timeRemaining;

    private UnitManager unitManager;

    private void Start()
    {
        unitManager = GetComponent<UnitManager>();
    }

    public enum Role
    {
        Heal,
        Boost
    }

    private void Update()
    {
        switch (role)
        {
            case Role.Heal:
                HandleAbility();
                break;
            case Role.Boost:
                HandleBoost();
                break;
            default:
                Debug.Log($"Error : switch role");
                break;
        }

        //HandleAbility();
    }

    private void HandleAbility()
    {
        // Active Ability
        if (units.Count > 0 && !abilityInUse) canUseAbility = true;

        // Handle Setup Ability
        if (canUseAbility)
        {
            canUseAbility = false;

            timeRemaining = timeOfAbility;

            slider.maxValue = timeRemaining;

            abilityInUse = true;
        }

        // Handle Timer + Use Ability
        if (abilityInUse)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                slider.value = timeRemaining;
            }
            else
            {
                for (int i = 0; i < units.Count; i++)
                {
                    Debug.Log($"Ability Heal {units[i]} + {value}");
                    units[i].UnitData.Health += (int)value;
                }

                abilityInUse = false;
            }
        }
    }

    private void HandleBoost()
    {
        if (units.Count <= 0) return;

        abilityInUse = true;

        for (int i = 0; i < units.Count; i++)
        {
            units[i].Boost(value);
        }
    }

    public void AddUnit(UnitManager unit)
    {
        if (unit.UnitData.TeamUnit == unitManager.UnitData.TeamUnit && !units.Contains(unit)) units.Add(unit);
    }

    public void RemoveUnit(UnitManager unit)
    {
        if (unit.UnitData.TeamUnit == unitManager.UnitData.TeamUnit && units.Contains(unit))
        {
            if (role == Role.Boost) unit.Boost(-1);

            units.Remove(unit);
        }
    }
}