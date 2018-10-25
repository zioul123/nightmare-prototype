using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChange(float health);
public delegate void CharacterRemoved();

public class Npc : Character 
{
    // Health change of selected NPC
    public event HealthChange healthChange;
    // Death of selected NPC
    public event CharacterRemoved characterRemoved;

    // Portrait for frame display on selection
    [SerializeField]
    private Sprite portrait;

    // Remove any UI listeners from this NPC
    public virtual void Deselect() 
    {
        healthChange -= UIManager.Instance.UpdateTargetFrame;
        characterRemoved -= UIManager.Instance.HideTargetFrame;
    }

    // Return the enemy's hitbox as target when selected. When this is called, usually listeners will be attached as well.
    public virtual Transform Select()
    {
        return hitBox;
    }

    // What to do when NPC's health changes
    public void OnHealthChange(float health)
    {
        // Call any listeners for health change
        if (healthChange != null)
        {
            healthChange(health);
        }
    }

    // What to do when NPC is removed (killed)
    public void OnCharacterRemoved () 
    {
        // Call any listeners for this NPC's removal
        if (characterRemoved != null) 
        {
            characterRemoved();
        }
        // Destroy the NPC
        Destroy(gameObject);
    }

    public Sprite Portrait { get { return portrait; } }
}
