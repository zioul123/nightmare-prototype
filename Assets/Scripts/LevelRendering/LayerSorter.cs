using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour 
{
    // The SpriteRenderer of the object this LayerSorter is attached to
    private SpriteRenderer parentRenderer;

    // The list of Obstacles that are currently touching the collider of the LayerSorter
    private List<Obstacle> obstacles;

	// Use this for initialization
	void Start () {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        obstacles = new List<Obstacle>();
	}
	
    // Move behind obstacles when contact is made
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle") {
            Obstacle o = collision.GetComponent<Obstacle>();
            // Fade the object
            o.FadeOut();

            // Shift the parent behind this obstacle, unless parent is already behind the obstacle
            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder) {
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }

            // Add to list of current obstacles
            obstacles.Add(o);
        }
    }

    // Remove obstacles from list and go back in front if applicable
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Obstacle") {
            Obstacle o = other.GetComponent<Obstacle>();
            // Fade the obstacle in
            o.FadeIn();

            // Remove it from current obstacles
            obstacles.Remove(o);

            // Set parent back to the front if no more obstacles colliding
            if (obstacles.Count == 0) {
                parentRenderer.sortingOrder = 0;
            // Otherwise put it behind the backmost obstacle
            } else {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
