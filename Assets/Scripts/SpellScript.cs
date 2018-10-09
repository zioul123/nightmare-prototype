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
    private float aoe;

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

            // Attack all in the AOE
            if (aoe != 0) {
                AttackAllInAoe();
            }

            // Stop triggering motion
            Target = null;
        }
    }

    // Damage all enemies in the Aoe
    protected void AttackAllInAoe () 
    {
        // Get all enemies in AOE
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(Target.position.x, Target.position.y), aoe);

        // Damage all enemies that are not the original target (prevent double damage)
        foreach (Collider2D collider in colliders) {
            if (collider.gameObject.tag == "Enemy" && (Target.parent.gameObject != collider.gameObject)) {
                // Inflict damage
                collider.GetComponentInParent<Enemy>().TakeDamage(damage);
            }
        }
    }

    // Set the speed and damage of the spell
    public void Initialize(float speed, int damage, float aoe) 
    {
        Speed = speed;
        this.damage = damage;
        this.aoe = aoe;
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
