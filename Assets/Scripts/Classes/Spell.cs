using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell 
{
    public enum SpellType { Projectile, Global, PointCast }

    // Customizable spell characteristics
    [SerializeField]
    private string name;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed;

    // Characteristic of the spell for coding purposes
    [SerializeField]
    private SpellType spellType;
    [SerializeField]
    private Character.AttackLayer attackLayer;
    [SerializeField]
    private float initialRotation;
    [SerializeField]
    private float initialY;
    [SerializeField]
    private float castTime;
    [SerializeField]
    private float trailingAnimationTime;

    // Appearance related
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private Color spellColor;

    public GameObject SpellPrefab
    {
        get
        {
            return spellPrefab;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
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

    public SpellType MySpellType
    {
        get
        {
            return spellType;
        }

        set
        {
            spellType = value;
        }
    }

    public Character.AttackLayer AttackLayer
    {
        get
        {
            return attackLayer;
        }

        set
        {
            attackLayer = value;
        }
    }

    public float InitialRotation
    {
        get
        {
            return initialRotation;
        }

        set
        {
            initialRotation = value;
        }
    }

    public float InitialY
    {
        get
        {
            return initialY;
        }

        set
        {
            initialY = value;
        }
    }

    public float CastTime
    {
        get
        {
            return castTime;
        }

        set
        {
            castTime = value;
        }
    }

    public float TrailingAnimationTime
    {
        get
        {
            return trailingAnimationTime;
        }

        set
        {
            trailingAnimationTime = value;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public Color SpellColor
    {
        get
        {
            return spellColor;
        }

        set
        {
            spellColor = value;
        }
    }
}
