using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCrossLevel : MonoBehaviour
{

    AudioSource music;
    private static MusicCrossLevel instance = null;
    // Update is called once per frame
    private void Start()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        
        music = GetComponent<AudioSource>();
    }
    void Update()
    {
        DontDestroyOnLoad(music);
    }
}
