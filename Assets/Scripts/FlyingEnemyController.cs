using System.Collections;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
    public float moveSpeed;
    public Vector2Int distanceToMove;
    public bool land;
    public float avoidWallsDistance;

    private float tilesToMove;
    private float timeBeforeLand;
    private int moveDirection;
    private int lastMove;
    private bool landing;
    private Rigidbody2D rb;
    private Animator anim;
    private LayerMask ignoreRaycast;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timeBeforeLand = Random.Range(6f, 12f);
        ignoreRaycast = LayerMask.GetMask("Screen Seperator");
    }

    // Update is called once per frame
    void Update()
    {
        if (land)
        {
            if(timeBeforeLand <= 0 && !landing)
            {
                landing = true;
                timeBeforeLand = Random.Range(4f, 8f);
                StartCoroutine(Land());
            }
            if(timeBeforeLand > 0 && !landing)
                timeBeforeLand -= Time.deltaTime;
        }
        if (tilesToMove > 0 && !landing)
        {
            tilesToMove -= Time.deltaTime * 2;
            //Up
            if (moveDirection == 1)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(0, 1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(0, 1), Color.blue);
                anim.SetFloat("AnimX", 0);
                anim.SetFloat("AnimY", 1);
                rb.velocity = new Vector3(0f, 1 * moveSpeed * Time.deltaTime, 0f);
            }
            //Up Right
            if (moveDirection == 2)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(1, 1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(1, 1), Color.blue);
                anim.SetFloat("AnimX", 1);
                anim.SetFloat("AnimY", 1);
                rb.velocity = new Vector3(0.7f * moveSpeed * Time.deltaTime, 0.7f * moveSpeed * Time.deltaTime, 0f);
            }
            //Right
            if (moveDirection == 3)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(1, 0), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(1, 0), Color.blue);
                anim.SetFloat("AnimX", 1);
                anim.SetFloat("AnimY", 0);
                rb.velocity = new Vector3(1 * moveSpeed * Time.deltaTime, 0f, 0f);
            }
            //Down Right
            if (moveDirection == 4)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(1, -1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(1, -1), Color.blue);
                anim.SetFloat("AnimX", 1);
                anim.SetFloat("AnimY", 0);
                rb.velocity = new Vector3(0.7f * moveSpeed * Time.deltaTime, -0.7f * moveSpeed * Time.deltaTime, 0f);
            }
            //Down
            if (moveDirection == 5)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(0, -1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(0, -1), Color.blue);
                anim.SetFloat("AnimX", 1);
                anim.SetFloat("AnimY", 0);
                rb.velocity = new Vector3(0f, -1f * moveSpeed * Time.deltaTime, 0f);
            }
            //Down Left
            if (moveDirection == 6)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(-1, -1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(-1, -1), Color.blue);
                anim.SetFloat("AnimX", 0);
                anim.SetFloat("AnimY", 0);
                rb.velocity = new Vector3(-0.7f * moveSpeed * Time.deltaTime, -0.7f * moveSpeed * Time.deltaTime, 0f);
            }
            //Left
            if (moveDirection == 7)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(-1, 0), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(-1, 0), Color.blue);
                anim.SetFloat("AnimX", 0);
                anim.SetFloat("AnimY", 0);
                rb.velocity = new Vector3(-1f * moveSpeed * Time.deltaTime, 0f, 0f);
            }
            //Up Left
            if (moveDirection == 8)
            {
                if (Physics2D.Raycast(transform.position, new Vector2(-1, 1), avoidWallsDistance, ignoreRaycast))
                {
                    moveDirection = Random.Range(1, 8);
                }
                Debug.DrawRay(transform.position, new Vector2(-1, 1), Color.blue);
                anim.SetFloat("AnimX", 0);
                anim.SetFloat("AnimY", 1);
                rb.velocity = new Vector3(-0.7f * moveSpeed * Time.deltaTime, 0.7f * moveSpeed * Time.deltaTime, 0f);
            }
        }
        //Choose New Movement Direction
        else if (!landing)
        {
            rb.velocity = Vector2.zero;
            moveDirection = Random.Range(1, 8);
            if (moveDirection != lastMove)
            {
                lastMove = moveDirection;
                tilesToMove = Random.Range(distanceToMove.x, distanceToMove.y);
            }
        }
    }

    IEnumerator Land()
    {
        //Slow down for land
        for (int i = 0; i < 40; i++)
        {
            yield return StartCoroutine(WaitFor.Frames(3));
            anim.speed *= 0.95f;
            rb.velocity *= 0.95f;
        }
        GetComponent<EnemyHealthManager>().invincibility = 0;
        anim.speed = 0f;
        rb.velocity = Vector2.zero;

        yield return StartCoroutine(WaitFor.Frames(220));
        anim.speed = 0.25f;
        yield return StartCoroutine(WaitFor.Frames(60));
        rb.velocity = new Vector2(0, 0.05f);
        GetComponent<EnemyHealthManager>().invincibility = 9999;

        //Speed upo for ascent
        for (int i = 0; i < 40; i++)
        {
            yield return StartCoroutine(WaitFor.Frames(3));
            anim.speed *= 1.035f;
            rb.velocity *= 1.035f;
        }
        anim.speed = 1;
        landing = false;
    }
}