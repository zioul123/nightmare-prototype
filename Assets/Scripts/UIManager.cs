using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    // Action buttons in the UI
    [SerializeField]
    private Button[] actionButtons;

    private KeyCode ab1, ab2, ab3, ab4, ab5, ab6, ab7;

	// Use this for initialization
	void Start () {
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
}
