using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingsButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InterfaceRefs refs;
    BlueprintManager blueprintManager;

    private void Start()
    {
        refs = GetComponentInParent<InterfaceRefs>();
        refs.BuildingCost.SetActive(false);

        blueprintManager = BlueprintManager.instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetBuildingInfos(eventData.pointerEnter.transform.parent.name);

        refs.BuildingCost.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        refs.BuildingCost.SetActive(false);
    }

    private void GetBuildingInfos(string buildingName)
    {
        BuildingDatas buildingDatas = blueprintManager.GetBuildingDatas(buildingName);

        if (buildingDatas == null) return;

        Sprite costSprite = blueprintManager.GetBuildingCostSprite(buildingDatas.CostType);

        if (costSprite == null) return;

        refs.SetCostUI(buildingDatas.Cost, costSprite);
    }
}