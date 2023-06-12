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
    [SerializeField] private GameObject selectionsInfos;

    private UnitManager currentUnit;
    private ResourceGathering currentElement;
    private Building currentBuilding;
    private bool showingInfos;

    private Button pawnProductButton;
    private string pawnButtonText = "Pion";
    private Button riderProductButton;
    private string riderButtonText = "Cavalier";

    #region Getters / Setters

    public GameObject UpgradeOptions
    {
        get { return upgradeOptions; }
        set { upgradeOptions = value; }
    }

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

        selectionsInfos.SetActive(false);
        endGameUI.SetActive(false);

        pawnProductButton = kingOptions.transform.GetChild(0).GetComponent<Button>();
        riderProductButton = kingOptions.transform.GetChild(1).GetComponent<Button>();
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

    public void ShowUnitInfos(UnitManager unit)
    {
        if (unit == null || showingInfos) return;

        showingInfos = true;

        currentUnit = unit;

        if (currentUnit == null)
        {
            showingInfos = false;
            return;
        }

        mainUI.GetComponent<InterfaceRefs>().InfosTitle.text = $"{currentUnit.UnitData.Name}";
        mainUI.GetComponent<InterfaceRefs>().InfosDescription.text = currentUnit.UnitData.Description;

        if (currentUnit.UnitData.TypeUnit == Unit.UnitType.King && currentUnit.UnitData.TeamUnit == Unit.UnitTeam.Player)
        {
            HandleUI(true, true);

            SetupBuildingsButtons(true);

            currentUnit.GetComponent<Production>().UpdateKingText();

            mainUI.GetComponent<InterfaceRefs>().OptionsTitle.text = "Gestion des troupes";
        }
        else
        {
            mainUI.GetComponent<InterfaceRefs>().OptionsTitle.text = "Unité";

            HandleUI(true);
        }

        if (currentUnit != null)
        {
            UnitManager unitManager = currentUnit.GetComponent<UnitManager>();

            mainUI.GetComponent<InterfaceRefs>().OptionsDescription.text = $"{unitManager.UnitData.Health} / {unitManager.UnitData.MaxHealth}";
        }

        showingInfos = false;
    }

    public void ShowResourceInfos(ResourceGathering element)
    {
        if (element == null || showingInfos) return;

        showingInfos = true;

        currentElement = element;

        if (currentElement == null)
        {
            showingInfos = false;
            return;
        }

        mainUI.GetComponent<InterfaceRefs>().InfosTitle.text = $"{currentElement.ElementName}";
        mainUI.GetComponent<InterfaceRefs>().InfosDescription.text = currentElement.ElementDescription;

        HandleUI(true);

        SetupBuildingsButtons(false);

        if (currentElement != null)
        {
            mainUI.GetComponent<InterfaceRefs>().OptionsTitle.text = "Ressource";

            mainUI.GetComponent<InterfaceRefs>().OptionsDescription.text = $"Temps de collecte : {currentElement.TimeCollect}s";
        }

        showingInfos = false;
    }

    public void ShowBuildingInfos(Building building)
    {
        if (building == null || showingInfos) return;

        showingInfos = true;

        currentBuilding = building;

        if (currentBuilding == null || currentBuilding.Datas == null)
        {
            showingInfos = false;
            return;
        }

        mainUI.GetComponent<InterfaceRefs>().InfosTitle.text = $"{currentBuilding.Datas.Name}";
        mainUI.GetComponent<InterfaceRefs>().InfosDescription.text = currentBuilding.Datas.Description;

        if (building.CompareTag("Upgrade"))
            HandleUI(true, false, true);
        else
            HandleUI(true);

        SetupBuildingsButtons(false);

        if (currentBuilding != null)
        {
            mainUI.GetComponent<InterfaceRefs>().OptionsTitle.text = "Ressource";
        }

        showingInfos = false;
    }

    private void SetupBuildingsButtons(bool state)
    {
        mainUI.GetComponent<InterfaceRefs>().BuildingsButtons.SetActive(state);
    }

    public void UpdateText(string text, int pawnsCount, int ridersCount)
    {
        if (pawnProductButton != null)
            pawnProductButton.GetComponentInChildren<Text>().text = $"{pawnButtonText} ({pawnsCount})";

        if (riderProductButton != null)
            riderProductButton.GetComponentInChildren<Text>().text = $"{riderButtonText} ({ridersCount})";
    }

    public void HandleResourcesUI(int wood, int iron, int gold)
    {
        InterfaceRefs refs = mainUI.GetComponent<InterfaceRefs>();

        refs.WoodCount.text = $"{wood}";
        refs.IronCount.text = $"{iron}";
        refs.GoldCount.text = $"{gold}";
    }

    public void ShowGameInfos()
    {
        if (selectionsInfos.activeSelf) selectionsInfos.SetActive(false);
        else selectionsInfos.SetActive(true);
    }

    #endregion
}