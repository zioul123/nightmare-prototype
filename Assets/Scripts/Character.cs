using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    public enum AttackLayer { MeleeLayer, ShootLayer, CastLayer };

    // The Character's movement speed
    [SerializeField]
    protected float speed = 1f;
    // The Character's direction
    private Vector2 direction;
    // The Character's animator
    private Animator animator;
    // The Character's rigidbody
    private Rigidbody2D rigidBody;
    // The Character's hitbox
    [SerializeField]
    protected Transform hitBox;
    // The Character's HP stat
    [SerializeField]
    private Stat health;
    [SerializeField]
    private float maxHealth;

    // The Attack layer to use
    private string attackLayer;
    // Whether the Character is attacking
    private bool isAttacking = false;
    // Whether using melee or another kind of attack; melee can occur while moving
    private bool isMeleeing = false;
    // Attack Coroutine
    private Coroutine attackRoutine;

    // Target of the character
    public Transform Target { get; set; }

    /*
     * Methods
     */
    // Use this for initialization
    protected virtual void Start () 
    {
        // Get components
        Animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        // Initialize stats
        Health.Initialize(maxHealth, maxHealth);

    }

    // Update is called once per frame
    protected virtual void Update () 
    {
        AnimateCharacter();
    }

    // Handle Physics
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    /*
     * Mechanics
     */
    // Move the Character
    public void MoveCharacter ()
    {
        rigidBody.velocity = Direction * speed;
    }

    // Inflict damage on this character
    public virtual void TakeDamage(float damage, Transform source) 
    {
        Health.CurrentValue -= damage;
        Debug.Log("Current health: " + Health.CurrentValue);

        if (IsDead) {
            direction = Vector2.zero;
            rigidBody.velocity = direction;
        }

        if (Health.CurrentValue <= 0) {
            Animator.SetTrigger("die");
        }
    }

    /*
     * Animation
     */
    // Animate the Character
    private void AnimateCharacter ()
    {
        if (!IsDead) {
            if (IsMoving && !IsMeleeing) {
                SetWalkAnimation(Direction);
            } else if (IsAttacking) {
                SetAttackAnimation();
            } else {
                SetIdleAnimation();
            }
        } else {
            SetDeathAnimation();
        }
    }

    // Animate for attack.
    // Precondition: SetAttackLayer should be called first.
    public void SetAttackAnimation ()
    {
        if (attackLayer != null) {
            ActivateLayer(attackLayer);
        } else {
            Debug.Log("Set attackLayer before setting attack animation.");
        }
    }

    // Animate for death
    private void SetDeathAnimation()
    {
        Debug.Log("Died");
        ActivateLayer("DeathLayer");
    }

    // Animate for idle state
    public void SetIdleAnimation () 
    {
        ActivateLayer("IdleLayer");
    }

    // Animate for movement
    public void SetWalkAnimation (Vector2 direction) 
    {
        ActivateLayer("WalkLayer");

        // Set animator parameters to trigger direction
        Animator.SetFloat("x", direction.x);
        Animator.SetFloat("y", direction.y);
    }

    /*
     * Utility
     */
    // Set the animation layer to be active
    public void ActivateLayer (string layerName)
    {
        for (int i = 0; i < Animator.layerCount; i++) {
                Animator.SetLayerWeight(i, 0);
        }
        Animator.SetLayerWeight(Animator.GetLayerIndex(layerName), 1);
    }

    // Whether the Character is moving
    public bool IsMoving { get { return Direction != Vector2.zero; } }

    public Stat Health { get { return health; } set { health = value; } }

    public Vector2 Direction { get { return direction; } set { this.direction = value; } }

    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }

    public bool IsMeleeing { get { return isMeleeing; } set { isMeleeing = value; } }

    public Animator Animator { get { return animator; } set { animator = value; } }

    public Coroutine AttackRoutine { get { return attackRoutine; }  set { attackRoutine = value; } }

    public bool IsDead { get { return Health.CurrentValue <= 0; } }

    // Set the attackLayer string based on the AttackLayer enum
    public void SetAttackLayer(AttackLayer al)
    {
        switch (al)
        {
            case AttackLayer.MeleeLayer:
                attackLayer = "MeleeLayer";
                break;
            case AttackLayer.ShootLayer:
                attackLayer = "ShootLayer";
                break;
            case AttackLayer.CastLayer:
                attackLayer = "CastLayer";
                break;
            default:
                Debug.Log("Invalid AttackLayer");
                break;
        }
    }
}
