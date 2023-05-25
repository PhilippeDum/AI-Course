using UnityEngine;

public class BridgeDetection : MonoBehaviour
{
    [SerializeField] private int countPawns = 0;
    [SerializeField] private int countRiders = 0;

    private void Update()
    {
        if (GameManager.instance.CurrentBridgeReparation == GetComponentInParent<BridgeReparation>())
        {
            GameManager.instance.CountPawnsDetection = countPawns;
            GameManager.instance.CountRidersDetection = countRiders;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitManager unit = other.GetComponent<UnitManager>();

            GameManager.instance.CurrentBridgeReparation = GetComponentInParent<BridgeReparation>();

            if (unit.UnitData.TypeUnit == Unit.UnitType.Pawn) countPawns++;
            if (unit.UnitData.TypeUnit == Unit.UnitType.Rider) countRiders++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitManager unit = other.GetComponent<UnitManager>();

            if (unit.UnitData.TypeUnit == Unit.UnitType.Pawn) countPawns--;
            if (unit.UnitData.TypeUnit == Unit.UnitType.Rider) countRiders--;

            if (countPawns == 0 && countRiders == 0) GameManager.instance.CurrentBridgeReparation = null;
        }
    }
}