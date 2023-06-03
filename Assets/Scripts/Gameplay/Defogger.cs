using System.Collections;
using UnityEngine;

public class Defogger : MonoBehaviour
{
    [SerializeField] private int defoggerRadius = 3000;
    private Fog fog;
    private bool canUnhide = false;

    private void Start()
    {
        fog = Fog.instance;
    }

    private void OnEnable()
    {
        Fog.OnCompleteInitialize += CanUnhide;
    }

    private void OnDisable()
    {
        Fog.OnCompleteInitialize -= CanUnhide;
    }

    private void CanUnhide()
    {
        canUnhide = true;
    }

    public IEnumerator Unhide()
    {
        while (!canUnhide)
        {
            yield return new WaitForSeconds(0.1f);
        }

        fog.UnhideUnit(transform, defoggerRadius);
    }
}