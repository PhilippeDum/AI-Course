using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject defoggerMesh;

    private BuildingDatas datas;

    public GameManager gameManager;
    public BlueprintManager blueprintManager;

    public BuildingDatas Datas
    {
        get { return datas; }
        set { datas = value; }
    }

    public GameObject DefoggerMesh
    {
        get { return defoggerMesh; }
        set { defoggerMesh = value; }
    }

    public virtual void OnEnable()
    {
        gameManager = GameManager.instance;
        blueprintManager = BlueprintManager.instance;

        Cost();
        PlayPlacementAnimation();
        PlayPlacementSound();
        SetParameters();

        blueprintManager.OnBuildingPlaced += BuildingPlacementComplete;
    }

    private void Cost()
    {
        Debug.Log($"Cost ({datas})");

        gameManager.ApplyCost(datas.CostType, datas.Cost);
    }

    private void PlayPlacementAnimation()
    {

    }

    private void PlayPlacementSound()
    {

    }

    private void SetParameters()
    {

    }

    public virtual void BuildingPlacementComplete()
    {
        blueprintManager.OnBuildingPlaced -= BuildingPlacementComplete;

        datas.AddBuildingCount();

        Debug.Log($"{name} placed on terrain");
    }
}