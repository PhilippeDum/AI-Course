using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private CanvasScaler canvasScaler;

    private Vector2 startPosition;
    private KingManager kingManager;

    [Header("UI")]
    [SerializeField] private Text infosTitle;
    [SerializeField] private Text infosDescription;
    [SerializeField] private Text optionsTitle;
    [SerializeField] private Text optionsDescription;
    [SerializeField] private Button optionsButton;

    private void Start()
    {
        kingManager = FindObjectOfType<KingManager>();

        SetupUI(false, false, false, false, false);
    }

    private void Update()
    {
        HandleSelection();
    }

    #region Selection

    private void HandleSelection()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            DetectUnit();
        }

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            DeselectAll();

            DetectUnit();
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

    private void DetectUnit()
    {
        if (EventSystem.current.IsPointerOverGameObject(-1)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Unit") || hit.transform.CompareTag("King"))
                ShowInfos(hit.transform);
            else
                SetupUI(false, false, false, false, false);

            if (hit.transform.CompareTag("Unit"))
            {
                Unit unit = hit.transform.GetComponent<Unit>();

                if (!unit.Selection.activeSelf)
                {
                    SelectUnit(unit);
                }
                else
                {
                    DeselectUnit(unit);
                }
            }
        }
    }

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
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                SelectUnit(unit);
            }
        }
    }

    private void SelectUnit(Unit unit)
    {
        kingManager.SelectedUnits.Add(unit);
        unit.Selection.SetActive(true);
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
            unit.Selection.SetActive(false);
        }

        kingManager.SelectedUnits.Clear();
    }

    #endregion

    #region UI

    private void SetupUI(bool state1, bool state2, bool state3, bool state4, bool state5)
    {
        infosTitle.gameObject.SetActive(state1);
        infosDescription.gameObject.SetActive(state2);
        optionsTitle.gameObject.SetActive(state3);
        optionsDescription.gameObject.SetActive(state4);
        optionsButton.gameObject.SetActive(state5);
    }

    private void ShowInfos(Transform hitTransform)
    {
        if (hitTransform.GetComponent<Element>())
        {
            Element element = hitTransform.GetComponent<Element>();

            infosTitle.text = $"Infos '{element.ElementName}'";
            infosDescription.text = element.ElementDescription;

            if (element.CompareTag("King"))
            {
                infosDescription.text = $"{element.ElementDescription}";

                optionsButton.name = element.ElementButtonName;
                optionsButton.GetComponentInChildren<Text>().text = element.ElementButtonName;
                optionsButton.onClick.AddListener(delegate { kingManager.AddUnitToProduction(); });

                SetupUI(true, true, true, false, true);
            }

            if (element.CompareTag("Unit"))
            {

            }
        }
    }

    public void UpdateText(string text)
    {
        infosDescription.text = $"Main Base : creating {text} unit(s)";
    }

    #endregion
}