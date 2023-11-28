using UnityEngine;
using System.Collections;

[ExecuteInEditMode] // Allows the script to run in the editor
public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn; // The prefab to instantiate
    [Range(1, 20)] public int numberOfInstances = 1; // Number of instances to spawn, with a slider from 1 to 20
    public float spacing = 1.0f; // Spacing between instances
    public bool isOriginal = true; // Flag to indicate if this is the original GameObject
    private float rotationSpeed = 40.0f; // Degrees per second

    public AudioSource spikeSFX;

    public Transform mainCamera;
    private float shakeDuration = 0.25f;
    private float shakeMagnitude = 2f;

    // Draw gizmos in the editor for preview
    void OnDrawGizmos()
    {
        if (isOriginal)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < numberOfInstances; i++)
            {
                // Calculate the position for each instance
                Vector3 spawnPosition = transform.position + i * spacing * transform.up;

                // Draw a small cube gizmo at each spawn position
                Gizmos.DrawCube(spawnPosition, new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Automatically assign the main camera
        mainCamera = Camera.main.transform;

        // Only spawn prefabs if we're running the game and this is the original GameObject
        if (Application.isPlaying && isOriginal)
        {
            SpawnPrefabs();
        }
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            // Calculate the position for each instance
            Vector3 spawnPosition = transform.position + i * spacing * transform.up;

            // Instantiate the prefab at the calculated position with the same rotation as the original
            GameObject instance = Instantiate(prefabToSpawn, spawnPosition, transform.rotation);

            // Set isOriginal to false for the new instance to prevent it from spawning more instances
            PrefabSpawner spawner = instance.GetComponent<PrefabSpawner>();
            if (spawner != null)
            {
                spawner.isOriginal = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player collided with spike
        if (collision.gameObject.name.Equals("Player"))
        {
            collision.gameObject.GetComponent<Respawn>().SpawnCharacter();
            spikeSFX.Play();

            // Start the camera shake coroutine
            //StartCoroutine(CameraShake());
        }
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalPosition = mainCamera.position;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.position = originalPosition;
    }

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

}
