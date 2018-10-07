using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour 
{
    // Activate/Deactivate the spawner
    [SerializeField]
    private bool active;
    [SerializeField]
    private GameObject prefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            if (gameObjects.Length < 10)
            {
                float x = Random.Range(-9f, 9f);
                float y = Random.Range(-4.5f, 4.5f);

                Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);

            }
        }
	}
}
