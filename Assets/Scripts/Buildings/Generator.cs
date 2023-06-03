using UnityEngine;

public class Generator : Building
{
    public override void OnEnable()
    {
        base.OnEnable();

        GainEnergy();
    }

    private void GainEnergy()
    {
        Debug.Log($"GainEnergy : {Datas.EnergyGain}");
        gameManager.AddFreeEnergy(Datas.EnergyGain);
    }
}