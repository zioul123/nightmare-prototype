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
    private float speed = 1f;
    // The Character's direction
    protected Vector2 direction;
    // The Character's animator
    protected Animator animator;
    // The Character's rigidbody
    private Rigidbody2D rigidBody;
    // The Character's hitbox
    [SerializeField]
    protected Transform hitBox;

    // The Attack layer to use
    private string attackLayer;
    // Whether the Character is attacking
    protected bool isAttacking = false;
    // Attack Coroutine
    protected Coroutine attackRoutine;

    /*
     * Methods
     */
    // Use this for initialization
    protected virtual void Start () 
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        rigidBody.velocity = direction * speed;
    }

    // Stop attacking
    public virtual void StopAttacking()
    {
        isAttacking = false; // Stop attacking in script
        animator.SetBool("attack", isAttacking); // Stop attacking in animation controller

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
            Debug.Log("Stopped attacking coroutine.");
        }
    }

    /*
     * Animation
     */
    // Animate the Character
    private void AnimateCharacter ()
    {
        if (IsMoving) {
            SetWalkAnimation(direction);
            StopAttacking();
        } else if (isAttacking) {
            SetAttackAnimation();
        } else {
            SetIdleAnimation();
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
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

    /*
     * Utility
     */
    // Set the animation layer to be active
    public void ActivateLayer (string layerName)
    {
        for (int i = 0; i < animator.layerCount; i++) {
                animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }

    // Whether the Character is moving
    public bool IsMoving
    {
        get
        {
            return direction != Vector2.zero;
        }
    }

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
