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
    public Sprite Portrait
    {
        get
        {
            return portrait;
        }
    }

    public virtual void Deselect() 
    {
        healthChange -= UIManager.Instance.UpdateTargetFrame;
        characterRemoved -= UIManager.Instance.HideTargetFrame;
    }

    public virtual Transform Select()
    {
        return hitBox;
    }


    public void OnHealthChange(float health)
    {
        if (healthChange != null)
        {
            healthChange(health);
        }
    }

    public void OnCharacterRemoved () 
    {
        if (characterRemoved != null) 
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }
}
