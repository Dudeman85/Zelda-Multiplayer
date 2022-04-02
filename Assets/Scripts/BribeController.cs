using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BribeController : MonoBehaviour
{

    public bool enough;
    public int cost;
    public Slider textCover1;
    public Slider textCover2;
    public int row1Letters;
    public int row2Letters;
    public int row1Start;
    public int row2Start;
    private GameObject[] clones;

    // Use this for initialization
    void Start()
    {
        textCover1 = GameObject.Find("Text Cover 1").GetComponent<Slider>();
        textCover2 = GameObject.Find("Text Cover 2").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PlayerController>().rupees - cost >= 0)
        {
            collision.GetComponent<PlayerController>().rupees -= cost;
            GameObject.Find("Prices").GetComponent<SpriteRenderer>().enabled = false;
            if (enough)
            {
                GameObject.Find("Bribe Success").GetComponent<SpriteRenderer>().enabled = true;
                textCover1.value = 182 - (row1Start * 8);
                textCover2.value = 182 - (row2Start * 8);
                StartCoroutine(text());
                destroyClones();
            }
            else
            {
                GameObject.Find("Bribe Fail").GetComponent<SpriteRenderer>().enabled = true;
                textCover1.value = 166;
                textCover2.value = 126;
                row1Letters = 17;
                row2Letters = 8;
                StartCoroutine(text());
                destroyClones();
            }
        }
    }
    IEnumerator text()
    {
        while (row1Letters > 0 || row2Letters > 0)
        {
            if (row1Letters > 0)
            {
                textCover1.value -= 8;
                row1Letters--;
            }
            else
            {
                textCover2.value -= 8;
                row2Letters--;
            }
            yield return StartCoroutine(WaitFor.Frames(7));
        }
        textCover1.value = 0;
        textCover2.value = 0;
    }
    void destroyClones()
    {
        clones = GameObject.FindGameObjectsWithTag("Bribe");
        for (int i = 0; i < clones.Length; i++){
            clones[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
