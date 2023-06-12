using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : Building
{
    [Header("References")]
    [SerializeField] private Slider miningSlider;
    [SerializeField] private UnitManager currentMiner;

    [Header("Mining Datas")]
    [SerializeField] private List<Resources> resourcesType;
    [SerializeField] private float randomRate = 30f;
    [SerializeField] private float timeOfMining = 10f;

    private float timeRemaining = 0f;

    private bool isMining = false;

    #region Getters / Setters

    public UnitManager CurrentMiner 
    { 
        get { return currentMiner; } 
        set { currentMiner = value; }
    }

    public List<Resources> ResourcesType
    {
        get { return resourcesType; }
        set { resourcesType = value; }
    }

    public bool IsMining
    {
        get { return isMining; }
        set { isMining = value; }
    }

    #endregion

    public override void OnEnable()
    {
        base.OnEnable();
    }

    #region Mining

    private void Update()
    {
        miningSlider.transform.parent.LookAt(Camera.main.transform.position);

        if (currentMiner != null && !isMining)
        {
            timeRemaining = timeOfMining;

            miningSlider.maxValue = timeOfMining;
            miningSlider.value = timeOfMining;

            isMining = true;
        }

        if (isMining)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                miningSlider.value = timeRemaining;
            }
            else
            {
                Resources resource = GetRandomResource();

                gameManager.AddResouce(resource, Datas.ResourceGain);

                isMining = false;
            }
        }
    }

    private Resources GetRandomResource()
    {
        int random = Random.Range(0, 100);

        if (random <= randomRate) return resourcesType[1];
        else return resourcesType[0];
    }

    #endregion
}