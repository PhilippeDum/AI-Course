using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerCours : MonoBehaviour
{
    public static UIManagerCours instance;

    [SerializeField] private TMP_Text tiberium;
    [SerializeField] private Slider energy;
    [SerializeField] private Slider energyUsed;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        UpdateTiberium(0);
        UpdateMaxEnergy(100);
        UpdateEnergyUI(25);
        UpdateEnergyUsedUI(0);
    }

    public void UpdateTiberium(int value)
    {
        tiberium.text = $"{value}";
    }

    public void UpdateMaxEnergy(int value)
    {
        energy.maxValue = value;
        energyUsed.maxValue = value;
    }

    public void UpdateEnergyUI(int value)
    {
        energy.value = value;
    }

    public void UpdateEnergyUsedUI(int value)
    {
        energyUsed.value = value;
    }
}