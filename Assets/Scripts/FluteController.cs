using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteController : MonoBehaviour
{

    public int atLevel = 0;
    private float tilesToMove;
    private Transform cam;
    private Sprite[] levelSprites;
    public int facing;
    private Vector2[] levels = new Vector2[8];
    private Vector2Int[] levelCoordinates = new Vector2Int[8];
    public GameObject tornado;
    private GameObject[] players;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        levelSprites = Resources.LoadAll<Sprite>("Overworld Tiles");
        levels[0] = new Vector2(-1.28f, 1.0538f);
        levels[1] = new Vector2(11.52f, 1.0538f);
        levels[2] = new Vector2(-8.96f, -5.986f);
        levels[3] = new Vector2(-6.4f, -0.706f);
        levels[4] = new Vector2(8.96f, 6.334f);
        levels[5] = new Vector2(-14.08f, 2.814f);
        levels[6] = new Vector2(-14.08f, -0.706f);
        levels[7] = new Vector2(14.08f, -4.226f);
        levelCoordinates[0] = new Vector2Int(8, 4);
        levelCoordinates[1] = new Vector2Int(13, 4);
        levelCoordinates[2] = new Vector2Int(5, 8);
        levelCoordinates[3] = new Vector2Int(6, 5);
        levelCoordinates[4] = new Vector2Int(12, 1);
        levelCoordinates[5] = new Vector2Int(3, 3);
        levelCoordinates[6] = new Vector2Int(3, 5);
        levelCoordinates[7] = new Vector2Int(14, 7);
    }

    public void TransitionLevels(int dir)
    {
        facing = dir;
        GetComponent<SpriteRenderer>().enabled = true;
        transform.position = new Vector2(cam.transform.position.x + 2.56f, cam.transform.position.y - 0.174f);
        tilesToMove = 16;
        StartCoroutine(transitionRight());
        CheckNewLevel();
        StartCoroutine(wait());
        if (GoToLevel() == 1)
            GetComponent<SpriteRenderer>().sprite = levelSprites[0];
        if (GoToLevel() == 2)
            GetComponent<SpriteRenderer>().sprite = levelSprites[1];
        if (GoToLevel() == 3)
            GetComponent<SpriteRenderer>().sprite = levelSprites[2];
        if (GoToLevel() == 4)
            GetComponent<SpriteRenderer>().sprite = levelSprites[3];
        if (GoToLevel() == 5)
            GetComponent<SpriteRenderer>().sprite = levelSprites[4];
        if (GoToLevel() == 6)
            GetComponent<SpriteRenderer>().sprite = levelSprites[5];
        if (GoToLevel() == 7)
            GetComponent<SpriteRenderer>().sprite = levelSprites[6];
        if (GoToLevel() == 8)
            GetComponent<SpriteRenderer>().sprite = levelSprites[7];
    }

    private int GoToLevel()
    {
        return PersistentManager.Instance.levelsCompleted[atLevel - 1];
    }

    private void CheckNewLevel()
    {
        if (facing == 1 || facing == 4)
        {
            atLevel = Overflow();
        }
        if (facing == 2 || facing == 3)
        {
            atLevel = Underflow();
        }
    }

    int Underflow()
    {
        if (atLevel - 1 > 0)
            return atLevel - 1;
        else
            return PersistentManager.Instance.levelsCompleted.Count;
    }

    int Overflow()
    {
        if (atLevel + 1 <= PersistentManager.Instance.levelsCompleted.Count)
            return atLevel + 1;
        else
            return 1;
    }

    IEnumerator transitionRight()
    {
        while (tilesToMove != 0)
        {
            cam.Translate(0.01f, 0f, 0f);
            tilesToMove -= 0.0625f;
            if (tilesToMove != 0)
            {
                cam.Translate(0.01f, 0f, 0f);
                tilesToMove -= 0.0625f;
                if (tilesToMove != 0)
                {
                    cam.Translate(0.01f, 0f, 0f);
                    tilesToMove -= 0.0625f;
                    if (tilesToMove != 0)
                    {
                        cam.Translate(0.01f, 0f, 0f);
                        tilesToMove -= 0.0625f;
                    }
                }
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }
    }
    IEnumerator wait()
    {
        yield return StartCoroutine(WaitFor.Frames(65));
        GameObject tornadoClone;
        if(PersistentManager.Instance.levelsCompleted[atLevel - 1] - 1 == 7)
            tornadoClone = Instantiate(tornado, new Vector3(transform.position.x - 1.16f, transform.position.y + 0.5f, 0), transform.rotation);
        else
            tornadoClone = Instantiate(tornado, new Vector3(transform.position.x - 1.16f, transform.position.y - 0.4f, 0), transform.rotation);
        yield return StartCoroutine(WaitFor.Frames(58));
        cam.position = levels[PersistentManager.Instance.levelsCompleted[atLevel - 1] - 1];
        GetComponent<SpriteRenderer>().enabled = false;
        PersistentManager.Instance.playerLocation = levelCoordinates[PersistentManager.Instance.levelsCompleted[atLevel - 1] - 1];
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = true;
            players[i].GetComponent<SpriteRenderer>().enabled = true;
            players[i].GetComponent<BoxCollider2D>().enabled = true;
            if (PersistentManager.Instance.levelsCompleted[atLevel - 1] - 1 == 7)
                players[i].transform.position = new Vector2(cam.position.x - 0.05f, cam.position.y + 0.3f);
            else
                players[i].transform.position = new Vector2(cam.position.x - 0.05f, cam.position.y - 0.6f);
        }
        Destroy(tornadoClone);
    }
}
