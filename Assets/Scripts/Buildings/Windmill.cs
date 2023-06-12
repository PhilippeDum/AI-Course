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

        GainEnergy();
    }

    private void GainEnergy()
    {
        Debug.Log($"GainEnergy : {Datas.EnergyGain}");
        gameManager.AddFreeEnergy(Datas.EnergyGain);
    }

    public override void BuildingPlacementComplete()
    {
        base.BuildingPlacementComplete();

        if (gameManager.CurrentQuestType == Quest.QuestType.BuildingPlacement)
            gameManager.CountBuildings++;

        InstantiateWorker();
    }

    private void InstantiateWorker()
    {
        if (workerPrefab == null) return;

        worker = Instantiate(workerPrefab, spawn.position, Quaternion.identity);
        worker.transform.SetParent(transform);

        GameManager.instance.CountWorkers++;

        GameManager.instance.KingPlayer.GetComponent<Production>().Units.Add(worker.GetComponent<UnitManager>());

        Debug.Log($"+1 Worker !");
    }
}