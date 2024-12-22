using System.Collections;
using UnityEngine;

public class BossEffects : MonoBehaviour
{
    public AudioClip roarSound;
    public AudioSource audioSource;
    public ParticleSystem splashEffect;


    public void PlayRoar()
    {
        audioSource.PlayOneShot(roarSound);
    }

    public void PlaySplashEffect()
    {
        if (splashEffect != null)
        {
            splashEffect.transform.position = transform.position; // set splash position to current position.
            splashEffect.Play();
            StartCoroutine(DisableParticleSystemAfterPlay());
        }
        else
        {
            Debug.LogWarning("No particle system set for the splash");
        }
    }
    private IEnumerator DisableParticleSystemAfterPlay()
    {
        //Wait for duration of splash, then disable
        yield return new WaitForSeconds(splashEffect.main.duration);
        splashEffect.Stop();
        splashEffect.Clear();
    }
}