using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCrossLevel : MonoBehaviour
{

    AudioSource music;
    // Update is called once per frame
    private void Start()
    {
        music = GetComponent<AudioSource>();
    }
    void Update()
    {
        DontDestroyOnLoad(music);
    }
}
