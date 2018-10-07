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

    public GameObject SpellPrefab
    {
        get
        {
            return spellPrefab;
        }
    }
}
