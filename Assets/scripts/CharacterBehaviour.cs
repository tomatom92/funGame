using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    HPBar healthBar;
    //public bool isStunned = false;
    //public float stunTime = 0.7f;
    protected SpriteRenderer spriteRenderer;
    public bool isDead = false;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Die() 
    { 
        
    }
    public virtual void TakeDamage()
    {
        healthBar.RemoveHeart(1);
        Debug.Log($"{gameObject.name} hp - 1");
    }
}

