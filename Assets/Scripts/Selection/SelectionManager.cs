using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform selectionBox;

    private Vector2 startPosition;
    private bool inSelection;

    [Header("Units")]
    [SerializeField] private List<Unit> units;

    #region Getters / Setters

    public bool InSelection
    {
        get { return inSelection; }
        set { inSelection = value; }
    }

    #endregion

    void Start()
    {
        startPosition = Input.mousePosition;
        inSelection = false;
    }

    void Update()
    {

    }
}