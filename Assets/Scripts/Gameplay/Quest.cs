using System;

[Serializable]
public class Quest
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