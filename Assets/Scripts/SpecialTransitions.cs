using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SpecialTransitions : MonoBehaviour
{

    public int transitionDirection;
    private Transform cam;
    private float tilesToMove;
    public Sprite screenSprite;
    public bool lastInChain;
    private Collider2D collision;
    private GameObject[] players;
    public bool ignoreCollision = false;
    public GameObject nextScreen;
    public Vector2 enabledIn;
    public Vector2 screenCoordinates;
    private bool collided = false;
    public int[] sequence;

    private void OnTriggerEnter2D(Collider2D other)
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        players = GameObject.FindGameObjectsWithTag("Player");
        if (enabledIn == PersistentManager.Instance.playerLocation && other.tag == "Player" && !collided)
        {
            collision = other;
            UpdateList();
            if (lastInChain && PersistentManager.Instance.directionsTraveled.SequenceEqual(sequence))
            {
                Array.Clear(PersistentManager.Instance.directionsTraveled, 0, PersistentManager.Instance.directionsTraveled.Length);
                ignoreCollision = true;
                FindObjectOfType<SFXManager>().PlaySound("Secret");
                StartCoroutine(wait());
            }
            if (!ignoreCollision)
            {
                nextScreen.GetComponent<SpriteRenderer>().sprite = screenSprite;
                nextScreen.GetComponent<SpriteRenderer>().enabled = true;
                PausePlayers();
                //up
                if (transitionDirection == 1)
                {
                    collided = true;
                    nextScreen.transform.position = new Vector2(cam.transform.position.x - 0.001f, cam.transform.position.y + 1.576f);
                    tilesToMove = 11;
                    StartCoroutine(transitionUp());
                }
                //left
                if (transitionDirection == 2)
                {
                    collided = true;
                    nextScreen.transform.position = new Vector2(cam.transform.position.x + -2.559f, cam.transform.position.y - 0.1838f);
                    tilesToMove = 16;
                    StartCoroutine(transitionLeft());
                }
                //down
                if (transitionDirection == 3)
                {
                    collided = true;
                    nextScreen.transform.position = new Vector2(cam.transform.position.x - 0.001f, cam.transform.position.y - 1.944f);
                    tilesToMove = 11;
                    StartCoroutine(transitionDown());
                }
                //right
                if (transitionDirection == 4)
                {
                    collided = true;
                    nextScreen.transform.position = new Vector2(cam.transform.position.x + 2.559f, cam.transform.position.y - 0.1838f);
                    tilesToMove = 16;
                    StartCoroutine(transitionRight());
                }
            }
        }
    }

    private void UpdateList()
    {
        for (var i = 0; i < 3; i++)
        {
            if (i < 4)
                PersistentManager.Instance.directionsTraveled[3 - i] = PersistentManager.Instance.directionsTraveled[2 - i];
        }
        PersistentManager.Instance.directionsTraveled[0] = transitionDirection;
    }

    private void PausePlayers()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
            players[i].GetComponent<PlayerController>().enabled = false;
        }
        GameObject.Find("UI").GetComponent<UIController>().enabled = false;
    }

    private void UnpausePlayers()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<BoxCollider2D>().enabled = true;
            players[i].GetComponent<Transform>().position = collision.transform.position;
            if (players[i].name == "Player 1" && !GameObject.Find("UI").GetComponent<UIController>().player1Paused)
                players[i].GetComponent<PlayerController>().enabled = true;
            if (players[i].name == "Player 2" && !GameObject.Find("UI").GetComponent<UIController>().player2Paused)
                players[i].GetComponent<PlayerController>().enabled = true;
            if (players[i].name == "Player 3" && !GameObject.Find("UI").GetComponent<UIController>().player3Paused)
                players[i].GetComponent<PlayerController>().enabled = true;
            if (players[i].name == "Player 4" && !GameObject.Find("UI").GetComponent<UIController>().player4Paused)
                players[i].GetComponent<PlayerController>().enabled = true;
        }
        GameObject.Find("UI").GetComponent<UIController>().enabled = true;
        collided = false;
        PersistentManager.Instance.fireUsedInRoom = false;
    }

    IEnumerator transitionUp()
    {
        while (tilesToMove != 0)
        {
            cam.Translate(0f, 0.01f, 0f);
            tilesToMove -= 0.0625f;
            if (tilesToMove != 0)
            {
                cam.Translate(0f, 0.01f, 0f);
                tilesToMove -= 0.0625f;
                if (tilesToMove != 0)
                {
                    cam.Translate(0f, 0.01f, 0f);
                    tilesToMove -= 0.0625f;
                    if (tilesToMove != 0)
                    {
                        cam.Translate(0f, 0.01f, 0f);
                        tilesToMove -= 0.0625f;
                    }
                }
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }
        cam.position = screenCoordinates;
        collision.GetComponent<Transform>().Translate(0f, -1.6f, 0f);
        UnpausePlayers();
    }

    IEnumerator transitionDown()
    {
        while (tilesToMove != 0)
        {
            cam.Translate(0f, -0.01f, 0f);
            tilesToMove -= 0.0625f;
            if (tilesToMove != 0)
            {
                cam.Translate(0f, -0.01f, 0f);
                tilesToMove -= 0.0625f;
                if (tilesToMove != 0)
                {
                    cam.Translate(0f, -0.01f, 0f);
                    tilesToMove -= 0.0625f;
                    if (tilesToMove != 0)
                    {
                        cam.Translate(0f, -0.01f, 0f);
                        tilesToMove -= 0.0625f;
                    }
                }
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }
        cam.position = screenCoordinates;
        collision.GetComponent<Transform>().Translate(0f, 1.6f, 0f);
        UnpausePlayers();
    }

    IEnumerator transitionLeft()
    {
        while (tilesToMove != 0)
        {
            cam.Translate(-0.01f, 0f, 0f);
            tilesToMove -= 0.0625f;
            if (tilesToMove != 0)
            {
                cam.Translate(-0.01f, 0f, 0f);
                tilesToMove -= 0.0625f;
                if (tilesToMove != 0)
                {
                    cam.Translate(-0.01f, 0f, 0f);
                    tilesToMove -= 0.0625f;
                    if (tilesToMove != 0)
                    {
                        cam.Translate(-0.01f, 0f, 0f);
                        tilesToMove -= 0.0625f;
                    }
                }
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }
        cam.position = screenCoordinates;
        collision.GetComponent<Transform>().Translate(2.38f, 0, 0f);
        UnpausePlayers();
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
        cam.position = screenCoordinates;
        collision.GetComponent<Transform>().Translate(-2.38f, 0, 0f);
        UnpausePlayers();
    }

    IEnumerator wait()
    {
        yield return WaitFor.Frames(300);
        ignoreCollision = false;
    }
}
