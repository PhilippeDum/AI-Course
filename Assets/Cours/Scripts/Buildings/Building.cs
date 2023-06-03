using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingDatas datas;
    public GameManager gameManager;

    public BuildingDatas Datas
    {
        get { return datas; }
        set { datas = value; }
    }

    public virtual void OnEnable()
    {
        gameManager = GameManager.instance;

        CostTiberium();
        PlayPlacementAnimation();
        PlayPlacementSound();
        SetParameters();
    }

    private void CostTiberium()
    {
        Debug.Log($"CostTiberium");
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
}