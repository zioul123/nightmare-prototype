using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Npc 
{
    // The health bar + empty bar
    [SerializeField]
    private CanvasGroup healthGroup;
    // The selection circle
    [SerializeField]
    private SpriteRenderer selectionCircle;
    // To stop the health group from fading
    private Coroutine hideHealthRoutine;

    // When selected, show the healthGroup
    public override Transform Select()
    {
        // Show health group and selection circle
        healthGroup.alpha = 1;
        selectionCircle.gameObject.SetActive(true);

        // Stop the hiding health coroutine from completing if it exists
        if (hideHealthRoutine != null) {
            StopCoroutine(hideHealthRoutine);
            hideHealthRoutine = null;
        }

        return base.Select();
    }

    // Hide selection circle and trigger health group to disappear
    public override void Deselect()
    {
        base.Deselect();
        // If the enemy is undamaged, hide health
        if (Health.CurrentValue == Health.MaxValue) {
            healthGroup.alpha = 0;
        // Otherwise make it disappear after a few seconds
        } else {
            hideHealthRoutine = StartCoroutine(HideHealthGroup());
        }
        selectionCircle.gameObject.SetActive(false);
    }

    // Hide the healthgroup after a few seconds delay
    public IEnumerator HideHealthGroup() {
        yield return new WaitForSeconds(5);
        healthGroup.alpha = 0;
        hideHealthRoutine = null;
    }

    // Perform character functions and call NPC's function that activates listeners
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChange(Health.CurrentValue);
    }

}
