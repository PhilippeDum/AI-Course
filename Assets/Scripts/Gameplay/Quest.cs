using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObject/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public QuestType questType;
    public int requiredAmount;
    public int requiredAmount2;

    public enum QuestType
    {
        Production,
        Reparation,
        Attack,
        BuildingPlacementWindmill,
        ResourceCollectWood,
        BuildingPlacementMine,
        ResourceCollectIron
    }
}