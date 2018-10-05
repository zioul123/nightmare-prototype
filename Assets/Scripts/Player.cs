﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    // Stat bars of the player
    [SerializeField]
    private Stat health;
    [SerializeField]
    private Stat mana;

    // Stats of the player
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxMana;

    // Use this for initialization
    protected override void Start () 
    {
        // Start level with max stats always
        health.Initialize(maxHealth, maxHealth); 
        mana.Initialize(maxMana, maxMana);

        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        GetInput();
        base.Update();
	}

    // Listens for player input
    private void GetInput () 
    {
        // Reset direction first
        direction = Vector2.zero;

        // FOR DEBUG
        if (Input.GetKeyDown(KeyCode.Q)) {
            health.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            health.CurrentValue += 10;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            mana.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            mana.CurrentValue += 10;
        }

        // Handle Movement
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.R)) {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction += Vector2.right;
        }

        // Handle Skills
        // Basic shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attackRoutine = StartCoroutine(Shoot());
        }
    }

    // Cast a basic shot
    private IEnumerator Shoot ()
    {
        if (!isAttacking && !IsMoving)
        {
            Debug.Log("Begin shoot");

            SetAttackLayer(AttackLayer.ShootLayer);
            isAttacking = true; // For animation
            animator.SetBool("attack", isAttacking);

            yield return new WaitForSeconds(3); // FOR DEBUG
            Debug.Log("Done shoot");

            StopAttacking();
        }
    }
}
