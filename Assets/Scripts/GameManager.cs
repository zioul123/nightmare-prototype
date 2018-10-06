﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    // The player object
    [SerializeField]
    private Player player;
    // Index of the clickable layr mask
    int clickableLayerMask; 

    // Use this for initialization
    void Start () {
        clickableLayerMask = LayerMask.GetMask("Clickable");
    }
	
	// Update is called once per frame
	void Update () {
        ClickTarget();
	}

    private void ClickTarget() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMask);
            // Select
            if (hit.collider != null && hit.collider.tag == "Enemy") {
                player.Target = hit.transform;
                Debug.Log("Selected target: " + hit.collider.gameObject);
            // Deselect
            } else if (player.Target != null) {
                    player.Target = null;
                    Debug.Log("Deselected target.");
            }
        }
    }
}
