using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour 
{
    // Singleton pattern
    private static UIManager instance;
    public static UIManager Instance 
    {
        get {
            if (instance == null) {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    // Action buttons in the UI
    [SerializeField]
    private Button[] actionButtons;

    private KeyCode ab1, ab2, ab3, ab4, ab5, ab6, ab7;

    // The target frame
    [SerializeField]
    private GameObject targetFrame;
    private Stat healthStat;
    [SerializeField]
    private Image portraitImage;

	// Use this for initialization
	void Start () {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
        ab1 = KeyCode.Alpha1;
        ab2 = KeyCode.Alpha2;
        ab3 = KeyCode.Alpha3;
        ab4 = KeyCode.Alpha4;
        ab5 = KeyCode.Alpha5;
        ab6 = KeyCode.Alpha6;
        ab7 = KeyCode.Alpha7;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(ab1)) {
            ActionButtonOnClick(0);
        }
        if (Input.GetKeyDown(ab2)) {
            ActionButtonOnClick(1);
        }
        if (Input.GetKeyDown(ab3)) {
            ActionButtonOnClick(2);
        }
        if (Input.GetKeyDown(ab4)) {
            ActionButtonOnClick(3);
        }
        if (Input.GetKeyDown(ab5)) {
            ActionButtonOnClick(4); 
        }
        if (Input.GetKeyDown(ab6)) {
            ActionButtonOnClick(5);
        }
        if (Input.GetKeyDown(ab7)) {
            ActionButtonOnClick(6);
        }
    }

    private void ActionButtonOnClick (int btnIndex) 
    {
        actionButtons[btnIndex].onClick.Invoke();
    }

    public void ShowTargetFrame(Npc target) 
    {
        targetFrame.SetActive(true);
        healthStat.Initialize(target.Health.CurrentValue, target.Health.MaxValue);
        portraitImage.sprite = target.Portrait;

        // Add health changed listener to the NPC
        target.healthChange += UpdateTargetFrame;
        // Add death listener to the NPC
        target.characterRemoved += HideTargetFrame;
    }

    public void HideTargetFrame ()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame (float value) 
    {
        healthStat.CurrentValue = value;
    }
}
