using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool completed;

    public string title;
    public string description;
    public ItemClass questItem;
    public ItemClass goldReward;
    public int goldAmount;
    public ItemClass itemReward;
    public QuestType questType;
    public enum QuestType
    {
        Fetch,
        kill,
        discover,
        puzzle,

    }
}
