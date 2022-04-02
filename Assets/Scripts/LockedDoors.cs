using UnityEngine;
using UnityEngine.Tilemaps;

public class LockedDoors : MonoBehaviour
{

    private Tilemap tilemap;

    // Use this for initialization
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && PersistentManager.Instance.keys > 0 || PersistentManager.Instance.hasSkeletonKey)
        {
            FindObjectOfType<SFXManager>().PlaySound("Door Unlock");
            Vector3Int tile = tilemap.WorldToCell(collision.transform.position) + new Vector3Int(collision.gameObject.GetComponent<PlayerController>().facingVectors.x, collision.gameObject.GetComponent<PlayerController>().facingVectors.y, 0);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (tilemap.HasTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y, 0)))
                    {
                        if (!PersistentManager.Instance.hasSkeletonKey)
                            PersistentManager.Instance.keys--;
                        tilemap.SetTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y, 0), null);
                        PersistentManager.Instance.doorsOpened.Add(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y, 0));
                    }
                    if (tilemap.HasTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x + collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y + collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0)))
                    {
                        if (!PersistentManager.Instance.hasSkeletonKey)
                            PersistentManager.Instance.keys--;
                        tilemap.SetTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x + collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y + collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0), null);
                        PersistentManager.Instance.doorsOpened.Add(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x + collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y + collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0));
                    }
                    if (tilemap.HasTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x - collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y - collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0)))
                    {
                        if (!PersistentManager.Instance.hasSkeletonKey)
                            PersistentManager.Instance.keys--;
                        tilemap.SetTile(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x - collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y - collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0), null);
                        PersistentManager.Instance.doorsOpened.Add(tile + new Vector3Int(x * collision.gameObject.GetComponent<PlayerController>().facingVectors.x - collision.gameObject.GetComponent<PlayerController>().facingVectors.y, y * collision.gameObject.GetComponent<PlayerController>().facingVectors.y - collision.gameObject.GetComponent<PlayerController>().facingVectors.x, 0));
                    }
                }
            }
        }
    }
}