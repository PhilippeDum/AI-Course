using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObject/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public QuestType questType;
    public int requiredAmountPawn;
    public int requiredAmountRider;

    public enum QuestType
    {
        Production,
        Reparation,
        Attack
    }
}