using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;  // Required for Timeline

public class BossFightManager : MonoBehaviour
{
    public CrocBoss crocodile;  // Reference to the boss
    public GameObject player;  // Reference to the player
    public BossUI bossUI;  // Reference to the boss health UI
    public PlayableDirector timelineDirector;  // Timeline component for the cutscene
    private BossEffects bossEffects;

    private void Start()
    {
        bossEffects = GetComponent<BossEffects>();
    }


    public bool isFightActive = false;

    public void StartBossFight()
    {
        isFightActive = true;
        crocodile.gameObject.SetActive(true);
        bossUI.ShowHealthBar();
        bossEffects.PlayRoar();
    }

    public void EndBossFight()
    {
        isFightActive = false;
        crocodile.gameObject.SetActive(false);
        bossUI.HideHealthBar();
    }

    public void TriggerCutsceneAndStartFight()
    {
        // Play the cutscene

        if (timelineDirector != null)
        {
            crocodile.SetState(CrocBoss.BossState.Idle);
            timelineDirector.Play();
            StartCoroutine(WaitForCutsceneToEnd());

            //Debug.Log($"boss State: {crocodile.GetState()}");
        }
        else
        {
            // If no Timeline is assigned, start the fight immediately
            crocodile.SetState(CrocBoss.BossState.Idle);
            StartBossFight();
            //Debug.Log($"boss State: {crocodile.GetState()}");
        }
        //make boss chase player when fight starts after cutscene


    }

    private IEnumerator WaitForCutsceneToEnd()
    {
        if (timelineDirector != null)
        {
            while (timelineDirector.state == PlayState.Playing)
            {
                if (Input.GetKeyDown(KeyCode.Space))  // Replace with your preferred skip key
                {
                    timelineDirector.time = timelineDirector.duration;  // Skip to the end
                    break;
                }
                yield return null;
            }
        }

        StartBossFight();
        crocodile.SetState(CrocBoss.BossState.Chase);
    }
}