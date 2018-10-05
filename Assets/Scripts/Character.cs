using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /*
     * Fields
     */
    // The Character's movement speed
    [SerializeField]
    private float speed = 1f;
    // The Character's direction
    protected Vector2 direction;
    // The Character's animator
    private Animator animator;
    // The Character's rigidbody
    private Rigidbody2D rigidBody;

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

    /*
     * Movement
     */

    // Handle Physics
    private void FixedUpdate ()
    {
        MoveCharacter();
    }

    // Moves the Character
    public void MoveCharacter ()
    {
        rigidBody.velocity = direction * speed;
    }

    /*
     * Animation
     */

    // Animate the Character
    private void AnimateCharacter()
    {
        if (IsMoving)
        {
            SetWalkAnimation(direction);
            return;
        }
        SetIdleAnimation();
    }

    // Animate for idle state
    public void SetIdleAnimation() 
    {
        ActivateLayer("IdleLayer");
    }

    // Animate for movement
    public void SetWalkAnimation(Vector2 direction) 
    {
        ActivateLayer("WalkLayer");

        // Set animator parameters to trigger direction
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }

    /*
     * Utility
     */
    // Return whether the Character is moving
    public bool IsMoving {
        get {
            return direction != Vector2.zero;
        }
    }

    // Set the animation layer to be active
    public void ActivateLayer (string layerName)
    {

        for (int i = 0; i < animator.layerCount; i++) {
                animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }
}
