using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTracker : MonoBehaviour
{
    [SerializeField] List<LevelButton> levels;

    [System.Serializable]
    public struct LevelButton 
    {
        public GameObject button;
        public string sceneName;
        public Image lockSprite;
    }

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
        VisitLevel("Level 1");
        foreach (LevelButton button in levels)
        {
            if(PlayerPrefs.GetInt(button.sceneName, 0) == 1)
            {
                button.button.GetComponent<Image>().color = unlockedColor;
                button.button.GetComponent<Button>().enabled = true;
                button.lockSprite.enabled = false;
            }
        }
    }

    public static void VisitLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, 1);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteAll();
        foreach (LevelButton button in levels)
        {
            if(PlayerPrefs.GetInt(button.sceneName, 0) == 1)
            {
                button.button.GetComponent<Image>().color = lockedColor;
                button.button.GetComponent<Button>().enabled = false;
                button.lockSprite.enabled = true;
            }
        }
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
