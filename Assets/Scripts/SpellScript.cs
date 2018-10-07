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
    private float speed = 1;
    protected int damage = 1;

    // Use this for initialization
    protected virtual void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Default trigger when spell hits an enemy
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: Possibly delete the second condition for piercing spells
        if (collision.tag == "HitBox" && collision.transform == Target) {
            // Inflict damage
            collision.GetComponentInParent<Enemy>().TakeDamage(damage);
            // Animate effect
            GetComponent<Animator>().SetTrigger("impact");
            // Stop moving the spell
            rigidBody.velocity = Vector2.zero;
            Speed = 0;
            // Stop triggering motion
            Target = null;
        }
    }

    // Set the speed and damage of the spell
    public void Initialize(float speed, int damage) 
    {
        this.Speed = speed;
        this.damage = damage;
    }

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }
}
