using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spell that exits from the player and flies at an enemy
public class ProjectileSpellScript : SpellScript 
{
    // Initial rotation of the spell for offsetting the animation
    [SerializeField]
    private float initialRotation;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        // Fix direction and rotation
        FixedUpdate();
    }
	
    // Handle movement and rotation
    private void FixedUpdate ()
    {
        if (Target != null) {
            // Handle movement
            Vector2 direction = Target.transform.position - transform.position;
            rigidBody.velocity = direction.normalized * Speed;
            // Handle rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - InitialRotation;
            rigidBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else {
            // Explode it
            GetComponent<Animator>().SetTrigger("impact");
        }
    }


    public float InitialRotation
    {
        get
        {
            return initialRotation;
        }

        set
        {
            initialRotation = value;
        }
    }
}
