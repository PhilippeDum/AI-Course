using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject resources;
    [SerializeField] private GameObject infos;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject kingOptions;
    [SerializeField] private GameObject upgradeOptions;

    [Header("UI")]
    [SerializeField] private Text infosTitle;
    [SerializeField] private Text infosDescription;
    [SerializeField] private Text optionsTitle;
    [SerializeField] private Text optionsDescription;

    private UnitManager currentUnit;
    private bool showingInfos;

    private void Start()
    {
        showingInfos = false;

        HandleUI(false);
    }

    #region UI

    public void HandleUI(bool state, bool isKing = false, bool isUpgrade = false)
    {
        resources.SetActive(state);
        infos.SetActive(state);
        options.SetActive(state);

        if (isKing || isUpgrade) optionsDescription.gameObject.SetActive(false);
        else optionsDescription.gameObject.SetActive(true);

        kingOptions.SetActive(isKing);
        upgradeOptions.SetActive(isUpgrade);
    }

    public void ShowInfos(UnitManager unit)
    {
        if (!showingInfos)
        {
            showingInfos = true;

            currentUnit = unit;

            infosTitle.text = $"Infos '{currentUnit.UnitData.Name}'";
            infosDescription.text = currentUnit.UnitData.Description;

            //if (currentUnit.CompareTag("King"))
            if (currentUnit.UnitData.TypeUnit == Unit.UnitType.King)
            {
                HandleUI(true, true);

                SetupKingButtons(currentUnit.GetComponent<Production>());

                currentUnit.GetComponent<Production>().UpdateKingText();
            }
            else if (currentUnit.CompareTag("Upgrade")) // Building
            {
                HandleUI(true, false, true);
            }
            else
            {
                HandleUI(true);
            }

            //if (currentUnit.CompareTag("King") || currentUnit.CompareTag("Unit") || currentUnit.CompareTag("Enemy"))
            if (currentUnit != null)
            {
                UnitManager unitManager = currentUnit.GetComponent<UnitManager>();

                optionsDescription.text = $"{unitManager.UnitData.Health} / {unitManager.UnitData.MaxHealth}";
            }

            showingInfos = false;
        }
    }

    private void SetupKingButtons(Production production)
    {
        Debug.Log($"SetupKingButtons");

        Button firstButton = kingOptions.transform.GetChild(0).GetComponent<Button>();
        firstButton.name = "Pawn";

        firstButton.onClick.RemoveAllListeners();
        firstButton.onClick.AddListener(delegate { production.AddPawnToProduction(); });

        Button secondButton = kingOptions.transform.GetChild(1).GetComponent<Button>();
        secondButton.name = "Rider";

        secondButton.onClick.RemoveAllListeners();
        secondButton.onClick.AddListener(delegate { production.AddRiderToProduction(); });
    }

    private void SetupUpgradeButton()
    {
        Button firstButton = upgradeOptions.transform.GetChild(0).GetComponent<Button>();
        //firstButton.onClick.AddListener(delegate { production.AddPawnToProduction(); });
        // Add upgrade function to onClick
    }

    public void UpdateText(string text)
    {
        infosDescription.text = text;
    }

    #endregion
}