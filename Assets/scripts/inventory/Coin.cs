using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{
    // Start is called before the first frame update
    [SerializeField]
    public ConsumableClass CoinData;
    public int amount;
    
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(CoinData, amount);
        }

    }
    
}
