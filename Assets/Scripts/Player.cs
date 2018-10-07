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
    private SpellBook spellBook;
    // Spawn point of spell animations
    [SerializeField]
    private GameObject[] exitPoints;
    private int exitIndex = 0; // Down
    // Target of the spell
    public Transform Target {
        get; set;
    }

    // Selected Attack Mode
    AttackMode attackMode;
    // GUI Attack Mode Highlighter
    [SerializeField]
    private GameObject[] attackModeFrames;

    // Line of sight blocker
    [SerializeField]
    private Block[] blocks;
    // Layer mask of blocker
    int blockLayerMask;

    // Use this for initialization
    protected override void Start () 
    {
        // Settle attack mode
        attackMode = new AttackMode(AttackMode.CHASER, attackModeFrames);

        // Start level with max stats always
        health.Initialize(maxHealth, maxHealth); 
        mana.Initialize(maxMana, maxMana);

        // Get the block layer mask
        blockLayerMask = LayerMask.GetMask("Block");

        // Get Spells
        spellBook = GetComponent<SpellBook>();

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
        // Toggle Attack mode
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.LeftShift)) {
            attackMode.ToggleAttackMode();
        }
        // Cast Attack Spell
        if (Input.GetKeyDown(KeyCode.Space)) {
            AttemptSpellCast();
        }
    }

    // Check conditions whether spellcast is possible and attack.
    public void AttemptSpellCast () 
    {
        ActivateBlocks();
        if (Target != null && !isAttacking && !IsMoving && inLineOfSight())
        {
            attackRoutine = StartCoroutine(SpellCast());
        }
    }

    // Cast a basic shot
    private IEnumerator SpellCast ()
    {
        AttackLayer attackLayer;
        float castTime, trailingAnimationTime;
        string attackName;

        switch (attackMode.selectedAttackMode) {
            case AttackMode.CHASER:
                attackLayer = AttackLayer.ShootLayer;
                castTime = 0.25f;
                trailingAnimationTime = 0.25f;
                attackName = "Chaser";
                break;
            case AttackMode.AGONISER:
                attackLayer = AttackLayer.ShootLayer;
                castTime = 0.25f;
                trailingAnimationTime = 0.25f;
                attackName = "Agoniser";
                break;
            case AttackMode.STALKER:
                attackLayer = AttackLayer.CastLayer;
                castTime = 0.333f;
                trailingAnimationTime = 0.08333f;
                attackName = "Stalker";
                break;
            case AttackMode.CASCADER:
                attackLayer = AttackLayer.CastLayer;
                castTime = 0.333f;
                trailingAnimationTime = 0.08333f;
                attackName = "Cascader";
                break;
            default:
                Debug.Log("Illegal attack mode; defaulting to Chaser.");
                attackLayer = AttackLayer.ShootLayer;
                castTime = 0.25f;
                trailingAnimationTime = 0.25f;
                attackName = "Chaser";
                break;
        }

        Debug.Log("Begin " + attackName + "Shot");

        // Trigger attacking
        SetAttackLayer(attackLayer);
        isAttacking = true;  // Trigger attacking in script
        animator.SetBool("attack", isAttacking); // Trigger attacking animation in animation controller

        // Animation cast time
        yield return new WaitForSeconds(castTime);
        InstantiateSpell();
        Debug.Log(attackName + " Shot Fired");

        // Carry on trailing animation
        yield return new WaitForSeconds(trailingAnimationTime);

        // End spell cast
        StopAttacking();

    }

    // Cast a spell
    public void InstantiateSpell () 
    {
        Spell spell = spellBook.GetSpell(attackMode.selectedAttackMode);
        SpellScript spellScript = Instantiate(spell.SpellPrefab, exitPoints[exitIndex].transform.position, Quaternion.identity).GetComponent<SpellScript>();
        spellScript.Target = Target;
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

    // Select the attackMode
    public void SetAttackMode(int selectedMode) 
    {
        attackMode.SetAttackMode(selectedMode);
    }

    class AttackMode {
        // Attack and spell indices
        // Attack modes
        public const int CHASER = 0;
        public const int AGONISER = 1;
        public const int STALKER = 2;
        public const int CASCADER = 3;
        // Spells
        public const int MESMER = 4;
        public const int PARALYSER = 5;
        public const int CAPTOR = 6;

        public int selectedAttackMode;
        private GameObject[] attackModeFrames;

        public AttackMode(int initialMode, GameObject[] frames) {
            selectedAttackMode = initialMode;
            attackModeFrames = frames;
        }

        // Increase attack mode by 1, and cycle back
        public void ToggleAttackMode() 
        {
            selectedAttackMode = (selectedAttackMode + 1) % 4;
            foreach (GameObject frame in attackModeFrames)
            {
                frame.SetActive(false);
            }
            attackModeFrames[selectedAttackMode].SetActive(true);
        }

        // Set attack mode to attackMode
        public void SetAttackMode(int attackMode)
        {
            selectedAttackMode = attackMode;
            foreach (GameObject frame in attackModeFrames)
            {
                frame.SetActive(false);
            }
            attackModeFrames[selectedAttackMode].SetActive(true);
        }

    }
}
