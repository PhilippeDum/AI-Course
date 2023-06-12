using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBuilding : Building
{
    [Header("References")]
    [SerializeField] private Slider upgradeSlider;
    [SerializeField] private List<UnitManager> units;

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

        Something();
    }

    private void Something()
    {

    }

    private void Update()
    {
        upgradeSlider.transform.parent.LookAt(Camera.main.transform.position);
    }
}