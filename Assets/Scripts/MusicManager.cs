using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private static MusicManager instance;
    public AudioClip[] music;
    public AudioSource player;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeSong(string name)
    {
        player.clip = Array.Find(music, sound => sound.name == name);
        player.Play();
    }
}
