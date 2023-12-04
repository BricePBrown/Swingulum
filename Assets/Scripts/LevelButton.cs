using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] string levelName;
    [SerializeField] Image lockImage;
    [SerializeField] TMP_Text coinsText;

    
    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color unlockedColor = new Color(1f, 1f, 1f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        if(PlayerPrefs.GetInt(levelName, 0) == 1)
        {
            GetComponent<Image>().color = unlockedColor;
            GetComponent<Button>().enabled = true;
            lockImage.enabled = false;
            coinsText.enabled = true;
            Debug.Log(levelName);
            if (PlayerPrefs.GetInt(levelName + "CoinsMax", -1) != -1)
            {
                coinsText.text = "" + PlayerPrefs.GetInt(levelName + "CoinsGet", 0) + "/" +  PlayerPrefs.GetInt(levelName + "CoinsMax", 0);
            }
        }
    }

    public void Play()
    {
        FindObjectOfType<LevelTracker>().LoadLevel(levelName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}