using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Quest;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform brokenBridgeTarget;
    [SerializeField] private GameObject king;
    [SerializeField] private GameObject kingEnemy;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject questsUI;

    private bool gameFinished;

    [Header("Quests")]
    [SerializeField] private List<Quest> quests;
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;
    [SerializeField] private QuestType currentQuestType;

    private int countPawns = 0;
    private int countRiders = 0;

    private bool production;
    private bool reparation;

    private bool changeQuest;

    [Header("Camera - Broken Bridge Datas")]
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float timeBeforeShowing = 2f;
    [SerializeField] private float timeShowingBrokenBridge = 3f;
    [SerializeField] private bool startShowing = false;
    [SerializeField] private bool stopShowing = false;

    private bool showing;

    private Vector3 lastPosition;

    private int countPawnsDetection = 0;
    private int countRidersDetection = 0;
    private bool currentBridgeRepaired;

    private BridgeReparation currentBridgeReparation;
    private EnemyManager enemyManager;

    #region Getters / Setters

    public bool GameFinished
    {
        get { return gameFinished; }
        set { gameFinished = value; }
    }

    public QuestType CurrentQuestType
    {
        get { return currentQuestType; }
        set { currentQuestType = value; }
    }

    public bool StartShowing
    {
        get { return startShowing; }
        set { startShowing = value; }
    }

    public int CountPawns
    {
        get { return countPawns; }
        set { countPawns = value; }
    }

    public int CountRiders
    {
        get { return countRiders; }
        set { countRiders = value; }
    }

    public int CountPawnsDetection
    {
        get { return countPawnsDetection; }
        set { countPawnsDetection = value; }
    }

    public int CountRidersDetection
    {
        get { return countRidersDetection; }
        set { countRidersDetection = value; }
    }

    public BridgeReparation CurrentBridgeReparation
    {
        get { return currentBridgeReparation; }
        set { currentBridgeReparation = value; }
    }

    public bool CurrentBridgeRepaired
    {
        get { return currentBridgeRepaired; }
        set { currentBridgeRepaired = value; }
    }

    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();

        gameFinished = false;

        production = false;
        reparation = false;

        changeQuest = false;

        currentBridgeRepaired = false;

        currentQuestType = QuestType.Production;

        showing = false;

        endGameUI.SetActive(false);
    }

    private void Update()
    {
        if (gameFinished) return;

        if (king == null) EndGame(false);
        if (kingEnemy == null) EndGame(true);

        if (Input.GetKeyDown(KeyCode.Escape) && !startShowing) startShowing = true;

        if (startShowing && !stopShowing) ActiveCameraShow();

        if (stopShowing && !startShowing) DeactiveCameraShow();

        HandleQuests();
    }

    #region Quests

    private void HandleQuests()
    {
        switch (currentQuestType)
        {
            case QuestType.Production:
                SetQuest(currentQuestType, countPawns, countRiders);
                QuestProduction();
                break;
            case QuestType.Reparation:
                SetQuest(currentQuestType, countPawnsDetection, countRidersDetection);
                QuestReparation();
                break;
            case QuestType.Attack:
                SetQuest(currentQuestType, countPawns, countRiders);
                break;
            default:
                Debug.LogError($"Error : quest switch");
                break;
        }
    }

    private void QuestProduction()
    {
        Quest currentQuest = GetCurrentQuest(currentQuestType);

        if (countPawns >= currentQuest.requiredAmountPawn && countRiders >= currentQuest.requiredAmountRider && !production)
        {
            production = true;

            questDescription.text = "Terminé !";

            if (!startShowing) startShowing = true;

            enemyManager.StartProduction();

            currentQuestType = QuestType.Reparation;
        }
    }

    private void QuestReparation()
    {
        Quest currentQuest = GetCurrentQuest(currentQuestType);

        if (countPawnsDetection >= currentQuest.requiredAmountPawn && countRidersDetection >= currentQuest.requiredAmountRider && !reparation && !currentBridgeRepaired)
        {
            reparation = true;

            if (currentBridgeReparation == null) return;

            currentBridgeReparation.Repair();
        }

        if (currentBridgeReparation != null && currentBridgeRepaired && !changeQuest && reparation)
        {
            changeQuest = true;

            questDescription.text = "Terminé !";

            changeQuest = false;

            currentQuestType = QuestType.Attack;

            currentBridgeRepaired = false;
        }
    }

    public void EndGame(bool endGame)
    {
        gameFinished = true;

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

    private void SetQuest(QuestType questType, int countPawns, int countRiders)
    {
        Quest quest = null;

        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questType == questType)
                quest = quests[i];
        }

        if (quest == null) return;

        questName.text = quest.questName;
        questDescription.text = $"{quest.questDescription}\npion(s) : {countPawns}/{quest.requiredAmountPawn}\ncavalier(s) : {countRiders}/{quest.requiredAmountRider}";
    }

    public Quest GetCurrentQuest(QuestType questType)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].questType == questType)
                return quests[i]; 
        }

        return null;
    }

    #endregion

    #region Handle Camera to Broken Bridge

    private void ActiveCameraShow()
    {
        if (!showing)
        {
            showing = true;

            cameraRoot.GetComponent<CameraMotion>().CanMove = false;

            lastPosition = cameraRoot.position;

            StartCoroutine(ShowBrokenBridge());
        }        
    }

    private IEnumerator ShowBrokenBridge()
    {
        yield return new WaitForSeconds(timeBeforeShowing);
        
        while (Distance(cameraRoot.position, brokenBridgeTarget.position) > 5)
        {
            cameraRoot.position = Vector3.Lerp(cameraRoot.position, brokenBridgeTarget.position, Time.deltaTime * smoothing);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(timeShowingBrokenBridge);

        stopShowing = true;
        startShowing = false;
    }

    private void DeactiveCameraShow()
    {
        StartCoroutine(ReturnLastPosition());
    }

    private IEnumerator ReturnLastPosition()
    {
        while (Distance(cameraRoot.position, brokenBridgeTarget.position) > 5)
        {
            cameraRoot.position = Vector3.Lerp(cameraRoot.position, lastPosition, Time.deltaTime * smoothing);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        cameraRoot.GetComponent<CameraMotion>().CanMove = true;

        stopShowing = false;
    }

    #endregion

    public float Distance(Vector3 pos1, Vector3 pos2)
    {
        Vector3 direction = pos1 - pos2;

        float distance = direction.magnitude;

        return distance;
    }
}