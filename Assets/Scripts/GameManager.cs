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

    [Header("Quests")]
    [SerializeField] private List<Quest> quests;
    [SerializeField] private Text questName;
    [SerializeField] private Text questDescription;
    [SerializeField] private QuestType currentQuestType;

    private int countPawns = 0;
    private int countRiders = 0;

    [Header("Camera - Broken Bridge Datas")]
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private float timeBeforeShowing = 2f;
    [SerializeField] private float timeShowingBrokenBridge = 3f;
    [SerializeField] private bool startShowing = false;
    [SerializeField] private bool stopShowing = false;

    private Vector3 lastPosition;

    #region Getters / Setters

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

    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentQuestType = QuestType.Production;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !startShowing) startShowing = true;

        if (startShowing) ActiveCameraShow();

        if (stopShowing) StartCoroutine(ReturnLastPosition());

        HandleQuests();
    }

    #region Quests

    private void HandleQuests()
    {
        switch (currentQuestType)
        {
            case QuestType.Production:
                SetQuest(currentQuestType);
                QuestProduction();
                break;
            case QuestType.Reparation:
                SetQuest(currentQuestType);
                QuestReparation();
                break;
            case QuestType.Attack:
                SetQuest(currentQuestType);
                QuestAttack();
                break;
            default:
                Debug.LogError($"Error : quest switch");
                break;
        }
    }

    private void QuestProduction()
    {
        if (countPawns >= GetCurrentQuest(currentQuestType).requiredAmountPawn && countRiders >= GetCurrentQuest(currentQuestType).requiredAmountRider)
        {
            currentQuestType = QuestType.Reparation;

            if (!startShowing) startShowing = true;
        }
    }

    private void QuestReparation()
    {

    }

    private void QuestAttack()
    {

    }

    private void SetQuest(QuestType questType)
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
        cameraRoot.GetComponent<CameraMotion>().CanMove = false;

        StartCoroutine(ShowBrokenBridge());
    }

    private IEnumerator ShowBrokenBridge()
    {
        yield return new WaitForSeconds(timeBeforeShowing);

        lastPosition = cameraRoot.position;

        cameraRoot.position = Vector3.Lerp(cameraRoot.position, brokenBridgeTarget.position, Time.deltaTime * smoothing);

        yield return new WaitForSeconds(timeShowingBrokenBridge);

        stopShowing = true;
        startShowing = false;
    }

    private IEnumerator ReturnLastPosition()
    {
        cameraRoot.position = Vector3.Lerp(cameraRoot.position, lastPosition, Time.deltaTime * smoothing);

        yield return new WaitForSeconds(timeShowingBrokenBridge);

        cameraRoot.GetComponent<CameraMotion>().CanMove = true;

        stopShowing = false;
    }

    #endregion
}