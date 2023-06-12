using System.Collections;
using UnityEngine;

public class Defogger : MonoBehaviour
{
    [SerializeField] private int defoggerRadius = 3000;
    [SerializeField] private bool isUnit = false;

    private Fog fog;

    private bool canUnhide = false;
    private float lastTime = 0f;

    public bool CanUnide
    {
        get { return canUnhide; }
        set { canUnhide = value; }
    }

    public bool IsUnit
    {
        get { return isUnit; }
        set { isUnit = value; }
    }

    private void Start()
    {
        fog = Fog.instance;

        lastTime = Time.time;
    }

    private void OnEnable()
    {
        Fog.OnCompleteInitialize += CanUnhide;
    }

    private void OnDisable()
    {
        Fog.OnCompleteInitialize -= CanUnhide;
    }

    private void Update()
    {
        //UnhideInContinue();
    }

    private void UnhideInContinue()
    {
        if (!isUnit) return;

        if (Time.time > lastTime + 5f)
        {
            fog.UnhideUnit(transform, defoggerRadius);

            lastTime = Time.time;
        }
    }

    private void CanUnhide()
    {
        canUnhide = true;
    }

    public void Unhide()
    {
        StartCoroutine(Unhiding());
    }

    public IEnumerator Unhiding()
    {
        Debug.Log($"Unhiding");

        while (!canUnhide)
        {
            yield return new WaitForSeconds(0.1f);
        }

        fog.UnhideUnit(transform, defoggerRadius);

        canUnhide = false;
    }
}