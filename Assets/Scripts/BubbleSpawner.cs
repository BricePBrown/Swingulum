using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // The prefab to spawn
    private float spawnHeight = 2.0f; // The height above the GameObject to spawn the prefab
    private float spawnInterval = 3f; // The time interval between spawns

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning bubbles
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
    }

    // Method to spawn a bubble
    void SpawnBubble()
    {
        // Calculate the spawn position
        Vector3 spawnPosition = transform.position + new Vector3(0, spawnHeight, 0);

        // Instantiate the bubble prefab at the calculated position
        Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
    }
}
