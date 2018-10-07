using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spell that instantly hits an enemy
public class PointCastSpell : Spell 
{
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
        transform.position = Target.position;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // After 0.5s, change animation
        StartCoroutine(Explode());
    }

    // Trigger change of ring to explosion
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("impact");
    }
}
