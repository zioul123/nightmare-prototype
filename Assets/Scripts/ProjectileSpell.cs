using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpell : MonoBehaviour {

    // The RigidBody of the spell
    private Rigidbody2D rigidBody;
    // The speed this spell moves
    [SerializeField]
    private float speed = 1;
    // Target of the spell
    private Transform target;
    // Initial rotation of the spell for offsetting the animation
    [SerializeField]
    private float initialRotation;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        // FOR DEBUG
        target = GameObject.Find("Target").transform;

        // Fix direction and rotation
        FixedUpdate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Handle movement and rotation
    private void FixedUpdate ()
    {
        // Handle movement
        Vector2 direction = target.transform.position - transform.position;
        rigidBody.velocity = direction.normalized * speed;
        // Handle rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - initialRotation;
        rigidBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
