﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spell that "falls from the sky"
public class GlobalSpell : Spell 
{
    // Initial offset of the spell
    [SerializeField]
    private float initialY;

    // Use this for initialization
    protected override void Start () 
    {
        base.Start();
        // Initial position
        transform.position = Target.position + new Vector3(0, initialY, 0);
        // Fix direction and rotation
        FixedUpdate();
    }

    // Handle movement or instant following
    private void FixedUpdate ()
    {
        if (Target != null) {
            // Move to the location of the target
            Vector2 direction = Target.transform.position - transform.position;
            rigidBody.velocity = direction.normalized * speed;
        }
    }
}
