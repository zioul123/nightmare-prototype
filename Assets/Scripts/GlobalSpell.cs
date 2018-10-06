using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpell : MonoBehaviour 
{
    // The RigidBody of the spell
    private Rigidbody2D rigidBody;
    // Target of the spell
    private Transform target;
    // Initial offset of the spell
    [SerializeField]
    private float initialY;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool instantTracking;

	// Use this for initialization
	void Start () 
    {
        rigidBody = GetComponent<Rigidbody2D>();
        // FOR DEBUG
        target = GameObject.Find("Target").transform;

        // Initial position
        transform.position = target.position + new Vector3(0, initialY, 0);
        // Fix direction and rotation
        FixedUpdate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Handle movement or instant following
    private void FixedUpdate ()
    {
        if (instantTracking) {
            transform.position = target.position;
        } else {
            // Move to the location of the target
            Vector2 direction = target.transform.position - transform.position;
            rigidBody.velocity = direction.normalized * speed;
        }
    }
}
