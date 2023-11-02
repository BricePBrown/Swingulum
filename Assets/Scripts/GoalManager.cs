using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public AudioSource goalSFX;
    public MMFeedbacks goalReachedFeedbacks;

    public Camera mainCamera;

    void Start()
    {
        // Automatically assign the main camera
        mainCamera = Camera.main;
    }

        private void OnTriggerEnter(Collider other)
    {
        goalSFX.Play();
        goalReachedFeedbacks.PlayFeedbacks();
        despawnCharacter(other.gameObject);
        StartCoroutine(CameraPulse(0.5f, 2f));  // 1 second duration, 0.5 magnitude
                                                // Start the scene transition
        FindObjectOfType<SceneTransitionManager>().StartTransition();
    }


    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Get the index of the current active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Calculate the index of the next scene
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // Load the next scene (or loop back to the first scene if it's the last scene)
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void despawnCharacter(GameObject currentPlayer)
    {
        currentPlayer.GetComponent<MeshRenderer>().enabled = false;
        // Get all TrailRenderers in the children of currentPlayer
        TrailRenderer[] trails = currentPlayer.GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trail in trails)
        {
            StartCoroutine(LerpTrailTimeToZero(trail, 0.25f));  // 0.5 seconds duration
        }

        // Assuming currentPlayer is a GameObject with a Rigidbody component
        Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset velocity and angular velocity
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        
    }

    private IEnumerator CameraPulse(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        float originalSize = mainCamera.orthographicSize;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // Calculate an offset using a sine wave that goes from -1 to +1
            float offset = Mathf.Sin(percentComplete * Mathf.PI * 2) * magnitude;

            // Set the camera's orthographic size to the original value + offset
            mainCamera.orthographicSize = originalSize + offset;

            yield return null;
        }

        // Reset the camera's orthographic size to its original value
        mainCamera.orthographicSize = originalSize;
    }



    IEnumerator LerpTrailTimeToZero(TrailRenderer trail, float duration)
    {
        float originalTime = trail.time;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trail.time = Mathf.Lerp(originalTime, 0, t);
            yield return null;
        }

        trail.time = 0;
        trail.enabled = false;
    }
}
