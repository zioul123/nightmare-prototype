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
    [SerializeField]
    private float aoe;

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

    // Getters
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
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public SpellType MySpellType
    {
        get
        {
            return spellType;
        }
    }

    public Character.AttackLayer AttackLayer
    {
        get
        {
            return attackLayer;
        }
    }

    public float InitialRotation
    {
        get
        {
            return initialRotation;
        }
    }

    public float InitialY
    {
        get
        {
            return initialY;
        }
    }

    public float CastTime
    {
        get
        {
            return castTime;
        }
    }

    public float TrailingAnimationTime
    {
        get
        {
            return trailingAnimationTime;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public Color SpellColor
    {
        get
        {
            return spellColor;
        }
    }

    public float Aoe
    {
        get
        {
            return aoe;
        }
    }
}
