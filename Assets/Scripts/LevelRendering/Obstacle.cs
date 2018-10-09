using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    // Two colors (same color with different alpha) that the obstacle will take 
    private Color defaultColor;
    private Color fadedColor;

    // The SpriteRenderer of this Obstacle
    public SpriteRenderer MySpriteRenderer { get; set; }

    // Sort Obstacles by sortingOrder
    int IComparable<Obstacle>.CompareTo(Obstacle other)
    {
        return MySpriteRenderer.sortingOrder - other.MySpriteRenderer.sortingOrder;
    }

    // Use this for initialization
    void Start () 
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MySpriteRenderer.color;
        // Faded color is a copy of the default color but with transparency
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
	}

    // Set the faded color
    public void FadeOut ()
    {
        MySpriteRenderer.color = fadedColor;
    }

    // Set the default color
    public void FadeIn ()
    {
        MySpriteRenderer.color = defaultColor;
    }
}
