using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTextChanger : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Get the TextMeshProUGUI component
        textMesh = GetComponent<TextMeshPro>();

        if (textMesh != null)
        {
            // Store the initial position and rotation
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            // Get the current scene
            Scene currentScene = SceneManager.GetActiveScene();

            // Set the text to the name of the current scene
            textMesh.text = currentScene.name;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found on this GameObject.");
        }
    }


    // LateUpdate is called once per frame after all other Update methods have been called
    void LateUpdate()
    {
        // Override the position and rotation to maintain the initial state
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
