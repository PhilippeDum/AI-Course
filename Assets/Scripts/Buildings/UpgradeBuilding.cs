using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuilding : Building
{
    [Header("References")]
    [SerializeField] private Slider upgradeSlider;
    [SerializeField] private List<UnitManager> units;

    private UIManager uiManager;

    private Button upgradeButton;

    #region Getters / Setters

    public List<UnitManager> Units
    {
        get { return units; }
        set { units = value; }
    }

    #endregion

    public override void OnEnable()
    {
        base.OnEnable();

        uiManager = UIManager.instance;

        upgradeButton = uiManager.UpgradeOptions.transform.GetComponentInChildren<Button>();
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeUnits);
    }

    private void Update()
    {
        upgradeSlider.transform.parent.LookAt(Camera.main.transform.position);
    }

    private void UpgradeUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetUnitData(units[i].UnitDataUpgraded);
        }
    }
}