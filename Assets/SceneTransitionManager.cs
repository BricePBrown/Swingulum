using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance = null;
    
    public RectTransform transitionPanel; // Reference to the UI panel's RectTransform
    //public float transitionSpeed = 1.5f; // Speed of the transition

    public void StartTransition()
    {
        StartCoroutine(Transition());
    }

    public void StartTransitionToScene(string input)
    {
        StartCoroutine(TransitionToScene(input));
    }

    void Start()
    {
        // Make this GameObject and all its children persistent
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);

        // If you only want to make the panel persistent, you can use:
        // DontDestroyOnLoad(transitionPanel.gameObject);
    }

    private float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4f * Mathf.Pow(t, 3) : 1f - Mathf.Pow(-2f * t + 2f, 3) / 2f;
    }

    private IEnumerator Transition()
    {
        Debug.Log("Transitioning");

        float animationDuration = 1f; // Duration of the animation in seconds
        float delay = .5f; // Delay in seconds
        float targetWidth = 1920; // Get the screen width at runtime

        // Set initial position and pivot for the start of the transition
        transitionPanel.anchoredPosition = new Vector2(0, transitionPanel.anchoredPosition.y);
        transitionPanel.pivot = new Vector2(0, transitionPanel.pivot.y);

        // Delay before the animation starts
        yield return new WaitForSeconds(delay);

        // Animate panel to cover the screen
        float panelWidth = 0;
        float elapsed = 0; // Time elapsed since the start of the animation

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOutCubic(elapsed / animationDuration);
            panelWidth = Mathf.Lerp(0, targetWidth, t);
            transitionPanel.sizeDelta = new Vector2(panelWidth, 1080);
            yield return null;
        }

        // ... (rest of your code remains the same)

        // Load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);

        // Wait for a frame to ensure the new scene is loaded
        yield return null;

        // Set initial position and pivot for the end of the transition
        transitionPanel.anchoredPosition = new Vector2(targetWidth, transitionPanel.anchoredPosition.y);
        transitionPanel.pivot = new Vector2(1, transitionPanel.pivot.y);

        // Reset elapsed time
        elapsed = 0;

        // Animate panel to reveal the new scene
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOutCubic(elapsed / animationDuration);
            panelWidth = Mathf.Lerp(targetWidth, 0, t);
            transitionPanel.sizeDelta = new Vector2(panelWidth, 1080);
            yield return null;
        }

        // Delay after the animation ends (if needed)
        // yield return new WaitForSeconds(delay);
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        Debug.Log("Transitioning");

        float animationDuration = 1f; // Duration of the animation in seconds
        float delay = .5f; // Delay in seconds
        float targetWidth = 1920; // Get the screen width at runtime

        // Set initial position and pivot for the start of the transition
        transitionPanel.anchoredPosition = new Vector2(0, transitionPanel.anchoredPosition.y);
        transitionPanel.pivot = new Vector2(0, transitionPanel.pivot.y);

        // Delay before the animation starts
        yield return new WaitForSeconds(delay);

        // Animate panel to cover the screen
        float panelWidth = 0;
        float elapsed = 0; // Time elapsed since the start of the animation

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOutCubic(elapsed / animationDuration);
            panelWidth = Mathf.Lerp(0, targetWidth, t);
            transitionPanel.sizeDelta = new Vector2(panelWidth, 1080);
            yield return null;
        }

        // ... (rest of your code remains the same)

        // Load the next scene
        SceneManager.LoadScene(sceneName);

        // Wait for a frame to ensure the new scene is loaded
        yield return null;

        // Set initial position and pivot for the end of the transition
        transitionPanel.anchoredPosition = new Vector2(targetWidth, transitionPanel.anchoredPosition.y);
        transitionPanel.pivot = new Vector2(1, transitionPanel.pivot.y);

        // Reset elapsed time
        elapsed = 0;

        // Animate panel to reveal the new scene
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = EaseInOutCubic(elapsed / animationDuration);
            panelWidth = Mathf.Lerp(targetWidth, 0, t);
            transitionPanel.sizeDelta = new Vector2(panelWidth, 1080);
            yield return null;
        }

        // Delay after the animation ends (if needed)
        // yield return new WaitForSeconds(delay);
    }
}

