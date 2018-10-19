using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour 
{
    // The Enemy that possesses this aggrorange
    Enemy parent;

    // Whether this is a Far target or close targetter
    [SerializeField]
    bool FarTarget;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<Enemy>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            if (FarTarget) {
                parent.FarTarget = collision.transform;
            } else {
                parent.Target = collision.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (FarTarget) {
                parent.FarTarget = null;
            } else {
                parent.Target = null;
            }
        }
    }
}
