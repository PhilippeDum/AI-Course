using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGathering : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject elementModel;
    [SerializeField] private GameObject selection;
    [SerializeField] private Slider respawnSlider;
    [SerializeField] private Color collectColor;
    [SerializeField] private Color respawnColor;
    [SerializeField] private string elementName;
    [SerializeField] private string elementDescription;
    [SerializeField] private List<Resources> resourcesType;
    [SerializeField] private float randomRate = 30f;
    [SerializeField] private int quantityDrop = 5;
    [SerializeField] private float timeCollect = 3f;
    [SerializeField] private float timeRespawn = 10f;

    private GameManager gameManager;

    private float timeRemaining = 0f;

    private bool isCollecting = false;
    private bool isRespawning = false;

    private UnitManager currentWorker;

    #region Getters / Setters

    public GameObject Selection
    {
        get { return selection; }
        set { selection = value; }
    }

    public string ElementName
    {
        get { return elementName; }
        set { elementName = value; }
    }

    public string ElementDescription
    {
        get { return elementDescription; }
        set { elementDescription = value; }
    }

    public List<Resources> ResourcesType
    {
        get { return resourcesType; }
        set { resourcesType = value; }
    }

    public float TimeCollect
    {
        get { return timeCollect; }
        set { timeCollect = value; }
    }

    public bool IsRespawning
    {
        get { return isRespawning; }
        set { isRespawning = value; }
    }

    #endregion

    private void Start()
    {
        gameManager = GameManager.instance;

        RandomYRotation();

        respawnSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        respawnSlider.transform.parent.LookAt(Camera.main.transform.position);

        Collecting();
    }

    private void RandomYRotation()
    {
        Quaternion rotation = transform.rotation;

        rotation.y = Random.Range(0f, 360f);

        transform.rotation = rotation;
    }

    public void Collect(UnitManager worker)
    {
        if (!isCollecting)
        {
            isCollecting = true;
            timeRemaining = timeCollect;
            currentWorker = worker;

            respawnSlider.maxValue = timeRemaining;
            respawnSlider.value = timeRemaining;

            respawnSlider.transform.GetChild(1).GetComponent<Image>().color = collectColor;
        }
    }

    private void Collecting()
    {
        if (!isCollecting && !isRespawning) return;

        respawnSlider.gameObject.SetActive(true);

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;

            respawnSlider.value = timeRemaining;
        }
        else
        {
            timeRemaining = 0f;

            // Collecting
            if (isCollecting)
            {
                isCollecting = false;

                elementModel.SetActive(false);

                if (selection.activeSelf) selection.SetActive(false);

                Resources randomResource;

                if (resourcesType.Count > 1)
                    randomResource = GetRandomResource();
                else
                    randomResource = resourcesType[0];

                gameManager.AddResouce(randomResource, quantityDrop);

                currentWorker.IsWorking = false;

                // Active Respawn

                isRespawning = true;

                timeRemaining = timeRespawn;

                respawnSlider.maxValue = timeRemaining;
                respawnSlider.value = timeRemaining;

                respawnSlider.transform.GetChild(1).GetComponent<Image>().color = respawnColor;

                respawnSlider.gameObject.SetActive(true);

                return;
            }

            // Respawning
            if (isRespawning)
            {
                isRespawning = false;

                respawnSlider.gameObject.SetActive(false);

                elementModel.SetActive(true);
            }
        }
    }

    private Resources GetRandomResource()
    {
        int random = Random.Range(0, 100);

        if (random <= randomRate) return resourcesType[1];
        else return resourcesType[0];
    }
}