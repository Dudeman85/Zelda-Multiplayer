using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{

    public int volume;
    private static SFXManager instance;
    public AudioClip[] soundEffects;
    private List<Vector2> coast;

    void Start()
    {
        coast = new List<Vector2> { new Vector2(12, 8), new Vector2(13, 8), new Vector2(14, 8), new Vector2(15, 8), new Vector2(16, 8), new Vector2(16, 7), new Vector2(16, 6), new Vector2(16, 5), new Vector2(16, 4), new Vector2(15, 4), new Vector2(15, 3), new Vector2(14, 3), new Vector2(14, 2), new Vector2(15, 2) };
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
    public void PlaySound(string name, int repeat = 1)
    {
        StartCoroutine(PlaySoundEffect(name, repeat));
    }
    public IEnumerator PlaySoundEffect(string name, int repeat)
    {
        AudioClip sound = Array.Find(soundEffects, s => s.name == name);
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.volume = volume / 100f;
        source.clip = sound;
        source.loop = true;
        source.Play();
        yield return new WaitForSeconds(sound.length * repeat);
        Destroy(source);
    }
    public IEnumerator Coast(Vector2 location)
    {
        yield return new WaitForSeconds(1);
        if (coast.Contains(PersistentManager.Instance.playerLocation) && PersistentManager.Instance.inLevel == 0)
        {
            while (location == PersistentManager.Instance.playerLocation)
            {
                PlaySound("Shore");
                yield return new WaitForSeconds(Array.Find(soundEffects, s => s.name == "Shore").length + UnityEngine.Random.Range(0.5f, 2));
            }
        }
    }
}
