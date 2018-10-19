using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour 
{
    // UI Elements
    [SerializeField]
    private Image castingBar;
    [SerializeField]
    private Text spellName;
    [SerializeField]
    private Text timeLeft;
    [SerializeField]
    private Image spellIcon;
    [SerializeField]
    private CanvasGroup canvasGroup;

    // Spells in this spellbook
    [SerializeField]
    private Spell[] spells;

    // To track parallel process
    private Coroutine spellRoutine;
    private Coroutine fadeRoutine;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Get a spell from the SpellBook
    public Spell GetSpell (int index) 
    {
        return spells[index];
    }

    // Activate the cast bar
    public void StartCastBar (int index)
    {
        // Set UI elements
        castingBar.color = spells[index].SpellColor;
        spellName.text = spells[index].Name;
        timeLeft.text = spells[index].CastTime.ToString();
        spellIcon.sprite = spells[index].Icon;
        castingBar.fillAmount = 0;

        // Set cast bar
        spellRoutine = StartCoroutine(Progress(index));

        // Fade the bar in
        fadeRoutine = StartCoroutine(FadeBar());
    }

    private IEnumerator Progress (int index) 
    {
        float timeDone = Time.deltaTime;
        float rate = 1.0f / spells[index].CastTime;
        float progress = 0.0f;

        while (progress <= 1.0f) {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);
            timeLeft.text = (spells[index].CastTime - timeDone).ToString("F1");
            if (spells[index].CastTime - timeDone < 0) {
                timeLeft.text = "0.0";
            }

            progress += Time.deltaTime * rate;
            timeDone += Time.deltaTime;

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator FadeBar () 
    {
        // Appear in 0.2s
        float rate = 1.0f / 0.2f;
        float progress = 0.0f;

        while (progress <= 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += Time.deltaTime * rate;
            yield return null;
        }
    }

    public void StopCasting() 
    {
        if (spellRoutine != null) {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }
    }
}
