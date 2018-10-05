using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    /*
     * Static constants  
     */
    // For Animations
    public const int IDLE_ANIMATION_LAYER = 0;
    public const int WALK_ANIMATION_LAYER = 1;

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

    /*
     * Methods
     */
    // Use this for initialization
    protected virtual void Start () 
    {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {
        HandleMovement();
	}

    // Moves the Character
    public void HandleMovement ()
    {
        TranslateCharacter();
        AnimateCharacter();
    }

    // Translate the Character
    private void TranslateCharacter () 
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    // Animate the Character
    private void AnimateCharacter()
    {
        if (direction == Vector2.zero)
        {
            SetIdleAnimation();
            return;
        }
        SetWalkAnimation(direction);
    }

    // Animate for idle state
    public void SetIdleAnimation() 
    {
        animator.SetLayerWeight(WALK_ANIMATION_LAYER, 0);
    }

    // Animate for movement
    public void SetWalkAnimation(Vector2 direction) 
    {
        // Set animator's walk layer to priority 1
        animator.SetLayerWeight(WALK_ANIMATION_LAYER, 1);

        // Set animator parameters to trigger direction
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }
}
