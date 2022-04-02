using System.Collections;
using UnityEngine;

public class JumpyEnemyController : MonoBehaviour
{
    public float speed;
    public Vector2Int minMaxTiles;
    public Vector2 jumpFrequency;
    public Sprite[] sprites;

    private float angle;
    private Vector2 jumpTo;
    private float timeBeforeJump;
    private float tilesToJump;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private bool jumping;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = sprites[0];

        timeBeforeJump = 99;
        StartCoroutine(Startup());
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeJump < 0 && !jumping)
            StartCoroutine(Jump());

       if(timeBeforeJump >= 0 && !jumping)
            timeBeforeJump -= Time.deltaTime;

        //Run whilst jumping
        if (jumping)
        {
            tilesToJump -= Time.deltaTime * 6 * (speed / 10);
            if (tilesToJump <= 0)
            {
                timeBeforeJump = Random.Range(jumpFrequency.x, jumpFrequency.y);
                jumping = false;
                rb.velocity = Vector2.zero;

                //Set next jump data
                angle = Random.Range(1, 360) * Mathf.Deg2Rad;
                jumpTo = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                while (Physics2D.Raycast(transform.position, jumpTo, 1f, 1024))
                {
                    Debug.DrawRay(transform.position, jumpTo, Color.blue, 5f, false);
                    angle = Random.Range(1, 360) * Mathf.Deg2Rad;
                    jumpTo = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                }
                tilesToJump = Random.Range(minMaxTiles.x, minMaxTiles.y);
                Debug.DrawRay(transform.position, jumpTo, Color.red, 5f, false);
                StartCoroutine(Idle());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile Wall")
        {
            angle = -Mathf.Atan2(jumpTo.y, jumpTo.x) * Mathf.Rad2Deg;
            angle *= Mathf.Deg2Rad;
            jumpTo = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            rb.velocity = jumpTo * (speed / 10);
            Debug.DrawRay(transform.position, jumpTo, Color.green, 5f, false);
        }
    }

    //Start stupid bcus timing
    IEnumerator Startup()
    {
        yield return StartCoroutine(WaitFor.Frames(35));
        timeBeforeJump = Random.Range(jumpFrequency.x, jumpFrequency.y);
        GetComponent<Animator>().enabled = false;
    }

    //Idle Animation
    IEnumerator Idle()
    {
        while (timeBeforeJump > 0)
        {
            sprite.sprite = sprites[0];
            yield return StartCoroutine(WaitFor.Frames(10));
            sprite.sprite = sprites[1];
            yield return StartCoroutine(WaitFor.Frames(10));
        }
    }

    //Jump Animation
    IEnumerator Jump()
    {
        sprite.sprite = sprites[0];
        yield return StartCoroutine(WaitFor.Frames(20));
        sprite.sprite = sprites[1];

        rb.velocity = jumpTo * (speed / 10);
        jumping = true;
    }
}
