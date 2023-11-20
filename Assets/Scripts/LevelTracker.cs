using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour
{
    List<LevelButton> levels = new List<LevelButton>();

    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color unlockedColor = new Color(1f, 1f, 1f, 1f);

    public bool transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        var buttons = FindObjectsOfType<LevelButton>();
        foreach (LevelButton button in buttons)
        {
            levels.Add(button);
        }
        VisitLevel("Level 1");
        foreach (LevelButton button in levels)
        {
            button.Initialize();
        }
    }

    public static void VisitLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 1);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        FindObjectOfType<SceneTransitionManager>().StartTransitionToScene("Level Select");
    }

    public void LoadLevel(string levelName)
    {
        if(!transitioning)
        {
            FindObjectOfType<SceneTransitionManager>().StartTransitionToScene(levelName);
        }
        transitioning = true;
    }
}
