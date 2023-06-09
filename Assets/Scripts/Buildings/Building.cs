using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject defoggerMesh;
    [SerializeField] private GameObject selection;
    [SerializeField] private Unit.UnitTeam team;

    private BuildingDatas datas;

    public GameManager gameManager;
    public BlueprintManager blueprintManager;

    #region Getters / Setters

    public GameObject DefoggerMesh
    {
        get { return defoggerMesh; }
        set { defoggerMesh = value; }
    }

    public GameObject Selection
    {
        get { return selection; }
        set { selection = value; }
    }

    public Unit.UnitTeam Team
    {
        get { return team; }
        set { team = value; }
    }

    public BuildingDatas Datas
    {
        get { return datas; }
        set { datas = value; }
    }

    #endregion

    public virtual void OnEnable()
    {
        gameManager = GameManager.instance;
        blueprintManager = BlueprintManager.instance;

        ApplyPlacementCost();

        blueprintManager.OnBuildingPlaced += BuildingPlacementComplete;
    }

    private void ApplyPlacementCost()
    {
        gameManager.ApplyCost(datas.CostType, datas.PlacementCost);
    }

    public virtual void BuildingPlacementComplete()
    {
        blueprintManager.OnBuildingPlaced -= BuildingPlacementComplete;

        datas.AddBuildingCount();
    }
}