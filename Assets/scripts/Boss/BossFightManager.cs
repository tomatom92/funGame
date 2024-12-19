using System.Collections;
using UnityEngine;
using UnityEngine.Playables;  // Required for Timeline

public class BossFightManager : MonoBehaviour
{
    public CrocBoss crocodile;  // Reference to the boss
    public GameObject player;  // Reference to the player
    public BossUI bossUI;  // Reference to the boss health UI
    public PlayableDirector timelineDirector;  // Timeline component for the cutscene

    public bool isFightActive = false;

    public void StartBossFight()
    {
        isFightActive = true;
        crocodile.gameObject.SetActive(true);
        bossUI.ShowHealthBar();
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
            
            Debug.Log($"boss State: {crocodile.GetState()}");
        }
        else
        {
            // If no Timeline is assigned, start the fight immediately
            crocodile.SetState(CrocBoss.BossState.Idle);
            StartBossFight();
            crocodile.SetState(CrocBoss.BossState.Chase);
            Debug.Log($"boss State: {crocodile.GetState()}");
        }
        //make boss chase player when fight starts after cutscene
        

    }

    private IEnumerator WaitForCutsceneToEnd()
    {
        if (timelineDirector != null)
        {
            float timeElapsed = 0f;
            while (timeElapsed < (float)timelineDirector.duration)
            {
                if (Input.GetKeyDown(KeyCode.Space))  // Replace with your preferred skip key
                {
                    timelineDirector.time = timelineDirector.duration;  // Skip to the end
                    break;
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        StartBossFight();
    }

}
