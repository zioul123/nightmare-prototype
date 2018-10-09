using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour 
{
    // The player object
    [SerializeField]
    private Player player;

    // The targetted Npc
    private Npc currentTarget;

    // Index of the clickable layer mask
    int clickableLayerMask; 

    // Use this for initialization
    void Start () {
        clickableLayerMask = LayerMask.GetMask("Clickable");
    }
	
	// Update is called once per frame
	void Update () {
        ClickTarget();
	}

    // Handle the case that an Npc is clicked
    private void ClickTarget() 
    {
        // Only check for Npc if it's not clicking a UI element like a button
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject()) {
            // Raycast from mouse position to anything in the clickable layer - only NPCs will be hit
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMask);

            // When there is something, select
            if (hit.collider != null) {
                // Deselect current target
                if (currentTarget != null) {
                    currentTarget.Deselect();
                }

                // Get the character's Npc script first
                currentTarget = hit.collider.GetComponent<Npc>();
                // Select the hitbox for spell casting
                player.Target = currentTarget.Select();
                // Show target frame with the Npc's character
                UIManager.Instance.ShowTargetFrame(currentTarget);
                // For Debug
                Debug.Log("Selected target: " + hit.collider.gameObject.name);
            // Nothing collided with the click, just deselect
            } else {
                // Deselect current target
                if (currentTarget != null) {
                    currentTarget.Deselect();
                }
                currentTarget = null;
                player.Target = null;
                // Hide UI Frame
                UIManager.Instance.HideTargetFrame();
                // For Debug
                Debug.Log("Deselected target.");
            }
        }
    }
}
