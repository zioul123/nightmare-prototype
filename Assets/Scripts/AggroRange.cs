using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRange : MonoBehaviour 
{
    // The Enemy that possesses this aggrorange
    Enemy parent;

	// Use this for initialization
	void Start () {
        parent = GetComponentInParent<Enemy>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Distance: " + Vector2.Distance(parent.transform.position, collision.transform.position));
        //Debug.Log("Aggro range entered");
        if (collision.CompareTag("Player")) {
            parent.SetTarget(collision.transform);
            if (parent.CurrentState is EvadeState) {
                parent.ChangeState(new FollowState());
            }
        }
    }
}
