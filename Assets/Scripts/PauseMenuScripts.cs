using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;

public class PauseMenuScripts : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject menu;
    [SerializeField] Image contour;

    private static float musicVolume = 1f;
    private static bool paused = false;

    private Color targetColor = new Color(0f, 0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
            {
                OpenPauseMenu();
            }
            else
            {
                ReturnToGame();
            }
        }

        if(Mathf.Abs((targetColor - contour.color).a) > 0.01)
        {
            contour.color += (targetColor - contour.color) * Time.deltaTime * 8.0f;
        }
        else
        {
            contour.color = targetColor;
        }
    }

    public void OpenPauseMenu()
    {
        paused = true;
        menu.SetActive(true);
        targetColor = new Color(0f, 0f, 0f, 0.8f);
    }

    public void ReturnToGame()
    {
        paused = false;
        menu.SetActive(false);
        targetColor = new Color(0f, 0f, 0f, 0.0f);
    }

    public void OpenLevelSelect()
    {
        if(FindObjectOfType<SceneTransitionManager>())
        {
            ReturnToGame();
            FindObjectOfType<SceneTransitionManager>().StartTransitionToScene("Level Select");
        }
    }

    public void OnMusicVolumeChange()
    {
        var foundAudioSources = FindObjectsOfType(typeof(AudioSource));
        Debug.Log(volumeSlider.value);
        AudioListener.volume = volumeSlider.value;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
