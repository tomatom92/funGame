using System.Collections;
using System.Collections.Generic;
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

    public void PlaySplashEffect(Vector3 position)
    {
        splashEffect.transform.position = position;
        splashEffect.Play();
    }
}

