using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spell that instantly hits an enemy
public class PointCastSpellScript : SpellScript 
{
    // Use to damage only once
    private bool triggered = false;
    // Use this for initialization
    protected override void Start () 
    {
        base.Start();
        // Initial position
        transform.position = Target.position;
    }

    // Handle movement or instant following
    private void FixedUpdate ()
    {
        if (Target != null) {
            transform.position = Target.position;
        } else {
            // Explode it
            GetComponent<Animator>().SetTrigger("impact");
        }
    }

    // Trigger instantly, and only once
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered) {
            // After 0.5s, change animation
            StartCoroutine(Explode());
            triggered = true;
        }
    }

    // Trigger change of ring to explosion
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("impact");
        Target.GetComponentInParent<Enemy>().TakeDamage(damage);
    }
}
