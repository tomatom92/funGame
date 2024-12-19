using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.ReloadAttribute;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public static QuestGiver instance;
    public TextMeshProUGUI questText;
    public string postQuestText;

    private void Awake()
    {
        instance = this;
    }


    public void IsQuestFulfilled()
    {
        if (quest.questType == Quest.QuestType.Fetch)
        {
            if (InventoryController.instance.Contains(quest.questItem) != null)
            {
                Debug.Log($"Item is in inventory");
                GiveReward();
            }
            else
                Debug.Log("dont have item :(");
        }
        if (quest.questType == Quest.QuestType.kill)
        {

        }
        if (quest.questType == Quest.QuestType.puzzle)
        {

        }
        if (quest.questType == Quest.QuestType.discover)
        {

        }
    }
    public void GiveReward()
    {
        //item reward
        InventoryController.instance.Add(quest.itemReward);
        //gold reward
        for (int i = 0; i < quest.goldAmount; i++)
        {
            InventoryController.instance.Add(quest.goldReward);
        }
        //remove quest item from inventory
        InventoryController.instance.Remove(quest.questItem);

        //change text to post quest text
        transform.GetChild(0).gameObject.SetActive(true);
        questText.text = postQuestText;

    }
}
