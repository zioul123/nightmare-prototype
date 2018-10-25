using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Npc 
{
    // The health bar + empty bar
    [SerializeField]
    private CanvasGroup healthGroup;
    // The selection circle
    [SerializeField]
    private SpriteRenderer selectionCircle;
    // To stop the health group from fading
    private Coroutine hideHealthRoutine;
    // The target of this enemy
    private Transform target;
    // The possible target of this enemy that is outside of aggro range
    private Transform farTarget;
    // The attack range of this enemy for initial attack
    [SerializeField]
    private float attackRange;
    // Runaway range of this enemy
    [SerializeField]
    private float extraRange = 0.1f;
    [SerializeField]
    private float attackCooldown;
    public float AttackCooldown { get; set; }
    public float TimeSinceLastAttack { get; set; }

    // Current state of the enemy
    private IState currentState;

    // Initialization
    protected void Awake()
    {
        ChangeState(new IdleState());
        AttackCooldown = attackCooldown;
        TimeSinceLastAttack = AttackCooldown; // Start with ability to attack already
    }

    // When selected, show the healthGroup
    public override Transform Select()
    {
        // Show health group and selection circle
        ShowHealthGroup();
        ShowSelectionCircle();

        // Stop the hiding health coroutine from completing if it exists
        if (hideHealthRoutine != null) {
            StopCoroutine(hideHealthRoutine);
            hideHealthRoutine = null;
        }

        return base.Select();
    }

    // Hide selection circle and trigger health group to disappear
    public override void Deselect()
    {
        base.Deselect();
        // If the enemy is undamaged, hide health
        if (!IsDamaged) {
            HideHealthGroup();
        // Otherwise make it disappear after a few seconds
        } else {
            hideHealthRoutine = StartCoroutine(HideHealthGroupAfterDelay());
        }
        HideSelectionCircle();
    }

    protected override void Update()
    {
        if (!IsDead) {
            if (!IsAttacking) {
                TimeSinceLastAttack += Time.deltaTime;
            }
            currentState.Update();
        }
        base.Update();

    }

    // Hide the healthgroup after a few seconds delay
    public IEnumerator HideHealthGroupAfterDelay() {
        yield return new WaitForSeconds(5);
        HideHealthGroup();
        hideHealthRoutine = null;
    }

    // Perform character functions and call NPC's function that activates listeners. Also show health group for a short while.
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChange(Health.CurrentValue);

        // Target the person who attacked it if possible
        if (target == null) {
            if (farTarget != null) {
                target = farTarget;
            }
        }

        // Health is not currently shown - not selected nor attacked within 5 seconds ago
        if (healthGroup.alpha <= 0) {
            ShowHealthGroup();
            // Restart the hide health timer if necessary
            if (hideHealthRoutine != null)
            {
                StopCoroutine(hideHealthRoutine);
                hideHealthRoutine = null;
            }
            hideHealthRoutine = StartCoroutine(HideHealthGroupAfterDelay());
        // Health is already shown
        } else {
            // It is there by selection
            if (hideHealthRoutine == null) {
                return;
            // It is there by damage
            } else if (hideHealthRoutine != null) {
                // Restart the hide health timer if necessary
                StopCoroutine(hideHealthRoutine);
                hideHealthRoutine = null;
                hideHealthRoutine = StartCoroutine(HideHealthGroupAfterDelay());
            }
        }
    }

    // Show the health group
    private void ShowHealthGroup() 
    {
        healthGroup.alpha = 1;
    }

    // Show the selection circle
    private void ShowSelectionCircle() 
    {
        selectionCircle.gameObject.SetActive(true);
    }

    // Hide the health group
    private void HideHealthGroup()
    {
        healthGroup.alpha = 0;
    }

    // Hide  the selection circle
    private void HideSelectionCircle()
    {
        selectionCircle.gameObject.SetActive(false);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null) {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    // Getter/setters
    public Transform Target { get { return target; } set { target = value; }}
    public Transform FarTarget { get { return farTarget; } set { farTarget = value; } }
    public bool IsDamaged { get { return Health.CurrentValue < Health.MaxValue; } }
    public float AttackRange { get { return attackRange; } set { attackRange = value; } }
    public float ExtraRange { get { return extraRange; } set { extraRange = value; } }
}
