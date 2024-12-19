using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGraphics : MonoBehaviour
{
    public Animator animator;
    float x,y;
    public float weaponDisplacementX = 1f;
    public float weaponDisplacementY = 1f;

    private InventoryController inventory;
    [SerializeField] private ToolClass weapon;
    SpriteRenderer sr;

    Vector2 movementDirection;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        inventory = InventoryController.instance;
        x = animator.GetFloat("Horizontal");
        y = animator.GetFloat("Vertical");
        movementDirection = new(x, y);

        //setting sword to weapon tool class
        sr.sprite = weapon.icon;

        
        //equipped item
        inventory.EquipItem(weapon);
        sr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveWeapon();
    }
    private void MoveWeapon()
    {

        float x = animator.GetFloat("Horizontal");
        float y = animator.GetFloat("Vertical");

        movementDirection = new(x, y);

        if (movementDirection != Vector2.zero)
        {
            transform.localPosition = movementDirection * new Vector2(weaponDisplacementX, weaponDisplacementY);
        }
    }
}
