using UnityEngine;

[System.Serializable]
public class BuildingDatas
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int tiberiumCost;
    [SerializeField] private int energyGain;
    [SerializeField] private int energyCost;
    [SerializeField] private int healthPoint;

    public string Name => name;
    public GameObject Prefab => prefab;
    public int TiberiumCost => tiberiumCost;
    public int EnergyGain => energyGain;
    public int EnergyCost => energyCost;
    public int HealthPoint => healthPoint;
}