using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingsButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InterfaceRefs refs;
    private BlueprintManager blueprintManager;

    private string buildingName;

    private void Awake()
    {
        refs = GetComponentInParent<InterfaceRefs>();
        refs.BuildingCost.SetActive(false);

        blueprintManager = BlueprintManager.instance;

        buildingName = GetComponentInChildren<Text>().text;
    }

    private void OnEnable()
    {
        ShowLimits();
    }

    private void ShowLimits()
    {
        BuildingDatas datas = blueprintManager.GetBuildingDatas(name);

        int currentCount = datas.CountBuildings;
        int maxLimit = datas.MaxCountBuildings;

        GetComponentInChildren<Text>().text = $"{buildingName}\n({currentCount}/{maxLimit})";
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