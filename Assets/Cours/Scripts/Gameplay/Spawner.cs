using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int defoggerRadius = 3000;
    private Fog fog;
    private bool canUnhide = false;

    private void Start()
    {
        fog = Fog.instance;
    }

    private void Update()
    {

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

    public IEnumerator UnhideBase()
    {
        while (!canUnhide)
        {
            yield return new WaitForSeconds(0.1f);
        }

        fog.UnhideUnit(transform, defoggerRadius);
    }
}
