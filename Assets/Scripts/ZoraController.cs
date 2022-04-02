using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoraController : MonoBehaviour
{

    public GameObject projectile;
    public Sprite[] sprites;
    private GameObject projectileClone;
    private SpriteRenderer sprite;
    private GameObject[] players;
    private Transform target;
    private BoxCollider2D hitbox;
    private Tilemap tilemap;
    private Transform cam;

    // Use this for initialization
    void Start()
    {
        tilemap = GameObject.Find("Special Spawns").GetComponent<Tilemap>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        hitbox = GetComponent<BoxCollider2D>();
        hitbox.enabled = false;
        players = GameObject.FindGameObjectsWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(Zora());
    }

    IEnumerator Zora()
    {
        while (true)
        {
            //Surfacing Animation
            sprite.sprite = sprites[0];
            yield return StartCoroutine(WaitFor.Frames(10));
            sprite.sprite = sprites[1];
            yield return StartCoroutine(WaitFor.Frames(10));
            sprite.sprite = sprites[0];
            yield return StartCoroutine(WaitFor.Frames(10));
            sprite.sprite = sprites[1];
            yield return StartCoroutine(WaitFor.Frames(10));
            sprite.sprite = sprites[0];
            yield return StartCoroutine(WaitFor.Frames(10));

            //Surface Sprite and other things
            hitbox.enabled = true;
            target = players[Random.Range(0, players.Length)].transform;
            int x = 0;
            while (target.GetComponent<PlayerController>().dead == true)
            {
                target = players[Random.Range(0, players.Length)].transform;
                x++;
                if (x > 40)
                {
                    Destroy(gameObject);
                    break;
                }
            }
            if (target.position.y > transform.position.y)
                sprite.sprite = sprites[2];
            else
                sprite.sprite = sprites[3];

            //Projectile
            projectileClone = Instantiate(projectile, transform.position, transform.rotation);
            projectileClone.transform.parent = transform;
            yield return StartCoroutine(WaitFor.Frames(20));

            //Do some math to lauch the projectile
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            projectileClone.GetComponent<Rigidbody2D>().velocity = direction;

            //Submerge
            yield return StartCoroutine(WaitFor.Frames(60));
            hitbox.enabled = false;
            for (int i = 0; i < 4; i++)
            {
                sprite.sprite = sprites[0];
                yield return StartCoroutine(WaitFor.Frames(10));
                sprite.sprite = sprites[1];
                yield return StartCoroutine(WaitFor.Frames(10));
            }

            Vector3 position = new Vector2(cam.position.x + (Random.Range(-7, 7) * 0.16f), cam.position.y + (Random.Range(-4, 4) * 0.16f));
            int a = 0;
            while (!tilemap.HasTile(tilemap.WorldToCell(position)))
            {
                position = new Vector2(cam.position.x + (Random.Range(-6, 6) * 0.16f), cam.position.y + (Random.Range(-3, 3) * 0.16f));
                a++;
                if (a > 100)
                    break;
            }
            if (a < 100)
                transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(position));

            sprite.sprite = null;

            yield return StartCoroutine(WaitFor.Frames(200));
        }
    }
}
