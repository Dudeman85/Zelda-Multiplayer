using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{

    private GameObject[] players;
    public bool fastTravel;
    public Vector2 cameraCoords;
    public Vector2Int location;
    public bool playAnimation;
    public bool showText;
    public int row1Start;
    public int row2Start;
    public Slider textCover1;
    public Slider textCover2;
    public int row1Letters;
    public int row2Letters;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject.Find("Black Screen").GetComponent<SpriteRenderer>().enabled = false;
        textCover1 = GameObject.Find("Text Cover 1").GetComponent<Slider>();
        textCover2 = GameObject.Find("Text Cover 2").GetComponent<Slider>();
        //Check if this is the proper spawn location
        if (PersistentManager.Instance.spawnpoint == gameObject.name)
        {
            textCover1.value = 0;
            textCover2.value = 0;
            //Move Camera on fast travel
            if (fastTravel)
            {
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = cameraCoords;
                PersistentManager.Instance.playerLocation = location;
            }
            //On trasition to overworld
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Overworld"))
            {
                GameObject.Find("NPC Cover").GetComponent<SpriteRenderer>().enabled = false;
                for (var i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PlayerController>().levelTransitionsEnabled = false;
                }
                if (!playAnimation)
                    FindObjectOfType<MusicManager>().ChangeSong("Overworld");
            }
            //On transition to Grotto
            else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Grotto"))
            {
                GameObject.Find("Prices").GetComponent<SpriteRenderer>().enabled = true;
            }
            //On transition to Dungeon
            else
            {
                FindObjectOfType<MusicManager>().ChangeSong("Dungeon");
                if (PersistentManager.Instance.spawnpoint == "LVL9")
                    FindObjectOfType<MusicManager>().ChangeSong("Level 9");
            }
            //Check if text is displayed and display it
            if (showText && !PersistentManager.Instance.grottosCompleted.Contains(name))
            {
                if ("D4E1E7H3I8L5N1".Contains(name) && !PersistentManager.Instance.letterDelivered)
                {
                    textCover1.value = 182;
                    textCover2.value = 182;
                }
                else
                {
                    StartCoroutine(text());
                }
            }
            //Cover NPC and text
            else if (showText && PersistentManager.Instance.grottosCompleted.Contains(name))
                GameObject.Find("NPC Cover").GetComponent<SpriteRenderer>().enabled = true;
            //Exit door with animation
            if (playAnimation)
            {
                FindObjectOfType<SFXManager>().PlaySound("Stairs");
                for (var i = 0; i < players.Length; i++)
                    players[i].SetActive(false);
                players[0].SetActive(true);
                players[0].GetComponent<BoxCollider2D>().enabled = false;
                players[0].transform.position = transform.position + new Vector3(0, -0.16f, 0);
                players[0].GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                players[0].GetComponent<PlayerController>().enabled = false;
                players[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.16f);
                players[0].GetComponent<Animator>().SetBool("Moving", true);
                players[0].GetComponent<Animator>().SetFloat("MoveY", -1);
                StartCoroutine(wait());
            }
            //Enable players without animation
            else
            {
                for (var i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<Animator>().SetBool("Moving", false);
                    players[i].transform.position = transform.position + new Vector3(0, -i / 1000f, 0);
                    players[i].GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }

    //Reveal text
    public IEnumerator text()
    {
        FindObjectOfType<SFXManager>().PlaySound("Text", row1Letters + row2Letters - 1);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = false;
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        PersistentManager.Instance.disableUI = true;
        textCover1.value = 182 - (row1Start * 8);
        textCover2.value = 182 - (row2Start * 8);
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
            yield return StartCoroutine(WaitFor.Frames(6));
        }
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<PlayerController>().enabled = true;
        PersistentManager.Instance.disableUI = false;
        textCover1.value = 0;
        textCover2.value = 0;
    }
    //Timing for animations
    IEnumerator wait()
    {
        yield return StartCoroutine(WaitFor.Frames(45));
        FindObjectOfType<MusicManager>().ChangeSong("Overworld");
        players[0].GetComponent<BoxCollider2D>().enabled = true;
        players[0].GetComponent<PlayerController>().enabled = true;
        players[0].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        players[0].GetComponent<SpriteRenderer>().sortingLayerName = "Players";
        for (var i = 0; i < players.Length; i++)
        {
            if (players[i])
            {
                players[i].transform.position = transform.position + new Vector3(0, i / 1000f, 0);
                players[i].SetActive(true);
                players[i].GetComponent<Animator>().SetFloat("LastMoveY", -1);
                players[i].GetComponent<Animator>().SetFloat("MoveY", -1);
            }
        }
    }
}