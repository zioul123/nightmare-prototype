using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spell that exits from the player and flies at an enemy
public class ProjectileSpell : Spell 
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
            rigidBody.velocity = direction.normalized * speed;
            // Handle rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - initialRotation;
            rigidBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
