using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    // The Character's movement speed
    [SerializeField]
    private float speed;

    // The Character's direction
    protected Vector2 direction;

    // The Character's animator
    private Animator animator;

    // Use this for initialization
    protected virtual void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        Move();
	}

    // Moves the Character
    public void Move () {

        transform.Translate(direction.normalized * speed * Time.deltaTime);
        AnimateMovement(direction);
    }

    public void AnimateMovement(Vector2 direction) {
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }
}
