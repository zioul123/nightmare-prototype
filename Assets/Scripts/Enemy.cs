using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Npc 
{
    [SerializeField]
    private CanvasGroup healthGroup;

    public override Transform Select()
    {
        healthGroup.alpha = 1;
        return base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
        healthGroup.alpha = 0;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHealthChange(Health.CurrentValue);
    }

}
