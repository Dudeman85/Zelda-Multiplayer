using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 shotFrequency;
    public GameObject projectile;

    protected Rigidbody2D rb;
    private Animator anim;
    private float tilesToMove;
    private float timeBeforeShot;
    private int moveDirection;
    private int lastMove;
    private Vector3 lastPosition;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        timeBeforeShot = Random.Range(1f, 2f);
        moveDirection = Random.Range(1, 5);
        tilesToMove = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        //Normal Movement
        if (timeBeforeShot >= 0f || !projectile)
        {
            timeBeforeShot -= Time.deltaTime;
            //Check if there was no movement last frame (stuck in wall: repick direction)
            if (lastPosition == transform.position)
                tilesToMove = 0;
            //Move if needed
            if (tilesToMove > 0)
                Move();
            //Refresh tiles to move and direction
            else
            {
                rb.velocity = Vector2.zero;
                moveDirection = Random.Range(1, 5);
                while (moveDirection == lastMove)
                    moveDirection = Random.Range(1, 5);
                lastMove = moveDirection;
                tilesToMove = Random.Range(1, 5);
                Move();
            }
        }
        //Shoot projectile if applicable
        else if (timeBeforeShot != -420)
        {
            timeBeforeShot = -420;
            rb.velocity = Vector2.zero;
            StartCoroutine(ProjectileWait());
        }
    }

    //Applies velocity to rigidbody based on direction, moving the enemy (1 = ↑, 2 = ↓, 3 = →, 4 = ←)
    public void Move()
    {
        tilesToMove -= Time.deltaTime * 0.125f * moveSpeed;
        StartCoroutine(LastPos(transform.position));
        //Up
        if (moveDirection == 1)
        {
            rb.velocity = new Vector3(0f, 1 * moveSpeed * Time.deltaTime, 0f);
            anim.SetFloat("AnimX", 0);
            anim.SetFloat("AnimY", 1);
        }
        //Down
        if (moveDirection == 2)
        {
            rb.velocity = new Vector3(0f, -1 * moveSpeed * Time.deltaTime, 0f);
            anim.SetFloat("AnimX", 0);
            anim.SetFloat("AnimY", -1);
        }
        //Right
        if (moveDirection == 3)
        {
            rb.velocity = new Vector3(1 * moveSpeed * Time.deltaTime, 0f, 0f);
            anim.SetFloat("AnimY", 0);
            anim.SetFloat("AnimX", 1);
        }
        //Left
        if (moveDirection == 4)
        {
            rb.velocity = new Vector3(-1 * moveSpeed * Time.deltaTime, 0f, 0f);
            anim.SetFloat("AnimY", 0);
            anim.SetFloat("AnimX", -1);
        }
    }

    private void OnColisionEnter2D(Collision2D collision)
    {
        if (moveDirection == 1)
            transform.Translate(0, -0.01f * moveSpeed * Time.deltaTime, 0);
        if (moveDirection == 2)
            transform.Translate(0, 0.01f * moveSpeed * Time.deltaTime, 0);
        if (moveDirection == 3)
            transform.Translate(0.01f * moveSpeed * Time.deltaTime, 0, 0);
        if (moveDirection == 4)
            transform.Translate(-0.01f * moveSpeed * Time.deltaTime, 0, 0);
        tilesToMove = 0;
    }

    IEnumerator LastPos(Vector3 pos)
    {
        yield return StartCoroutine(WaitFor.Frames(1));
        lastPosition = pos;
    }

    IEnumerator ProjectileWait()
    {
        yield return StartCoroutine(WaitFor.Frames(20));
        if (projectile)
        {
            GameObject p;
            //Up
            if (moveDirection == 1)
            {
                p = Instantiate(projectile, transform.position, transform.rotation);
                p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2);
                if (projectile.name.Contains("(rotate)"))
                    p.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            //Down
            if (moveDirection == 2)
            {
                p = Instantiate(projectile, transform.position, transform.rotation);
                p.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -2);
                if (projectile.name.Contains("(rotate)"))
                    p.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //Right
            if (moveDirection == 3)
            {
                p = Instantiate(projectile, transform.position, transform.rotation);
                p.GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
                if (projectile.name.Contains("(rotate)"))
                    p.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            //Left
            if (moveDirection == 4)
            {
                p = Instantiate(projectile, transform.position, transform.rotation);
                p.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
                if (projectile.name.Contains("(rotate)"))
                    p.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }
        yield return StartCoroutine(WaitFor.Frames(20));
        timeBeforeShot = Random.Range(shotFrequency.x, shotFrequency.y);
    }
}