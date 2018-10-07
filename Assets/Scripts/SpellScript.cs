using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellScript : MonoBehaviour {
    // The RigidBody of the spell
    protected Rigidbody2D rigidBody;
    // Target of the spell
    public Transform Target { get; set; }
    // The speed this spell moves
    [SerializeField]
    protected float speed = 1;

    // Use this for initialization
    protected virtual void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Default trigger when spell hits an enemy
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Possibly elete the second condition for piercing spells
        if (collision.tag == "HitBox" && collision.transform == Target) {
            // Animate effect
            GetComponent<Animator>().SetTrigger("impact");
            // Stop moving the spell
            rigidBody.velocity = Vector2.zero;
            // Stop triggering motion
            Target = null;
        }
    }
}
