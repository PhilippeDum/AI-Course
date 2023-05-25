using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private CanvasScaler canvasScaler;

    private Vector2 startPosition;
    private GameManager gameManager;
    private UnitManager currentUnit;
    private UIManager uiManager;
    private Production productionPlayer;

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

        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();

        productionPlayer = gameManager.KingPlayer.GetComponent<Production>();
    }

    private void Update()
    {
        if (GameManager.instance.GameFinished) return;

        HandleDetectionAndSelection();
    }

    #region Selection & Detection

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
                if (currentUnit != null && currentUnit && !selectAllSameUnit)
                {
                    if (currentUnit.UnitData.TeamUnit == Unit.UnitTeam.Enemy) yield break;

                    selectAllSameUnit = true;
                    SelectAllSameUnit(currentUnit.GetComponent<UnitManager>());
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
            if (hit.transform.GetComponent<UnitManager>())
            {
                currentUnit = hit.transform.GetComponent<UnitManager>();

                uiManager.ShowInfos(currentUnit);

                if (currentUnit.Selection == null) return;

                if (!currentUnit.Selection.activeSelf)
                {
                    currentUnit.Selection.SetActive(true);

                    SelectUnit(currentUnit);
                }
                else
                {
                    currentUnit.Selection.SetActive(false);

                    DeselectUnit(currentUnit);
                }
            }
            else
            {
                uiManager.HandleUI(false);

                DeselectAll();
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

        foreach (UnitManager unit in productionPlayer.Units)
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

    private void SelectUnit(UnitManager unit)
    {
        if (unit == null) return;

        productionPlayer.SelectedUnits.Add(unit);
        unit.Selection.SetActive(true);
    }

    private void SelectAllSameUnit(UnitManager unit)
    {
        for (int i = 0; i < productionPlayer.Units.Count; i++)
        {
            UnitManager possessedUnit = productionPlayer.Units[i];

            if (possessedUnit.UnitData.TypeUnit == unit.UnitData.TypeUnit)
            {
                SelectUnit(possessedUnit);
            }
        }

        selectAllSameUnit = false;
    }

    private void DeselectUnit(UnitManager unit)
    {
        productionPlayer.SelectedUnits.Remove(unit);
        unit.Selection.SetActive(false);
    }

    private void DeselectAll()
    {
        foreach (UnitManager unit in productionPlayer.SelectedUnits)
        {
            if (unit == null) return;

            unit.Selection.SetActive(false);
        }

        if (currentUnit != null)
        {
            currentUnit.Selection.SetActive(false);
            currentUnit = null;
        }

        productionPlayer.SelectedUnits.Clear();
    }

    #endregion

    #endregion
}