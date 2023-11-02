using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject
    private Camera mainCamera;  // Reference to the main camera

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Convert the player's world position to viewport position
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(player.transform.position);

        // Check if the player is off-screen
        if (viewportPosition.x < -1 || viewportPosition.x > 2 || viewportPosition.y < 0)
        {
            // Trigger respawn
            player.GetComponent<Respawn>().SpawnCharacter();
        }
    }
}
