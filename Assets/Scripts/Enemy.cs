using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Npc 
{
    // The health bar + empty bar
    [SerializeField]
    private CanvasGroup healthGroup;

    // When selected, show the healthGroup
    public override Transform Select()
    {
        healthGroup.alpha = 1;
        return base.Select();
    }

    // Hide the healthGroup
    public override void Deselect()
    {
        base.Deselect();
        healthGroup.alpha = 0;
    }

    // Perform character functions and call NPC's function that activates listeners
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChange(Health.CurrentValue);
    }

}
