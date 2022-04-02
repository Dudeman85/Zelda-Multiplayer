using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    private GameObject player1;
    private GameObject[] players;
    private Transform cam;
    public static List<Vector2> coast;
    private static GameObject map;
    private static GameObject posMarker;
    private static GameObject roomMarker;
    private static GameObject doorMarker;
    private Vector2 doorMarkerOffset;
    private bool transitioning;

    private GameObject tunnels;

    // Use this for initialization
    void Start()
    {
        map = GameObject.Find("Map Data");
        posMarker = GameObject.Find("Dungeon Position Marker");
        roomMarker = Resources.Load("Map Room") as GameObject;
        doorMarker = Resources.Load("Map Door") as GameObject;
        cam = gameObject.GetComponent<Transform>();
        player1 = GameObject.Find("Player 1");
        players = GameObject.FindGameObjectsWithTag("Player");
        tunnels = GameObject.Find("Basement Points");
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitioning && !PersistentManager.Instance.disableTransitions)
        {
            //Up
            if (cam.transform.position.y - player1.transform.position.y < -0.67f)
            {
                PersistentManager.Instance.playerLocation.y--;
                PersistentManager.Instance.localDungeonPos.y++;
                posMarker.transform.Translate(new Vector2(0, 0.04f));
                doorMarkerOffset = new Vector2(0, -0.02f);
                StartCoroutine(Transition(1));
            }
            //Right
            if (cam.transform.position.x - player1.transform.position.x < -1.195f)
            {
                PersistentManager.Instance.playerLocation.x++;
                PersistentManager.Instance.localDungeonPos.x++;
                posMarker.transform.Translate(new Vector2(0.08f, 0));
                doorMarkerOffset = new Vector2(-0.04f, 0);
                StartCoroutine(Transition(2));
            }
            //Down
            if (cam.transform.position.y - player1.transform.position.y > 0.95f)
            {
                PersistentManager.Instance.playerLocation.y++;
                PersistentManager.Instance.localDungeonPos.y--;
                posMarker.transform.Translate(new Vector2(0, -0.04f));
                doorMarkerOffset = new Vector2(0, 0.02f);
                StartCoroutine(Transition(3));
            }
            //Left
            if (cam.transform.position.x - player1.transform.position.x > 1.2f)
            {
                PersistentManager.Instance.playerLocation.x--;
                PersistentManager.Instance.localDungeonPos.x--;
                posMarker.transform.Translate(new Vector2(-0.08f, 0));
                doorMarkerOffset = new Vector2(0.04f, 0);
                StartCoroutine(Transition(4));
            }
        }
    }

    private void PausePlayer()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
            players[i].GetComponent<PlayerController>().enabled = false;
            players[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        GameObject.Find("UI").GetComponent<UIController>().enabled = false;
        Array.Clear(PersistentManager.Instance.directionsTraveled, 0, PersistentManager.Instance.directionsTraveled.Length);
        //Turn off basement entrances in specific rooms in the dungeon
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon") && "C4 F1 J1 K4 F9 I10 I12 N9".Contains(((char)(PersistentManager.Instance.playerLocation.x + 64)).ToString() + PersistentManager.Instance.playerLocation.y.ToString()))
            tunnels.SetActive(false);
        //Update map
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon"))
        {
            //Rooms
            if (!PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel].Contains(PersistentManager.Instance.localDungeonPos))
            {
                PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel].Add(PersistentManager.Instance.localDungeonPos);
                GameObject room = Instantiate(roomMarker);
                room.transform.parent = map.transform;
                room.transform.localPosition = Vector2.zero;
                room.transform.Translate(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f));
            }
            //Doors
            if (!PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel].Contains(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f) + doorMarkerOffset))
            {
                PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel].Add(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f) + doorMarkerOffset);
                GameObject door = Instantiate(doorMarker);
                door.transform.parent = map.transform;
                door.transform.localPosition = Vector2.zero;
                door.transform.Translate(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f) + doorMarkerOffset);
            }
        }
    }
    private void UnpausePlayer()
    {
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<BoxCollider2D>().enabled = true;
            players[i].GetComponent<Transform>().position = player1.transform.position;
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
        PersistentManager.Instance.fireUsedInRoom = false;

        //Dungeons
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon"))
        {
            //Turn Basement entrances back on
            if (!"C4 F1 J1 K4 F9 I10 I12 N9".Contains(((char)(PersistentManager.Instance.playerLocation.x + 64)).ToString() + PersistentManager.Instance.playerLocation.y.ToString()))
                tunnels.SetActive(true);
            FindObjectOfType<PushableBlocks>().ResetTiles();
        }
    }

    IEnumerator Transition(int dir)
    {
        transitioning = true;
        float tilesToMove = 0;
        Vector2 direction = new Vector2();

        PausePlayer();

        switch (dir)
        {
            case 1:
                tilesToMove = 11;
                direction = new Vector2(0f, 0.01f);
                break;
            case 2:
                tilesToMove = 16;
                direction = new Vector2(0.01f, 0f);
                break;
            case 3:
                tilesToMove = 11;
                direction = new Vector2(0f, -0.01f);
                break;
            case 4:
                tilesToMove = 16;
                direction = new Vector2(-0.01f, 0f);
                break;
            default:
                break;
        }

        while (tilesToMove > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (tilesToMove > 0)
                {
                    cam.Translate(direction);
                    tilesToMove -= 0.0625f;
                }
            }
            yield return StartCoroutine(WaitFor.Frames(1));
        }

        player1.transform.Translate(direction * 17);
        if (PersistentManager.Instance.inLevel != 0)
            player1.transform.Translate(direction * 24);

        transitioning = false;

        UnpausePlayer();
        cam.GetComponent<EnemySpawnManager>().Despawn();
        cam.GetComponent<EnemySpawnManager>().Spawn();
    }
}