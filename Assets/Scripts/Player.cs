using System.Collections;
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

    // Spells
    [SerializeField]
    private GameObject[] spellPrefabs;
    // Spawn point of spell animations
    [SerializeField]
    private GameObject[] exitPoints;
    private int exitIndex = 0; // Down

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

        // Handle Movement and Exit Points
        if (Input.GetKey(KeyCode.W)) {
            direction += Vector2.up;
            exitIndex = 1;
        }
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector2.left;
            exitIndex = 3;
        }
        if (Input.GetKey(KeyCode.R)) {
            direction += Vector2.down;
            exitIndex = 0;
        }
        if (Input.GetKey(KeyCode.S)) {
            direction += Vector2.right;
            exitIndex = 2;
        }

        // Handle Skills
        // Basic shoot
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (!isAttacking && !IsMoving) {
                attackRoutine = StartCoroutine(Attack());
            }
        }
    }

    // Cast a basic shot
    private IEnumerator Attack ()
    {
        Debug.Log("Begin shoot");

        SetAttackLayer(AttackLayer.ShootLayer);
        isAttacking = true;  // Trigger attacking in script
        animator.SetBool("attack", isAttacking); // Trigger attacking animation in animation controller

        yield return new WaitForSeconds(0.25f); // FOR DEBUG
        CastSpell();
        Debug.Log("Shot fired");
        yield return new WaitForSeconds(0.25f); // FOR DEBUG
        StopAttacking();
    }

    // Cast a spell
    public void CastSpell () 
    {
        Instantiate(spellPrefabs[0], exitPoints[exitIndex].transform.position, Quaternion.identity); 
    }
}
