using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : InteractableObject
{
    public GameObject player;
    private float distance;
    private Quest quest;
    public bool hasQuest;



    protected override void Start()
    {
        base.Start();
        player = PlayerController.instance.gameObject;
        if (hasQuest)
        {
            quest = GetComponent<QuestGiver>().quest;
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }
    protected override void OnInteract()
    {
        base.OnInteract();
        QuestGiver.instance.IsQuestFulfilled();
        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
        

    }
    protected override void Update()
    {
        base.Update();

        distance = Vector2.Distance(transform.position, player.transform.position);

        //Debug.Log($"npc: {instance.gameObject.name} is {instance.distance} away");
        if (distance > 2f)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}

