using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character 
{
    // Stat bars of the player
    [SerializeField]
    private Stat mana;

    // Stats of the player
    [SerializeField]
    private float maxMana;
    [SerializeField]
    private float meleeDamage;

    // Spells
    private SpellBook spellBook;
    // Spawn point of spell animations
    [SerializeField]
    private GameObject[] exitPoints;
    private int exitIndex = 0; // Down by default
    // Target of the spell
    public Transform Target { get; set; }
    // Current selected spell
    Spell currentSpell;

    // Selected Attack Mode (Chaser, Agoniser, Stalker, Cascader). Melee is separate mechanic.
    AttackMode attackMode;
    // GUI Attack Mode Highlighter
    [SerializeField]
    private GameObject[] attackModeFrames;

    // Line of sight blocker, for AOE facing attacks (Not yet implemented)
    [SerializeField]
    private Block[] blocks;
    // Layer mask of blocker
    int blockLayerMask;

    // Melee AOEs
    [SerializeField]
    private GameObject[] meleeAoes;
    // Active Melee AOE
    GameObject meleeAoe;

    // Layer mask of clickables
    int clickableLayerMask;

    // Movement limits. They are set by Camera.
    private Vector3 min, max;

    // Use this for initialization
    protected override void Start () 
    {
        // Settle attack mode
        attackMode = new AttackMode(AttackMode.CHASER, attackModeFrames);

        // Start level with max stats always
        mana.Initialize(maxMana, maxMana);

        // Get the block layer mask
        blockLayerMask = LayerMask.GetMask("Block");

        // Get the clickable layer mask
        clickableLayerMask = LayerMask.GetMask("Clickable");

        // Get Spells
        spellBook = GetComponent<SpellBook>();

        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () 
    {
        GetInput();
        base.Update();

        // Prevent movement outside screen bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x),
                                         Mathf.Clamp(transform.position.y, min.y, max.y),
                                         transform.position.z);

        // Change the melee input box if necessary
        if (isMeleeing) {
            ActivateMeleeAoe(exitIndex);
        }
	}

    // Set the movement limits of the player so she cannot leave the map
    public void SetLimits(Vector3 min, Vector3 max) 
    {
        this.min = min;
        this.max = max;
    }

    // Listens for player input
    private void GetInput () 
    {
        // Reset direction first
        direction = Vector2.zero;

        // FOR DEBUG
        if (Input.GetKeyDown(KeyCode.P)) {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            Health.CurrentValue += 10;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            mana.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            mana.CurrentValue += 10;
        }

        // Targetting closest enemy
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TargetClosestEnemy();
        }

        // Handle Movement and Exit Points
        if (Input.GetKey(KeyCode.A)) {
            direction += Vector2.left;
            exitIndex = 3;
        }
        if (Input.GetKey(KeyCode.R)) {
            direction += Vector2.down;
            exitIndex = 0;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            exitIndex = 1;
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
            AttemptSpellCast(false);
        }
        // Melee
        if (Input.GetKeyDown(KeyCode.F)) {
            AttemptMelee();
        }
    }

    // Targets the closest enemy
    public void TargetClosestEnemy() 
    {
        Collider2D closestEnemy = null;
        // Check 20 radii for an enemy in growing order
        for (float r = 0.1f; r < 10 && closestEnemy == null; r += 0.5f) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, r, clickableLayerMask);
            foreach (Collider2D collider in colliders) {
                // Select if the target is an enemy and not dead
                if (collider.gameObject.tag == "Enemy" && collider.GetComponent<Character>().Health.CurrentValue != 0) {
                    closestEnemy = collider;
                    break;
                }
            }
        }

        // Only target if there exists an enemy
        if (closestEnemy != null) {
            GameManager gm = GameManager.Instance;
            gm.SelectTarget(closestEnemy);
        }
    }

    // Rotate the player toward the target she is casting at
    public void RotatePlayerTowardTarget ()
    {
        if (Target == null) {
            return;
        }

        Vector3 dir = Target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Right
        if (-45 < angle && angle <= 45) {
            animator.SetFloat("x", 1);
            animator.SetFloat("y", 0);
            exitIndex = 2;
        // Down
        } else if (-135 < angle && angle <= -45) {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", -1);
            exitIndex = 0;
        // Left
        } else if (angle <= -135 || 135 < angle) {
            animator.SetFloat("x", -1);
            animator.SetFloat("y", 0);
            exitIndex = 3;
        // Up
        } else if (45 < angle && angle <= 135) {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", 1);
            exitIndex = 1;
        }
    }

    // Check conditions whether melee is possible and attack
    public void AttemptMelee() 
    {
        if (!isAttacking)
        {
            attackRoutine = StartCoroutine(MeleeAttack());
        }
    }

    // Use melee. Precondition: All conditions to melee were already checked.
    private IEnumerator MeleeAttack()
    {
        Debug.Log("Begin melee attack");

        // Trigger attacking
        SetAttackLayer(AttackLayer.MeleeLayer);
        isAttacking = true;
        animator.SetBool("attack", isAttacking);
        isMeleeing = true;

        // Show the AOE box
        meleeAoe = meleeAoes[exitIndex];
        meleeAoe.SetActive(true);

        // Animation attack time
        yield return new WaitForSeconds(0.3333f);
        EffectMelee();
        Debug.Log("Melee executed");
        isMeleeing = false; // Moving will now cancel attack

        // Trailing animation time
        yield return new WaitForSeconds(0.1667f);
        StopAttacking();
    }

    // Cast AOE damage to enemies in melee area
    private void EffectMelee() 
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(meleeAoe.transform.position, meleeAoe.transform.lossyScale, 0, clickableLayerMask);
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Enemy" && collider.GetComponent<Character>().Health.CurrentValue != 0) {
                collider.GetComponent<Enemy>().TakeDamage(meleeDamage);
            }
        }

        // Hide AOE box
        meleeAoe.SetActive(false);
    }

    // Activate the specified AOE and deactivate the others
    private void ActivateMeleeAoe(int exitPoint) 
    {
        for (int i = 0; i < 4; i++) {
            meleeAoes[i].SetActive(false);
        }
        meleeAoe = meleeAoes[exitIndex];
        meleeAoe.SetActive(true);
    }

    // Check conditions whether spellcast is possible and attack.
    public void AttemptSpellCast (bool isDirectional) 
    {
        if (isDirectional) {
            ActivateBlocks();
        }
        if (Target != null && !isAttacking && !IsMoving && (!RequiresLineOfSight() || InLineOfSight()))
        {
            RotatePlayerTowardTarget();
            attackRoutine = StartCoroutine(SpellCast());
        }
    }

    // Cast a basic shot. Precondition: All conditions to cast a spell were already met.
    private IEnumerator SpellCast ()
    {
        currentSpell = spellBook.GetSpell(attackMode.selectedAttackMode);

        Debug.Log("Begin " + currentSpell.Name + "Shot");

        // Trigger attacking
        SetAttackLayer(currentSpell.AttackLayer);
        isAttacking = true;  // Trigger attacking in script
        animator.SetBool("attack", isAttacking); // Trigger attacking animation in animation controller

        // Animation cast time
        yield return new WaitForSeconds(currentSpell.CastTime);
        // Final check before shooting
        if (Target != null && (!RequiresLineOfSight() || InLineOfSight())) {
            RotatePlayerTowardTarget();
            InstantiateSpell();
        }

        Debug.Log(currentSpell.Name + " Shot Fired");

        // Carry on trailing animation
        yield return new WaitForSeconds(currentSpell.TrailingAnimationTime);

        // End spell cast
        StopAttacking();
    }

    // Cast a spell
    public void InstantiateSpell () 
    {
        SpellScript spellScript = Instantiate(currentSpell.SpellPrefab, exitPoints[exitIndex].transform.position, Quaternion.identity).GetComponent<SpellScript>();
        spellScript.Initialize(currentSpell.Speed, currentSpell.Damage, currentSpell.Aoe);

        // Add fields to the spell as necessary
        if (currentSpell.MySpellType == Spell.SpellType.Projectile) {
            ((ProjectileSpellScript)spellScript).InitialRotation = currentSpell.InitialRotation;
        }
        if (currentSpell.MySpellType == Spell.SpellType.Global) {
            ((GlobalSpellScript)spellScript).InitialY = currentSpell.InitialY;
        }
        spellScript.Target = Target;
    }

    // Stop attacking
    public override void StopAttacking()
    {
        spellBook.StopCasting(); // If doing melee, this doesn't do anything
        meleeAoe.SetActive(false); // If spell casting, this doesn't do anything
        base.StopAttacking();
    }

    // Check if enemy is in line of sight
    private bool InLineOfSight ()
    {
        // Make sure there is a target
        if (Target == null) {
            return false;
        }

        Vector3 targetDirection = (Target.position - transform.position).normalized;
        // 8 is blocks layermask
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.position), blockLayerMask);
        return hit.collider == null;
    }

    // Check if spell requires line of sight
    private bool RequiresLineOfSight () 
    {
        // Currently, everything needs LOS except Stalker
        return attackMode.selectedAttackMode != AttackMode.STALKER;
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

    // Used to encapsulate which attack is being used to control UI, as well as a link with the SpellBook
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

            if (attackMode < 4) { // Change attack mode 
                foreach (GameObject frame in attackModeFrames)
                {
                    frame.SetActive(false);
                }
                attackModeFrames[selectedAttackMode].SetActive(true);
            }
        }
    }
}
