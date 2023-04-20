using UnityEngine;

public class BridgeDetection : MonoBehaviour
{
    [SerializeField] private int countPawns = 0;
    [SerializeField] private int countRiders = 0;

    private void Update()
    {
        if (GameManager.instance.CurrentQuestType == Quest.QuestType.Reparation)
        {
            Quest quest = GameManager.instance.GetCurrentQuest(Quest.QuestType.Reparation);

            if (countPawns >= quest.requiredAmountPawn && countRiders >= quest.requiredAmountRider)
            {
                Debug.Log($"Repair");

                GetComponentInParent<BridgeReparation>().Repair();

                GameManager.instance.CurrentQuestType = Quest.QuestType.Attack;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name}");

        if (other.CompareTag("Unit"))
        {
            Unit unit = other.GetComponent<Unit>();

            if (unit.GetUnitType == Unit.UnitType.Pawn) countPawns++;
            if (unit.GetUnitType == Unit.UnitType.Rider) countRiders++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.GetComponent<Unit>();

            if (unit.GetUnitType == Unit.UnitType.Pawn) countPawns--;
            if (unit.GetUnitType == Unit.UnitType.Rider) countRiders--;
        }
    }
}