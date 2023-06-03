using UnityEngine;

public class Windmill : Building
{
    [SerializeField] private GameObject workerPrefab;
    [SerializeField] private GameObject worker;
    [SerializeField] private Transform spawn;

    public override void OnEnable()
    {
        base.OnEnable();

        GainEnergy();
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

        Debug.Log($"+1 Worker !");
    }

    private void GainEnergy()
    {
        Debug.Log($"GainEnergy : {Datas.EnergyGain}");
        gameManager.AddFreeEnergy(Datas.EnergyGain);
    }
}