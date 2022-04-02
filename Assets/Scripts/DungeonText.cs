using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DungeonText : MonoBehaviour
{

    private GameObject[] players;
    public int row1Start;
    public int row2Start;
    public Slider textCover1;
    public Slider textCover2;
    public int row1Letters;
    public int row2Letters;
    private int row1Letters2;
    private int row2Letters2;
    private GameObject cover;
    private bool collisions;
    private GameObject[] oldMen;
    public GameObject[] fires;

    // Use this for initialization
    void Start()
    {
        oldMen = GameObject.FindGameObjectsWithTag("Old Man");
        fires = GameObject.FindGameObjectsWithTag("Fire");
        textCover1 = GameObject.Find("Text Cover 1").GetComponent<Slider>();
        textCover2 = GameObject.Find("Text Cover 2").GetComponent<Slider>();
        players = GameObject.FindGameObjectsWithTag("Player");
        players = players.OrderBy(p => p.name).ToArray();
        cover = GameObject.Find("Text Covers");
        cover.SetActive(true);
        collisions = false;
        if (gameObject.name == "A11" && PersistentManager.Instance.A11Done)
        {
            GameObject.Find("Text Cover A11").GetComponent<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
        if (gameObject.name == "O15" && PersistentManager.Instance.O15Done)
        {
            GameObject.Find("Text Cover O15").GetComponent<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
        if (gameObject.name == "H2" && PersistentManager.Instance.H2Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
        {
            GameObject.Find("Text Cover H2").GetComponent<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
        if (gameObject.name == "A13" && PersistentManager.Instance.A13Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
        {
            GameObject.Find("Text Cover A13").GetComponent<SpriteRenderer>().enabled = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < fires.Length; i++)
            {
                fires[i].GetComponent<Animator>().SetBool("Start", false);
            }
            for (int i = 0; i < oldMen.Length; i++)
            {
                oldMen[i].GetComponent<Animator>().SetBool("Start", false);
            }
            cover.SetActive(true);
            if (gameObject.name == "A11")
                GameObject.Find("Text Cover A11").GetComponent<SpriteRenderer>().enabled = true;
            if (gameObject.name == "O15")
                GameObject.Find("Text Cover O15").GetComponent<SpriteRenderer>().enabled = true;
            if (gameObject.name == "H2")
                GameObject.Find("Text Cover H2").GetComponent<SpriteRenderer>().enabled = true;
            if (gameObject.name == "A13")
                GameObject.Find("Text Cover A13").GetComponent<SpriteRenderer>().enabled = true;
            collisions = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        row1Letters2 = row1Letters;
        row2Letters2 = row2Letters;
        if (collision.tag == "Player" && collisions == false)
        {
            for (int i = 0; i < fires.Length; i++)
            {
                fires[i].GetComponent<Animator>().SetBool("Start", true);
            }
            for (int i = 0; i < oldMen.Length; i++)
            {
                oldMen[i].GetComponent<Animator>().SetBool("Start", true);
            }
            if (gameObject.name == "A11")
                GameObject.Find("Text Cover A11").GetComponent<SpriteRenderer>().enabled = false;
            if (gameObject.name == "O15")
            {
                GameObject.Find("Text Cover O15").GetComponent<SpriteRenderer>().enabled = false;
                if (PersistentManager.Instance.levelsCompleted.Count == 8)
                {
                    PersistentManager.Instance.O15Done = true;
                    StartCoroutine(level9entry());
                }
            }
            if (gameObject.name == "H2")
                GameObject.Find("Text Cover H2").GetComponent<SpriteRenderer>().enabled = false;
            if (gameObject.name == "A13")
                GameObject.Find("Text Cover A13").GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(text());
        }
    }

    IEnumerator level9entry()
    {
        yield return StartCoroutine(WaitFor.Frames(1));
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 25f * Time.deltaTime);
            players[i].GetComponent<Animator>().SetFloat("MoveY", 1f);
            players[i].GetComponent<Animator>().SetBool("Moving", true);
        }
        yield return StartCoroutine(WaitFor.Frames(60));
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            players[i].GetComponent<Animator>().SetBool("Moving", false);
        }
        yield return StartCoroutine(WaitFor.Frames(281));
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<Animator>().SetBool("Moving", true);
        players[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 25f * Time.deltaTime);
        if (players.Length == 2)
        {
            players[1].GetComponent<Animator>().SetFloat("MoveY", 0f);
            players[1].GetComponent<Animator>().SetFloat("MoveX", 1f);
            players[1].GetComponent<Rigidbody2D>().velocity = new Vector2(25f * Time.deltaTime, 0f);
        }
        if (players.Length == 3)
        {
            players[2].GetComponent<Animator>().SetFloat("MoveY", 0f);
            players[2].GetComponent<Animator>().SetFloat("MoveX", -1f);
            players[2].GetComponent<Rigidbody2D>().velocity = new Vector2(-25f * Time.deltaTime, 0f);
        }
        yield return StartCoroutine(WaitFor.Frames(30));
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            players[i].GetComponent<Animator>().SetBool("Moving", false);
            players[i].GetComponent<Animator>().SetBool("LiftTriforce", true);
        }
        yield return StartCoroutine(WaitFor.Frames(120));
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<Animator>().SetBool("LiftTriforce", false);
        GameObject.Find("Text Cover O15").GetComponent<SpriteRenderer>().enabled = true;
        yield return StartCoroutine(WaitFor.Frames(30));
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<PlayerController>().enabled = true;
        Destroy(gameObject);
    }

    IEnumerator text()
    {
        collisions = true;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = false;
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            players[i].GetComponent<Animator>().SetBool("Moving", false);
        }
        cover.SetActive(false);
        textCover1.value = 182 - (row1Start * 8);
        textCover2.value = 182 - (row2Start * 8);
        while (row1Letters2 > 0 || row2Letters2 > 0)
        {
            if (row1Letters2 > 0)
            {
                textCover1.value -= 8;
                row1Letters2--;
            }
            else
            {
                textCover2.value -= 8;
                row2Letters2--;
            }
            FindObjectOfType<SFXManager>().PlaySound("Text Slow");
            yield return StartCoroutine(WaitFor.Frames(7));
        }
        if (gameObject.name != "O15" || PersistentManager.Instance.levelsCompleted.Count != 8)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerController>().enabled = true;
                players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            }
            PersistentManager.Instance.disableUI = false;
        }
        textCover1.value = 0;
        textCover2.value = 0;
    }
}