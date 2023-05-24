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
        if (other.CompareTag("UnitMovement"))
        {
            UnitMovement unit = other.GetComponent<UnitMovement>();

            GameManager.instance.CurrentBridgeReparation = GetComponentInParent<BridgeReparation>();

            if (unit.GetUnitType == UnitMovement.UnitType.Pawn) countPawns++;
            if (unit.GetUnitType == UnitMovement.UnitType.Rider) countRiders++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("UnitMovement"))
        {
            UnitMovement unit = other.GetComponent<UnitMovement>();

            if (unit.GetUnitType == UnitMovement.UnitType.Pawn) countPawns--;
            if (unit.GetUnitType == UnitMovement.UnitType.Rider) countRiders--;

            if (countPawns == 0 && countRiders == 0) GameManager.instance.CurrentBridgeReparation = null;
        }
    }
}