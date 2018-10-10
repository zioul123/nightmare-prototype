using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    // The target the camera follows
    private Transform target;
    // Max and minimum locations of the camera
    private float minX, maxX, minY, maxY;

    // The tilemap of the ground
    [SerializeField]
    private Tilemap tilemap;

	// Use this for initialization
	void Start () 
    {
        // Get the player to track
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the position of the min and max tiles
        Vector3 minTile = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 maxTile = tilemap.CellToWorld(tilemap.cellBounds.max);

        // Offset max x by -1 because it will trail off for some reason.
        Debug.Log(minTile + ", " + maxTile + ". Offsetting max x by -1.");
        maxTile.x -= 1;

        // Set the limits of the camera
        SetLimits(minTile, maxTile);

        // Set the limits of the player
        minTile.y += 1;
        maxTile.x -= 0.5f;
        minTile.x += 0.5f;
        target.gameObject.GetComponent<Player>().SetLimits(minTile, maxTile);
    }

    // Called after movement update
    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), transform.position.z);
    }

    // Set the limits of the camera given the two corners of the tilemap
    private void SetLimits(Vector3 minTile, Vector3 maxTile) 
    {
        // Get the camera
        Camera cam = Camera.main;

        // Get the height and width of the camera
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        minX = minTile.x + width / 2;
        maxX = maxTile.x - width / 2;
        minY = minTile.y + height / 2;
        maxY = maxTile.y - height / 2;
    }
}
