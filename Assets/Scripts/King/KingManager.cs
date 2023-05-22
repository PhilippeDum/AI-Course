using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> pawnUnitPrefabs;
    [SerializeField] private List<GameObject> riderUnitPrefabs;
    [SerializeField] private Transform unitParent;
    [SerializeField] private Transform gathering;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private Vector3 center;
    [SerializeField] private GameObject canvasWorld;
    [SerializeField] private Slider productionSlider;
    [SerializeField] [Range(0, 1)] private float lerp = 0.5f;

    private GameObject currentCanvasWorld;

    private UIManager uiManager;

    [Header("Production Properties")]
    [SerializeField] private int pawnsToProduct = 0;
    [SerializeField] private int ridersToProduct = 0;
    [SerializeField] private float timeOfPawnProduction = 3f;
    [SerializeField] private float timeOfRiderProduction = 3f;
    [SerializeField] private List<Unit> units;
    [SerializeField] private List<Unit> selectedUnits;

    private bool inProduction;
    private float timeRemaining = 0f;
    private bool canStartProduction = false;

    // Formation
    private FormationBase _formation;

    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    private List<Vector3> points = new List<Vector3>();

    #region Getters / Setters

    public List<Unit> Units
    {
        get { return units; }
        set { units = value; }
    }

    public List<Unit> SelectedUnits
    {
        get { return selectedUnits; }
        set { selectedUnits = value; }
    }

    public Vector3 Center
    {
        get { return center; }
        set { center = value; }
    }

    #endregion

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        inProduction = false;

        center = gathering.position;
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        if (GetComponent<Stats>().IsDead) GameManager.instance.EndGame(true);

        HandleProduction();
        Producing(timeOfPawnProduction);

        Move();

        Clear();
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentCanvasWorld != null) Destroy(currentCanvasWorld);

            HandleMovement();
        }
    }

    private void Clear()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null) units.Remove(units[i]);
        }

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] == null) selectedUnits.Remove(selectedUnits[i]);
        }
    }

    #region Production

    private void HandleProduction()
    {
        if ((pawnsToProduct > 0 || ridersToProduct > 0) && !inProduction)
        {
            StartCoroutine(StartProduction());
        }

        productionSlider.value = Mathf.Lerp(productionSlider.value, 0f, lerp * Time.deltaTime);
    }

    public void AddPawnToProduction()
    {
        pawnsToProduct++;

        UpdateKingText();
    }

    public void AddRiderToProduction()
    {
        ridersToProduct++;

        UpdateKingText();
    }

    public void UpdateKingText()
    {
        Element element = GetComponent<Element>();

        string text = $"{element.ElementDescription}\nIn Production : pawns ({pawnsToProduct}) & riders ({ridersToProduct})";

        uiManager.UpdateText(text);
    }

    private IEnumerator StartProduction()
    {
        inProduction = true;

        while (pawnsToProduct > 0)
        {
            productionSlider.maxValue = timeOfPawnProduction;
            productionSlider.value = timeOfPawnProduction;

            yield return new WaitForSeconds(timeOfPawnProduction);

            int randomPawn = Random.Range(0, pawnUnitPrefabs.Count);
            Unit unit = Instantiate(pawnUnitPrefabs[randomPawn], unitParent).GetComponent<Unit>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            GameManager.instance.CountPawns++;

            pawnsToProduct--;
        }

        while (ridersToProduct > 0)
        {
            productionSlider.maxValue = timeOfRiderProduction;
            productionSlider.value = timeOfRiderProduction;

            yield return new WaitForSeconds(timeOfRiderProduction);

            int randomRider = Random.Range(0, riderUnitPrefabs.Count);
            Unit unit = Instantiate(riderUnitPrefabs[randomRider], unitParent).GetComponent<Unit>();

            unit.MoveToPosition(gathering.position);

            units.Add(unit);

            GameManager.instance.CountRiders++;

            ridersToProduct--;
        }

        inProduction = false;
    }

    private void CanProduce()
    {
        canStartProduction = true;
    }

    private void Producing(float specificTime)
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CanProduce();
        }

        if (canStartProduction)
        {
            canStartProduction = false;

            timeRemaining = specificTime;
        }

        if (timeRemaining > 0)
        {
            Debug.Log($"Producing... {timeRemaining}");

            inProduction = true;

            timeRemaining -= Time.deltaTime;
        }
        else
        {
            Debug.Log($"Completed");

            inProduction = false;
        }
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayer))
        {
            Vector3 hitPos = hit.point;
            hitPos.y += 0.1f;

            currentCanvasWorld = Instantiate(canvasWorld, hitPos, canvasWorld.transform.rotation);

            Destroy(currentCanvasWorld, 0.5f);

            center = hit.point;
            MoveSelectedUnit(hit.point);
        }
    }

    private void MoveSelectedUnit(Vector3 position)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].MoveToPosition(position);
        }

        HandleArmy();
    }

    #endregion

    public void HandleArmy()
    {
        int totalUnits = selectedUnits.Count;

        if (totalUnits == 0) return;

        points = Formation.EvaluatePoints(totalUnits, center);

        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] != null)
                selectedUnits[i].MoveToPosition(points[i]);
        }
    }
}