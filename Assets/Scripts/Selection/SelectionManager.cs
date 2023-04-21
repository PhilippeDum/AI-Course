using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private CanvasScaler canvasScaler;

    private Vector2 startPosition;
    private Element currentElement;
    private KingManager kingManager;
    private UIManager uiManager;

    // Double click
    [Header("Double Click")]
    [SerializeField] private float timeBetweenLeftClick = 0.3f;

    private float firstLeftClickTime;
    private bool isTimeCheckAllowed = true;
    private int LeftClickNum = 0;
    private bool selectAllSameUnit;

    private void Start()
    {
        selectAllSameUnit = false;

        kingManager = FindObjectOfType<KingManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        HandleDetectionAndSelection();
    }

    private void HandleDetectionAndSelection()
    {
        DoubleClick();

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            DetectElement();
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            DeselectAll();

            DetectElement();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    private void DoubleClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            LeftClickNum++;
        }

        if (LeftClickNum == 1 && isTimeCheckAllowed)
        {
            firstLeftClickTime = Time.time;
            StartCoroutine(DetectDoubleLeftClick());
        }
    }

    private IEnumerator DetectDoubleLeftClick()
    {
        isTimeCheckAllowed = false;

        while (Time.time < firstLeftClickTime + timeBetweenLeftClick)
        {
            if (LeftClickNum == 2)
            {
                if (currentElement != null && currentElement.GetComponent<Unit>() && !selectAllSameUnit)
                {
                    Stats stats = currentElement.GetComponent<Stats>();

                    if (stats.GetTeam == Team.Enemy) yield break;

                    selectAllSameUnit = true;
                    SelectAllSameUnit(currentElement.GetComponent<Unit>());
                }

                break;
            }

            yield return new WaitForEndOfFrame();
        }

        LeftClickNum = 0;
        isTimeCheckAllowed = true;
    }

    private void DetectElement()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.GetComponent<Element>())
            {
                currentElement = hit.transform.GetComponent<Element>();

                uiManager.ShowInfos(currentElement);

                if (currentElement.ElementSelection == null) return;

                if (!currentElement.ElementSelection.activeSelf)
                {
                    currentElement.ElementSelection.SetActive(true);

                    if (currentElement.CompareTag("Unit")) SelectUnit(currentElement.GetComponent<Unit>());
                }
                else
                {
                    currentElement.ElementSelection.SetActive(false);

                    if (currentElement.CompareTag("Unit")) DeselectUnit(currentElement.GetComponent<Unit>());
                }
            }
            else
            {
                uiManager.HandleUI(false);
            }
        }
    }

    #region Box

    private void UpdateSelectionBox(Vector2 cursorMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = cursorMousePos.x - startPosition.x;
        float height = cursorMousePos.y - startPosition.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPosition + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (Unit unit in kingManager.Units)
        {
            if (unit == null) return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectUnit(unit);
            }
        }
    }

    #endregion

    #region Unit Selection

    private void SelectUnit(Unit unit)
    {
        if (unit == null) return;

        kingManager.SelectedUnits.Add(unit);
        unit.Selection.SetActive(true);
    }

    private void SelectAllSameUnit(Unit unit)
    {
        for (int i = 0; i < kingManager.Units.Count; i++)
        {
            Unit possessedUnit = kingManager.Units[i];

            if (possessedUnit.GetUnitType == unit.GetUnitType)
            {
                SelectUnit(possessedUnit);
            }
        }

        selectAllSameUnit = false;
    }

    private void DeselectUnit(Unit unit)
    {
        kingManager.SelectedUnits.Remove(unit);
        unit.Selection.SetActive(false);
    }

    private void DeselectAll()
    {
        foreach (Unit unit in kingManager.SelectedUnits)
        {
            if (unit == null) return;

            unit.Selection.SetActive(false);
        }

        if (currentElement != null)
        {
            currentElement.ElementSelection.SetActive(false);
            currentElement = null;
        }

        kingManager.SelectedUnits.Clear();
    }

    #endregion
}