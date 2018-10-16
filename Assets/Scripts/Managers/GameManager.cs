using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    private static GameManager instance;

    // The player object
    [SerializeField]
    private Player player;

    // The targetted Npc
    private Npc currentTarget;

    // Index of the clickable layer mask
    int clickableLayerMask;

    // Use this for initialization
    void Start()
    {
        clickableLayerMask = LayerMask.GetMask("Clickable");
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    // Handle the case that an Npc is clicked
    private void ClickTarget()
    {
        // Only check for Npc if it's not clicking a UI element like a button
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Raycast from mouse position to anything in the clickable layer - only NPCs will be hit
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayerMask);

            // When there is something, select
            if (hit.collider != null)
            {
                SelectTarget(hit.collider);
            }
            // Nothing collided with the click, just deselect
            else
            {
                DeselectTarget();
            }
        }
    }

    // Select the provided target
    public void SelectTarget(Collider2D target) 
    {
        // Deselect current target
        if (CurrentTarget != null) {
            CurrentTarget.Deselect();
        }

        // Get the character's Npc script first
        CurrentTarget = target.GetComponent<Npc>();
        // Select the hitbox for spell casting
        player.Target = CurrentTarget.Select();
        // Show target frame with the Npc's character
        UIManager.Instance.ShowTargetFrame(CurrentTarget);
        // For Debug
        Debug.Log("Selected target: " + target.gameObject.name);
    }

    // Deselect current target
    public void DeselectTarget() 
    {
        // Deselect current target
        if (CurrentTarget != null)
        {
            CurrentTarget.Deselect();
        }
        CurrentTarget = null;
        player.Target = null;
        // Hide UI Frame
        UIManager.Instance.HideTargetFrame();
        // For Debug
        Debug.Log("Deselected target.");
    }

    // Singleton getter
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public Npc CurrentTarget
    {
        get
        {
            return currentTarget;
        }

        set
        {
            currentTarget = value;
        }
    }
}
