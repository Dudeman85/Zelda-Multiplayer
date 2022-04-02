using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadArea : MonoBehaviour
{
    public bool pausePlayer;
    public string levelToLoad;
    public string spawnToLoad;
    public bool playAnimation;
    public bool moveCamera;
    public Vector2Int playerLocation;
    public Vector2 camPosition;
    private GameObject[] players;
    private GameObject player;
    public static GameObject map;
    public static GameObject overworldMap;
    private static GameObject posMarker;
    public static GameObject roomMarker;
    public static GameObject doorMarker;
    private static SpriteRenderer overworldPointer;
    private static SpriteRenderer dungeonPointer;
    private static GameObject dungeonCompassPointer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        map = GameObject.Find("Map Data");
        posMarker = GameObject.Find("Dungeon Position Marker");
        overworldMap = GameObject.Find("Menu Map");
        overworldPointer = GameObject.Find("Position Marker").GetComponent<SpriteRenderer>();
        dungeonPointer = GameObject.Find("Dungeon Position Marker").GetComponent<SpriteRenderer>();
        dungeonCompassPointer = GameObject.Find("Dungeon Compass Marker");
        roomMarker = Resources.Load("Map Room") as GameObject;
        doorMarker = Resources.Load("Map Door") as GameObject;
        players = GameObject.FindGameObjectsWithTag("Player");

        if (other.tag == "Player" && other.GetComponent<PlayerController>().levelTransitionsEnabled)
        {
            FindObjectOfType<MusicManager>().ChangeSong("none");
            if (playAnimation)
            {
                FindObjectOfType<SFXManager>().PlaySound("Stairs");
                player = other.gameObject;
                //Disable all other players
                for (int i = 0; i < players.Length; i++)
                    if (players[i] != other.gameObject)
                        players[i].SetActive(false);
                //Set variables for animation
                player.transform.position = transform.position;
                player.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                player.GetComponent<PlayerController>().enabled = false;
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0.16f);
                player.GetComponent<Animator>().SetBool("Moving", true);
                player.GetComponent<Animator>().SetFloat("MoveY", 1);
                //Set other Variables
                PersistentManager.Instance.spawnpoint = spawnToLoad;
                //Start Wait
                StartCoroutine(wait());
            }
            else
            {
                if (levelToLoad == "Overworld")
                {
                    //Set up spawn manager
                    Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
                    EnemySpawnManager spawner = FindObjectOfType<EnemySpawnManager>();

                    for (int x = 0; x < spawner.overworld.Length; x++)
                    {
                        for (int y = 0; y < spawner.overworld[x].y.Length; y++)
                        {
                            spawner.currentEnemies[x].y[y].enemies = new GameObject[spawner.overworld[x].y[y].enemies.Length];
                            for (int i = 0; i < spawner.overworld[x].y[y].enemies.Length; i++)
                            {
                                spawner.currentEnemies[x].y[y].enemies[i] = spawner.overworld[x].y[y].enemies[i].gameObject;
                            }
                        }
                    }

                    //Setup Map for Overworld
                    overworldMap.GetComponent<SpriteRenderer>().enabled = true;
                    map.GetComponent<SpriteRenderer>().enabled = false;
                    overworldPointer.enabled = true;
                    dungeonPointer.enabled = false;
                    dungeonCompassPointer.GetComponent<SpriteRenderer>().enabled = false;
                    GameObject.Find("UI Compass").GetComponent<SpriteRenderer>().enabled = false;
                    GameObject.Find("UI Map").GetComponent<SpriteRenderer>().enabled = false;
                    PersistentManager.Instance.inLevel = 0;
                    //Delete Dungeon Map Items
                    for (int i = 0; i < map.transform.childCount; i++)
                        Destroy(map.transform.GetChild(i).gameObject);
                }
                if (levelToLoad == "Dungeon")
                {
                    //Set up spawn manager
                    Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
                    EnemySpawnManager spawner = FindObjectOfType<EnemySpawnManager>();

                    for (int x = 0; x < spawner.dungeon.Length; x++)
                    {
                        for (int y = 0; y < spawner.dungeon[x].y.Length; y++)
                        {
                            spawner.currentEnemies[x].y[y].enemies = new GameObject[spawner.dungeon[x].y[y].enemies.Length];
                            for (int i = 0; i < spawner.dungeon[x].y[y].enemies.Length; i++)
                            {
                                spawner.currentEnemies[x].y[y].enemies[i] = spawner.dungeon[x].y[y].enemies[i].gameObject;
                            }
                        }
                    }
                    SetupMap();
                }
                PersistentManager.Instance.spawnpoint = spawnToLoad;
                SceneManager.LoadScene(levelToLoad);
                GameObject.Find("Black Screen").GetComponent<SpriteRenderer>().enabled = true;
                if (moveCamera)
                {
                    GameObject.FindGameObjectWithTag("MainCamera").transform.position = camPosition;
                    PersistentManager.Instance.playerLocation = playerLocation;
                }
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelToLoad);
        GameObject.Find("Black Screen").GetComponent<SpriteRenderer>().enabled = true;
        if (levelToLoad == "Dungeon")
        {
            SetupMap();
        }
        if (moveCamera)
        {
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = camPosition;
            PersistentManager.Instance.playerLocation = playerLocation;
        }
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<SpriteRenderer>().sortingLayerName = "Players";
        for (int i = 0; i < players.Length; i++)
            players[i].SetActive(true);
    }

    //Setup map for dungeon
    void SetupMap()
    {
        PersistentManager.Instance.inLevel = int.Parse(spawnToLoad[3].ToString());
        PersistentManager.Instance.localDungeonPos = Vector2Int.zero;
        posMarker.transform.localPosition = new Vector2(PersistentManager.Instance.dungeonMapOffsets[PersistentManager.Instance.inLevel] * 0.075f, -0.1537f);
        map.transform.localPosition = new Vector2(PersistentManager.Instance.dungeonMapOffsets[PersistentManager.Instance.inLevel] * 0.075f, -0.1537f);
        overworldMap.GetComponent<SpriteRenderer>().enabled = false;
        map.GetComponent<SpriteRenderer>().enabled = true;
        overworldPointer.enabled = false;
        dungeonPointer.enabled = true;
        if (PersistentManager.Instance.dungeonMaps.Contains(PersistentManager.Instance.inLevel))
            GameObject.Find("UI Map").GetComponent<SpriteRenderer>().enabled = true;
        if (PersistentManager.Instance.dungeonCompasses.Contains(PersistentManager.Instance.inLevel))
        {
            GameObject.Find("UI Compass").GetComponent<SpriteRenderer>().enabled = true;
            dungeonCompassPointer.GetComponent<SpriteRenderer>().enabled = true;
        }
        dungeonCompassPointer.transform.position = dungeonPointer.transform.position;
        dungeonCompassPointer.transform.Translate(new Vector2(PersistentManager.Instance.dungeonCompassPositions[PersistentManager.Instance.inLevel].x * 0.08f, PersistentManager.Instance.dungeonCompassPositions[PersistentManager.Instance.inLevel].y * 0.04f));
        //Create Dungeon Map
        foreach (Vector2 i in PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel])
        {
            GameObject room = Instantiate(roomMarker);
            room.transform.parent = map.transform;
            room.transform.localPosition = Vector2.zero;
            room.transform.localScale = Vector3.one;
            room.transform.Translate(new Vector2(i.x * 0.08f, i.y * 0.04f));
        }
        foreach (Vector2 i in PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel])
        {
            GameObject door = Instantiate(doorMarker);
            door.transform.parent = map.transform;
            door.transform.localPosition = Vector2.zero;
            door.transform.localScale = Vector3.one;
            door.transform.Translate(new Vector2(i.x, i.y));
        }
    }
}