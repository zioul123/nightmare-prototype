using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour 
{
    /*
     * Fields
     */
    // The Stat's bar graphic
    private Image content;
    // Speed at which bar graphic changes length
    [SerializeField]
    private float lerpSpeed;

    // The Maximum Value of this stat
    public float MaxValue { get; set; }

    // The ratio of CurrentValue/MaxValue; updated automatically when CurrentValue is changed.
    private float currentFill;
    // The Current value of this stat. Setter updates currentFill automatically.
    private float currentValue;
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value <= MaxValue && value >= 0) {
                currentValue = value;
            } else if (value > MaxValue) {
                currentValue = MaxValue;
            } else if (value < 0) {
                currentValue = 0;
            }
            // Update the fill whenever value is changed
            currentFill = CurrentValue / MaxValue;
        }
    }

    /*
     * Methods
     */
	// Use this for initialization
	void Start () 
    {
        content = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateBarFill();
	}

    // Fill the bar by the currentFill
    private void UpdateBarFill() 
    {
        if (content.fillAmount != currentFill) {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, lerpSpeed * Time.deltaTime);
        }
    }

    // For initializing values
    public void Initialize (float currentValue, float maxValue)
    {
        MaxValue = maxValue; // Must be set before currentValue, since Fill depends on it.
        CurrentValue = currentValue;
    }
}
