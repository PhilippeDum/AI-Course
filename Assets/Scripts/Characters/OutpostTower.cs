using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutpostTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<UnitStats> units = new List<UnitStats>();
    [SerializeField] private Slider healSlider;
    [SerializeField] private float timeOfHealing = 5f;
    [SerializeField] private int healValue = 10;

    private bool isHealing = false;
    private bool canHeal = false;
    private float timeRemaining;

    private UnitStats unitStats;

    private void Start()
    {
        unitStats = GetComponent<UnitStats>();
    }

    private void Update()
    {
        Heal();
    }

    private void Heal()
    {
        // Active Heal
        if (units.Count > 0 && !isHealing) canHeal = true;
        //if (units.Count > 0) canHeal = true;

        // Handle Setup Start Production
        if (canHeal)
        {
            canHeal = false;

            timeRemaining = timeOfHealing;

            healSlider.maxValue = timeRemaining;

            isHealing = true;
        }

        // Handle Timer + Unit produced
        if (isHealing)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                healSlider.value = timeRemaining;
            }
            else
            {
                for (int i = 0; i < units.Count; i++)
                {
                    Debug.Log($"Heal {units[i]} + {healValue}");
                    units[i].Health += healValue;
                }

                isHealing = false;
            }
        }
    }

    public void AddUnit(UnitStats unit)
    {
        if (unit.GetTeam == unitStats.GetTeam && !units.Contains(unit)) units.Add(unit);
    }

    public void RemoveUnit(UnitStats unit)
    {
        if (unit.GetTeam == unitStats.GetTeam && units.Contains(unit)) units.Remove(unit);
    }
}