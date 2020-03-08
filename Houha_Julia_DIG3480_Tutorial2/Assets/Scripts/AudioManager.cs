using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;

    public AudioClip musicClip01;
    public AudioClip musicClip02;
    
    // Start is called before the first frame update
    void Start()
    {
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            musicSource.clip = musicClip01;
            musicSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            musicSource.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            musicSource.clip = musicClip02;
            musicSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            musicSource.Stop();
        }
    }
}
