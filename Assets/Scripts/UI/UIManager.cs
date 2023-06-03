using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("References")]
    [SerializeField] private GameObject resources;
    [SerializeField] private GameObject infos;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject kingOptions;
    [SerializeField] private GameObject upgradeOptions;

    [Header("UI")]
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject questsUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private Text woodUI;
    [SerializeField] private Text silverUI;
    [SerializeField] private Text goldUI;

    private UnitManager currentUnit;
    private bool showingInfos;

    private Button pawnProductButton;
    private string pawnButtonText = "Pion";
    private Button riderProductButton;
    private string riderButtonText = "Cavalier";

    #region Getters / Setters

    public GameObject MainUI
    {
        get { return mainUI; }
        set { mainUI = value; }
    }

    public GameObject QuestsUI
    {
        get { return questsUI; }
        set { questsUI = value; }
    }

    public GameObject EndGameUI
    {
        get { return endGameUI; }
        set { endGameUI = value; }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        showingInfos = false;

        HandleUI(false);

        endGameUI.SetActive(false);
    }

    public void HandleEndGameUI(bool endGame)
    {
        if (endGame)
        {
            Debug.Log($"Victory !! EnemyManager defeat");
            endGameUI.GetComponentInChildren<Text>().text = $"Victory !! EnemyManager defeat";
        }
        else
        {
            Debug.Log($"Defeat !! EnemyManager victory");
            endGameUI.GetComponentInChildren<Text>().text = $"Defeat !! EnemyManager victory";
        }

        mainUI.SetActive(false);
        questsUI.SetActive(false);
        endGameUI.SetActive(true);
    }

    #region UI

    public void HandleUI(bool state, bool isKing = false, bool isUpgrade = false)
    {
        resources.SetActive(state);
        infos.SetActive(state);
        options.SetActive(state);

        if (isKing || isUpgrade) mainUI.GetComponent<InterfaceRefs>().OptionsDescription.gameObject.SetActive(false);
        else
        {
            mainUI.GetComponent<InterfaceRefs>().OptionsDescription.gameObject.SetActive(true);
            mainUI.GetComponent<InterfaceRefs>().BuildingsButtons.SetActive(false);
        }

        kingOptions.SetActive(isKing);
        upgradeOptions.SetActive(isUpgrade);
    }

    public void ShowInfos(UnitManager unit)
    {
        if (!showingInfos)
        {
            showingInfos = true;

            currentUnit = unit;

            mainUI.GetComponent<InterfaceRefs>().InfosTitle.text = $"{currentUnit.UnitData.Name}";
            mainUI.GetComponent<InterfaceRefs>().InfosDescription.text = currentUnit.UnitData.Description;

            //if (currentUnit.CompareTag("King"))
            if (currentUnit.UnitData.TypeUnit == Unit.UnitType.King && currentUnit.UnitData.TeamUnit == Unit.UnitTeam.Player)
            {
                HandleUI(true, true);

                SetupBuildingsButtons();

                SetupKingButtons(currentUnit.GetComponent<Production>());

                currentUnit.GetComponent<Production>().UpdateKingText();
            }
            else if (currentUnit.CompareTag("Upgrade") && currentUnit.UnitData.TeamUnit == Unit.UnitTeam.Player) // Building
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

                mainUI.GetComponent<InterfaceRefs>().OptionsDescription.text = $"{unitManager.UnitData.Health} / {unitManager.UnitData.MaxHealth}";
            }

            showingInfos = false;
        }
    }

    private void SetupBuildingsButtons()
    {
        mainUI.GetComponent<InterfaceRefs>().BuildingsButtons.SetActive(true);
    }

    private void SetupKingButtons(Production production)
    {
        pawnProductButton = kingOptions.transform.GetChild(0).GetComponent<Button>();
        pawnProductButton.name = pawnButtonText;

        pawnProductButton.onClick.RemoveAllListeners();
        pawnProductButton.onClick.AddListener(delegate { production.AddPawnToProduction(); });

        riderProductButton = kingOptions.transform.GetChild(1).GetComponent<Button>();
        riderProductButton.name = riderButtonText;

        riderProductButton.onClick.RemoveAllListeners();
        riderProductButton.onClick.AddListener(delegate { production.AddRiderToProduction(); });
    }

    private void SetupUpgradeButton()
    {
        Button firstButton = upgradeOptions.transform.GetChild(0).GetComponent<Button>();
        //firstButton.onClick.AddListener(delegate { production.AddPawnToProduction(); });
        // Add upgrade function to onClick
    }

    public void UpdateText(string text, int pawnsCount, int ridersCount)
    {
        if (pawnProductButton != null)
            pawnProductButton.GetComponentInChildren<Text>().text = $"{pawnButtonText} ({pawnsCount})";

        if (riderProductButton != null)
            riderProductButton.GetComponentInChildren<Text>().text = $"{riderButtonText} ({ridersCount})";

        //mainUI.GetComponent<InterfaceRefs>().InfosDescription.text = text;
    }

    public void HandleResourcesUI(int wood, int silver, int gold)
    {
        /*woodUI.text = $"{wood}";
        silverUI.text = $"{silver}";
        goldUI.text = $"{gold}";*/
    }

    #endregion
}