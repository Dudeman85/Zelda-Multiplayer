using UnityEngine;
using UnityEngine.Tilemaps;

public class LadderController : MonoBehaviour
{
    private Tilemap tilemap;
    private Vector3Int tile;
    public Tile ladder;
    public Tile water;
    
    void Update()
    {
        if (GetComponent<PlayerController>().ladderInUse)
            if (Mathf.Abs(transform.position.x - tilemap.GetCellCenterWorld(tile).x) > 0.16 || Mathf.Abs(transform.position.y - tilemap.GetCellCenterWorld(tile).y) > 0.16)
            {
                tilemap.SetTile(tile, water);
                GetComponent<PlayerController>().ladderInUse = false;
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Water" && !GetComponent<PlayerController>().ladderInUse && PersistentManager.Instance.hasLadder)
        {
            tilemap = GameObject.Find("Water").GetComponent<Tilemap>();
            ladder.colliderType = Tile.ColliderType.None;
            tile = tilemap.WorldToCell(gameObject.transform.position) + new Vector3Int(gameObject.GetComponent<PlayerController>().facingVectors.x, gameObject.GetComponent<PlayerController>().facingVectors.y, 0);
            if (tilemap.HasTile(tile))
            {
                GetComponent<PlayerController>().ladderInUse = true;
                tilemap.SetTile(tile, ladder);
            }
        }
    }
}
