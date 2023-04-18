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

    private void Start()
    {
        kingManager = FindObjectOfType<KingManager>();
        uiManager = FindObjectOfType<UIManager>();
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
            if (hit.transform.GetComponent<Element>())
            {
                currentElement = hit.transform.GetComponent<Element>();

                uiManager.ShowInfos(currentElement);

                if (currentElement.ElementSelection == null) return;

                if (!currentElement.ElementSelection.activeSelf)
                {
                    currentElement.ElementSelection.SetActive(true);

                    if (currentElement.CompareTag("Unit"))
                    {
                        Unit unit = currentElement.GetComponent<Unit>();
                        SelectUnit(unit);
                    }
                }
                else
                {
                    currentElement.ElementSelection.SetActive(false);

                    if (currentElement.CompareTag("Unit"))
                    {
                        Unit unit = currentElement.GetComponent<Unit>();
                        DeselectUnit(unit);
                    }
                }
            }
            else
            {
                uiManager.HandleUI(false);
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

        if (currentElement != null)
        {
            currentElement.ElementSelection.SetActive(false);
            currentElement = null;
        }

        kingManager.SelectedUnits.Clear();
    }

    #endregion
}