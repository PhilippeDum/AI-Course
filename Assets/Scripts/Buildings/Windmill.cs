using UnityEngine;

public class Windmill : Building
{
    [Header("References")]
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private GameObject worker;
    [SerializeField] private Transform spawn;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void BuildingPlacementComplete()
    {
        base.BuildingPlacementComplete();

        if (gameManager.CurrentQuestType == Quest.QuestType.BuildingPlacementWindmill)
            gameManager.CountWindmills++;

        InstantiateWorker();
    }

    private void InstantiateWorker()
    {
        if (workerPrefab == null) return;

        worker = Instantiate(workerPrefab, spawn.position, Quaternion.identity);
        worker.transform.SetParent(transform);

        GameManager.instance.CountWorkers++;

        GameManager.instance.KingPlayer.GetComponent<Production>().Units.Add(worker.GetComponent<UnitManager>());
    }
}