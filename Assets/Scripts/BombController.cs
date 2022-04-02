using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class BombController : MonoBehaviour
{

    private Animator anim;
    private BoxCollider2D[] hitbox = new BoxCollider2D[2];
    private Tilemap tilemap;

    // Use this for initialization
    void Start()
    {
        tilemap = GameObject.Find("Bombable Walls").GetComponent<Tilemap>();
        anim = GetComponent<Animator>();
        hitbox = GetComponents<BoxCollider2D>();
        hitbox[0].enabled = false;
        hitbox[1].enabled = false;
        anim.SetBool("Explode", true);
        StartCoroutine(hitboxTimer());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bombable Walls")
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Overworld"))
                for (int y = -1; y < 3; y++)
                    for (int x = -1; x < 3; x++)
                    {
                        if (tilemap.HasTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0)))
                            PersistentManager.Instance.coversCleared.Add(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0));
                        tilemap.SetTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0), null);
                    }
            FindObjectOfType<SFXManager>().PlaySound("Secret");
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon"))
        {
            for (int y = -1; y < 5; y++)
                for (int x = -4; x < 2; x++)
                {
                    if (tilemap.HasTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0)))
                        PersistentManager.Instance.coversCleared.Add(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0));
                    tilemap.SetTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x, y, 0), null);
                }
            FindObjectOfType<SFXManager>().PlaySound("Secret");
        }
    }

    IEnumerator hitboxTimer()
    {
        yield return WaitFor.Frames(50);
        FindObjectOfType<SFXManager>().PlaySound("Bomb Explode");
        hitbox[0].enabled = true;
        yield return StartCoroutine(WaitFor.Frames(33));
        hitbox[1].enabled = true;
        yield return StartCoroutine(WaitFor.Frames(2));
        Destroy(gameObject);
    }
    IEnumerator bombWall()
    {
        yield return StartCoroutine(WaitFor.Frames(1));
    }
}
