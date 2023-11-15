using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public GameObject player;  // Reference to the player GameObject
    private Camera mainCamera;  // Reference to the main camera
    //private float sstimer = 1.0f;
    //public ScreenShake ss;
    public AudioSource spikeSFX;

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
            /*if (sstimer <= 0)
            {
                ss.Shake(.2f, 1.4f);
                spikeSFX.Play();
                sstimer = 1f;
            }*/
        }

        //sstimer -= Time.deltaTime;
    }
}
