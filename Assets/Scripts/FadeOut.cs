using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{

    private GameObject[] fadeitems;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut()
    {
        yield return StartCoroutine(WaitFor.Frames(1));
        fadeitems = GameObject.FindGameObjectsWithTag("Fade Out");
        for (var i = 0; i < fadeitems.Length; i++)
        {
            if (fadeitems[i].GetComponent<Animator>())
                fadeitems[i].GetComponent<Animator>().enabled = false;
        }
        for (var i = 0; i < 120; i++)
        {
            for (var a = 0; a < fadeitems.Length; a++)
            {
                fadeitems[a].GetComponent<SpriteRenderer>().enabled = !fadeitems[a].GetComponent<SpriteRenderer>().enabled;
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }
        Destroy(gameObject);
    }
}