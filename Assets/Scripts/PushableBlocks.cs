using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PushableBlocks : MonoBehaviour
{

    private Tilemap tilemap;
    public Tilemap closedDoors;
    public GameObject pushableBlock;
    public Tile testTile;
    public static GameObject block;
    private Vector3Int center;

    public GameObject tunnels;
    private Tilemap pushableBlocksOG;

    // Use this for initialization
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        pushableBlocksOG = Resources.Load<Tilemap>("Pushable Blocks") as Tilemap;
        //The tile in the center of the camera
        center = tilemap.WorldToCell(FindObjectOfType<Camera>().transform.position);
    }

    //Wtf is this code
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Gets rid of floating-point impercision in camera transform that sometimes causes center of room to be misplaced
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector2((float)Math.Round(GameObject.FindGameObjectWithTag("MainCamera").transform.position.x, 3), (float)Math.Round(GameObject.FindGameObjectWithTag("MainCamera").transform.position.y, 3));
            //Has to refresh Tilemap Closed Doors due to reloading on level transitions
            if (PersistentManager.Instance.inLevel != 0)
                closedDoors = GameObject.Find("Closed Doors").GetComponent<Tilemap>();
            //The tile in from of the player
            Vector3Int tile = tilemap.WorldToCell(collision.transform.position) + new Vector3Int(collision.gameObject.GetComponent<PlayerController>().facingVectors.x, collision.gameObject.GetComponent<PlayerController>().facingVectors.y, 0);
            //Fast travel points in overworld, only works with power bracelet
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Overworld") && tilemap.HasTile(tile) && PersistentManager.Instance.hasPowerBracelet && collision.gameObject.GetComponent<PlayerController>().facingVectors.x == 0 && tile != new Vector3Int(21, -4, 0))
            {
                FindObjectOfType<SFXManager>().PlaySound("Secret");
                StartCoroutine(MoveBlock(tile, collision));
                if (PersistentManager.Instance.playerLocation == new Vector2(10, 5) || PersistentManager.Instance.playerLocation == new Vector2(14, 2))
                {
                    if (!PersistentManager.Instance.coversCleared.Contains(tile + new Vector3Int(2, 2, 0)))
                    {
                        tilemap.SetTile(tile + new Vector3Int(2, 2, 0), null);
                        PersistentManager.Instance.coversCleared.Add(tile + new Vector3Int(2, 2, 0));
                    }
                }
                else
                {
                    if (!PersistentManager.Instance.coversCleared.Contains(tile + new Vector3Int(1, 0, 0)))
                    {
                        tilemap.SetTile(tile + new Vector3Int(1, 0, 0), null);
                        PersistentManager.Instance.coversCleared.Add(tile + new Vector3Int(1, 0, 0));
                    }
                }
            }
            //Pushable blocks in dungeons, for opening doors and hidden stairwells
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon") && tilemap.HasTile(tile))
            {
                //Only with tunnel in the center (don't allow horizontal pushing and don't bother revealing tunnels or opening doors)
                if (tilemap.WorldToCell(collision.transform.position) + new Vector3Int(2, 1, 0) == center)
                {
                    if (collision.gameObject.GetComponent<PlayerController>().facingVectors.x == 0)
                    {
                        StartCoroutine(MoveBlock(tile, collision));
                        FindObjectOfType<SFXManager>().PlaySound("Door Unlock");
                    }
                }
                //Open tunnels and doors
                else
                {
                    FindObjectOfType<SFXManager>().PlaySound("Door Unlock");
                    //Enables tunnel entrances
                    tunnels.SetActive(true);
                    //Moves Block
                    StartCoroutine(MoveBlock(tile, collision));
                    //Checks for doors to open and opens them
                    closedDoors.SetTile(new Vector3Int(center.x, center.y + 3, 0), testTile);
                    closedDoors.SetTile(new Vector3Int(center.x - 7, center.y - 1, 0), testTile);
                    closedDoors.SetTile(new Vector3Int(center.x + 9, center.y - 1, 0), testTile);
                    closedDoors.SetTile(new Vector3Int(center.x, center.y - 8, 0), testTile);
                }
            }
        }
    }

    IEnumerator MoveBlock(Vector3Int tile, Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().enabled = false;
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        block = Instantiate(pushableBlock);
        block.GetComponent<SpriteRenderer>().sprite = tilemap.GetSprite(tile);
        block.transform.position = tilemap.GetCellCenterWorld(tile);
        block.GetComponent<Rigidbody2D>().velocity = new Vector2((float)collision.gameObject.GetComponent<PlayerController>().facingVectors.x / 2, (float)collision.gameObject.GetComponent<PlayerController>().facingVectors.y / 2);
        tilemap.SetTile(tile, null);
        yield return StartCoroutine(WaitFor.Frames(10));
        collision.gameObject.GetComponent<PlayerController>().enabled = true;
        yield return StartCoroutine(WaitFor.Frames(9));
        block.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    //Resets the tilemaps for Closed Doors and Pushable blocks, closing the doors and moving the blocks to their original position
    public void ResetTiles()
    {

        //Destroy(GameObject.Find("Closed Doors"));
        Destroy(block);
        Tilemap temp = Instantiate(pushableBlocksOG);
        temp.transform.parent = GameObject.Find("Grid").transform;
        temp.name = "Pushable Blocks";
        temp.GetComponent<PushableBlocks>().tunnels = tunnels;
        /*
        Tilemap temp2 = Instantiate(closedDoorsOG);
        temp2.transform.parent = GameObject.Find("Grid").transform;
        temp2.name = "Closed Doors";
        */
        Destroy(gameObject);
    }
}