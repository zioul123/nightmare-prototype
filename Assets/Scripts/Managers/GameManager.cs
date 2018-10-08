using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour 
{
    // The player object
    [SerializeField]
    private Player player;

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

    private void ClickTarget() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject()) {
            // Raycast from mouse position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMask);

            // Select
            if (hit.collider != null) {
                // Deselect current target
                if (currentTarget != null)
                {
                    currentTarget.Deselect();
                }

                currentTarget = hit.collider.GetComponent<Npc>();
                player.Target = currentTarget.Select();
                UIManager.Instance.ShowTargetFrame(currentTarget);
                Debug.Log("Selected target: " + hit.collider.gameObject.name);
            
            } else {
                if (currentTarget != null) {
                    currentTarget.Deselect();
                }
                currentTarget = null;
                player.Target = null;
                UIManager.Instance.HideTargetFrame();
                Debug.Log("Deselected target.");
            }
        }
    }
}
