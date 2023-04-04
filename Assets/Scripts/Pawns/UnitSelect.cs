using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    [SerializeField] private GameObject selection;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selection.SetActive(true);
        }
    }
}