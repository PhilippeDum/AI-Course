using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mine : Building
{
    [Header("References")]
    [SerializeField] private Slider miningSlider;
    [SerializeField] private UnitManager currentMiner;

    [Header("Timers")]
    [SerializeField] private float timer = 10f;

    bool canStartMining = false;

    #region Getters / Setters

    public UnitManager CurrentMiner 
    { 
        get { return currentMiner; } 
        set { currentMiner = value; }
    }

    #endregion

    public override void OnEnable()
    {
        base.OnEnable();
    }

    private IEnumerator Mining()
    {
        yield return new WaitForSeconds(10f);
    }

    private void Update()
    {
        miningSlider.transform.parent.LookAt(Camera.main.transform.position);

        if (currentMiner != null && !canStartMining)
        {
            canStartMining = true;

            StartCoroutine(Mining());

            Debug.Log($"{currentMiner} start mining in {name}");
        }
    }


}