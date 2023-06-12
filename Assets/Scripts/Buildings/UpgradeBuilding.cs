using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : Building
{
    [SerializeField] private List<UnitManager> units;

    public List<UnitManager> Units
    {
        get { return units; }
        set { units = value; }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        Something();
    }

    private void Something()
    {

    }
}