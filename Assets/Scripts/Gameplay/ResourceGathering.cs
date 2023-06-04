using System.Collections;
using System.Numerics;
using UnityEngine;

public class ResourceGathering : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject elementModel;
    [SerializeField] private GameObject selection;
    [SerializeField] private string elementName;
    [SerializeField] private string elementDescription;
    [SerializeField] private Resources resourceType;
    [SerializeField] private int quantityDrop = 5;
    [SerializeField] private float timeCollect = 3f;
    [SerializeField] private float timeRespawn = 10f;

    private GameManager gameManager;

    private bool isCollecting = false;

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

    public Resources ResourceType
    {
        get { return resourceType; }
        set { resourceType = value; }
    }

    public float TimeCollect
    {
        get { return timeCollect; }
        set { timeCollect = value; }
    }

    #endregion

    private void Start()
    {
        gameManager = GameManager.instance;

        RandomYRotation();
    }

    private void RandomYRotation()
    {
        UnityEngine.Quaternion rotation = transform.rotation;

        rotation.y = Random.Range(0f, 360f);

        transform.rotation = rotation;
    }

    public void Collect(UnitManager currentWorker)
    {
        if (!isCollecting)
        {
            isCollecting = true;

            StartCoroutine(Collecting(currentWorker));
        }
    }

    private IEnumerator Collecting(UnitManager currentWorker)
    {
        yield return new WaitForSeconds(timeCollect);

        elementModel.SetActive(false);

        if (selection.activeSelf) selection.SetActive(false);

        gameManager.AddResouce(resourceType, quantityDrop);

        yield return new WaitForSeconds(timeRespawn);

        elementModel.SetActive(true);

        isCollecting = false;

        currentWorker.IsWorking = false;
    }
}