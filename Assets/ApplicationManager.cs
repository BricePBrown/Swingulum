using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        // Check for user input to reload the scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Check for number key presses to load corresponding scenes
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                // Load the scene corresponding to the number key pressed
                // Make sure the scene exists in your build settings
                if (i < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(i);
                }
            }
        }

        // Check for Escape key to close the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
