using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private CanvasScaler canvasScaler;

    private Vector2 startPosition;
    private KingManager kingManager;

    [Header("Selection")]
    [SerializeField] private List<Unit> selectedUnits;

    private void Start()
    {
        kingManager = FindObjectOfType<KingManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;

            Deselect();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Unit"))
                {
                    Unit unit = hit.transform.GetComponent<Unit>();

                    selectedUnits.Add(unit);
                    unit.Selection.SetActive(true);
                }
            }
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
                selectedUnits.Add(unit);
                unit.Selection.SetActive(true);
            }
        }
    }

    private void Deselect()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Selection.SetActive(false);
        }

        selectedUnits.Clear();
    }
}