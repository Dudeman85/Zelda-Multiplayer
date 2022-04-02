using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FireController : MonoBehaviour
{

    private Tilemap tilemap;

    // Use this for initialization
    void Start()
    {
        tilemap = GameObject.Find("Burnable Walls").GetComponent<Tilemap>();
        FindObjectOfType<SFXManager>().PlaySound("Candle");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Burnable Walls")
            StartCoroutine(burnWall());
    }
    IEnumerator burnWall()
    {
        yield return StartCoroutine(WaitFor.Frames(30));
        FindObjectOfType<SFXManager>().PlaySound("Secret");
        for (int y = 0; y < 3; y++)
            for (int x = 0; x < 3; x++)
            {
                if (tilemap.HasTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x - 1, y - 1, 0)))
                    PersistentManager.Instance.coversCleared.Add(tilemap.WorldToCell(transform.position) - new Vector3Int(x - 1, y - 1, 0));
                tilemap.SetTile(tilemap.WorldToCell(transform.position) - new Vector3Int(x - 1, y - 1, 0), null);
            }
    }
}
