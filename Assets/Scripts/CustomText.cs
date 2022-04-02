using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomText : MonoBehaviour
{

    public int line1Letters;
    public int line2Letters;
    public Transform transform2;
    private GameObject[] players;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        transform2 = GetComponentsInChildren<Transform>()[1];
        if (PersistentManager.Instance.pausePlayer)
        {
            PersistentManager.Instance.disableUI = true;
            StartCoroutine(initialize());
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerController>().enabled = false;
                players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            }
        }
    }

    IEnumerator initialize()
    {
        yield return StartCoroutine(WaitFor.Frames(28));
        StartCoroutine(move());
    }
    IEnumerator move()
    {
        while (line1Letters != 0 || line2Letters != 0)
        {
            yield return StartCoroutine(WaitFor.Frames(7));
            if (line1Letters != 0)
            {
                transform.Translate(0.08f, 0f, 0f);
                transform2.Translate(-0.08f, 0f, 0f);
                line1Letters--;
            }
            else if (line2Letters != 0)
            {
                transform2.Translate(0.08f, 0f, 0f);
                line2Letters--;
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = true;
        }
        PersistentManager.Instance.disableUI = false;
    }
}
