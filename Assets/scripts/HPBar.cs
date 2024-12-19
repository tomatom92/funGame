using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{

    public int hpRemaining;
    public GameObject deathGUI;
    public GameObject healthHolder;
    private GameObject[] heartArray;

    private SpriteRenderer spriteRenderer;
    private CharacterBehaviour character;
    private Animator animator;
    private Collider2D charCollider;
    private float hurtTimer;

    //sfx
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Awake()
    {
        charCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterBehaviour>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {

        hurtTimer = character.stunTime;
        
        healthHolder.SetActive(true);

        //filling heart array and setting health based on amount of child heart objects
        heartArray = new GameObject[healthHolder.transform.childCount];
        hpRemaining = healthHolder.transform.childCount;
        for (int i = 0; i < healthHolder.transform.childCount; i++)
        {
            heartArray[i] = healthHolder.transform.GetChild(i).gameObject;

        }
        RefreshHP();
    }
    public void RemoveHeart(int heartAmount)
    {
        hpRemaining -= heartAmount;
        RefreshHP();
        //hit noise;
        audioSource.Play();
        if (hpRemaining <= 0)
        {
            Die();
        }
        //Debug.Log($"{gameObject.name}, removed {heartAmount} hearts");

    }

    public void HurtAnimation(float timer)
    {
        //Debug.Log((int)(100 * timer / 16));
        if ((int)(100 * timer / 16) % 2 == 0)
        {
            //Debug.Log($"im red");
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
    // Update is called once per frame
    public void RefreshHP()
    {
        for (int i = 0; i < healthHolder.transform.childCount; i++) { heartArray[i].SetActive(false); }
        for (int i = 0; i < hpRemaining; i++) { heartArray[i].SetActive(true); }
    }
    public void Die()
    {
        audioSource.clip = deathSound;
        audioSource.volume = 0.15f;
        audioSource.Play();
        spriteRenderer.color = Color.red;
        charCollider.enabled = false;
        StartCoroutine(Wait(hurtTimer));
        spriteRenderer.enabled = false;
        animator.enabled = false;
        if (gameObject.CompareTag("Player"))
        {
            PlayerController.instance.enabled = false;
            deathGUI.SetActive(true);

        }
        
    }

    private void Update()
    {

        if (character.isStunned)
        {
            HurtAnimation(hurtTimer);
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0.0f)
            {
                //timer finished, resetting timer and resetting character color and stunned value
                hurtTimer = 0.7f;
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                character.isStunned = false;
            }

        }

    }
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
