using System;

[Serializable]
public class Quest
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
        Attack
    }
}