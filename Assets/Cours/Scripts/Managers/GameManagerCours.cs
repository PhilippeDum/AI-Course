using System.Collections.Generic;
using UnityEngine;

public class GameManagerCours : MonoBehaviour
{
    public static GameManagerCours instance;

    [SerializeField] private int numPlayer = 0;
    [SerializeField] private GameObject player;

    private int maxEnergy;
    private int freeEnergy;
    private int currentEnergy;
    private int tiberium;

    private Spawner[] spawners;

    private UIManagerCours uiManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        spawners = FindObjectsOfType<Spawner>();
    }

    private void Start()
    {
        uiManager = UIManagerCours.instance;

        player.transform.position = new Vector3(spawners[numPlayer].transform.position.x, 0, spawners[numPlayer].transform.position.z);

        StartCoroutine(spawners[numPlayer].UnhideBase());
    }

    public void AddEnergyMax(int value)
    {
        maxEnergy += value;
    }

    public void AddFreeEnergy(int value)
    {
        freeEnergy += value;

        uiManager.UpdateEnergyUI(value);
    }

    public void RemoveFreeEnergy(int value)
    {
        freeEnergy -= value;
    }

    public void AddCurrentEnergy(int value)
    {
        currentEnergy += value;

        if (currentEnergy > freeEnergy)
        {
            Debug.Log($"Probleme energy");
        }
    }
}