using System.Collections.Generic;
using UnityEngine;

public class GameManagerCours : MonoBehaviour
{
    [SerializeField] private int numPlayer = 0;
    [SerializeField] private GameObject player;

    private Spawner[] spawners;

    private void Awake()
    {
        spawners = FindObjectsOfType<Spawner>();
    }

    private void Start()
    {
        player.transform.position = new Vector3(spawners[numPlayer].transform.position.x, 0, spawners[numPlayer].transform.position.z);

        StartCoroutine(spawners[numPlayer].UnhideBase());
    }
}