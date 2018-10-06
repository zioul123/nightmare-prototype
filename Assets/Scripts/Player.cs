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
    // Target of the spell
    public Transform Target {
        get; set;
    }

    // Line of sight blocker
    [SerializeField]
    private Block[] blocks;
    // Layer mask of blocker
    int blockLayerMask;

    // Use this for initialization
    protected override void Start () 
    {
        // Start level with max stats always
        health.Initialize(maxHealth, maxHealth); 
        mana.Initialize(maxMana, maxMana);

        // Get the block layer mask
        blockLayerMask = LayerMask.GetMask("Block");

        // FOR DEBUG
        //Target = GameObject.Find("Target").transform;

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
        if (Input.GetKeyDown(KeyCode.Space)) {
            ActivateBlocks();
            if (Target != null && !isAttacking && !IsMoving && inLineOfSight()) {
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

    // Check if enemy is in line of sight
    private bool inLineOfSight ()
    {
        Vector3 targetDirection = (Target.position - transform.position).normalized;
        // 8 is blocks layermask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.position), blockLayerMask);
        return hit.collider == null;
    }

    // Activate only the correct set of sight blockers
    private void ActivateBlocks ()
    {
        foreach (Block block in blocks)
        {
            block.Deactivate();
        }
        blocks[exitIndex].Acctivate();
    }

}
