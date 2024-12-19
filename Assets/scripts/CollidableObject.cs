using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    protected Collider2D z_collider;
    [SerializeField]
    private ContactFilter2D z_filter;
    private readonly List<Collider2D> z_CollidedObjects = new(1);

    protected virtual void Start()
    {
        z_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
        if(z_CollidedObjects != null)
        {
            z_collider.OverlapCollider(z_filter, z_CollidedObjects);
            foreach (var o in z_CollidedObjects)
            {
                if (o.gameObject != null)
                    OnCollided(o.gameObject);
            }
        }
        
    }
    protected virtual void OnCollided(GameObject collidedObject)
    {
        
        Debug.Log("collided with " +  collidedObject.name);
    }
    
}
