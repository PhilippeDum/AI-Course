using UnityEngine;

[System.Serializable]
public class BuildingDatas
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int placementCost;
    [SerializeField] private Resources costType;
    [SerializeField] private int resourceGain;
    [SerializeField] private int energyCost;
    [SerializeField] private int healthPoint;
    [SerializeField] private int countBuildings = 0;
    [SerializeField] private int maxCountBuildings = 2;

    public string Name => name;
    public string Description => description;
    public GameObject Prefab => prefab;
    public int PlacementCost => placementCost;
    public Resources CostType => costType;
    public int ResourceGain => resourceGain;
    public int EnergyCost => energyCost;
    public int HealthPoint => healthPoint;
    public int CountBuildings => countBuildings;
    public int MaxCountBuildings => maxCountBuildings;

    public void AddBuildingCount()
    {
        countBuildings++;
    }
}