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

    private Element currentElement;
    private KingManager kingManager;
    private bool showingInfos;

    private void Start()
    {
        kingManager = FindObjectOfType<KingManager>();

        showingInfos = false;

        HandleUI(false);

        SetupKingButtons();
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

    public void ShowInfos(Element element)
    {
        if (!showingInfos)
        {
            showingInfos = true;

            currentElement = element;

            infosTitle.text = $"Infos '{currentElement.ElementName}'";
            infosDescription.text = currentElement.ElementDescription;

            if (currentElement.CompareTag("King"))
            {
                HandleUI(true, true);

                kingManager.UpdateKingText();
            }
            else if (currentElement.CompareTag("Upgrade"))
            {
                HandleUI(true, false, true);
            }
            else
            {
                HandleUI(true);
            }

            if (currentElement.CompareTag("King") || currentElement.CompareTag("Unit") || currentElement.CompareTag("Enemy"))
            {
                Stats stats = currentElement.GetComponent<Stats>();

                optionsDescription.text = $"{stats.Health} / {stats.MaxHealth}";
            }

            showingInfos = false;
        }
    }

    private void SetupKingButtons()
    {
        Button firstButton = kingOptions.transform.GetChild(0).GetComponent<Button>();
        firstButton.name = "Pawn";
        firstButton.onClick.AddListener(delegate { kingManager.AddPawnToProduction(); });

        Button secondButton = kingOptions.transform.GetChild(1).GetComponent<Button>();
        secondButton.name = "Rider";
        secondButton.onClick.AddListener(delegate { kingManager.AddRiderToProduction(); });
    }

    private void SetupUpgradeButton()
    {
        Button firstButton = upgradeOptions.transform.GetChild(0).GetComponent<Button>();
        //firstButton.onClick.AddListener(delegate { kingManager.AddPawnToProduction(); });
        // Add upgrade function to onClick
    }

    public void UpdateText(string text)
    {
        infosDescription.text = text;
    }

    #endregion
}