using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemAnim : MonoBehaviour
{
    public TextMeshProUGUI itemText;
    //SpriteRenderer spriteRenderer;

    void Update()
    {
        itemText.text = name;
    }



}
