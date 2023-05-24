using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<UnitStats> units = new List<UnitStats>();
    [SerializeField] private Role role;
    [SerializeField] private Slider slider;
    [SerializeField] private float timeOfAbility = 5f;
    [SerializeField] private float value = 10;

    private bool abilityInUse = false;
    private bool canUseAbility = false;
    private float timeRemaining;

    private UnitStats unitStats;

    public enum Role
    {
        Heal,
        Boost
    }

    private void Start()
    {
        unitStats = GetComponent<UnitStats>();
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
                    units[i].Health += (int)value;
                }

                /*switch (role)
                {
                    case Role.Heal:
                        for (int i = 0; i < units.Count; i++)
                        {
                            Debug.Log($"Ability Heal {units[i]} + {value}");
                            units[i].Health += (int)value;
                        }
                        break;
                    case Role.Boost:
                        for (int i = 0; i < units.Count; i++)
                        {
                            Debug.Log($"Ability Boost {units[i]} + {value}");
                            units[i].Boost(value);
                        }
                        break;
                    default:
                        break;
                }*/

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

    public void AddUnitMovement(UnitStats unit)
    {
        if (unit.GetTeam == unitStats.GetTeam && !units.Contains(unit)) units.Add(unit);
    }

    public void RemoveUnitMovement(UnitStats unit)
    {
        if (unit.GetTeam == unitStats.GetTeam && units.Contains(unit))
        {
            if (role == Role.Boost) unit.Boost(-1);

            units.Remove(unit);
        }
    }
}