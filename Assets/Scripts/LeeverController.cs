using System.Collections;
using UnityEngine;

public class LeeverController : MonoBehaviour
{

    private float timeBeforeDive;
    public Vector2 diveTimes;
    private Transform cam;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public int moveSpeed;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        timeBeforeDive = Random.Range(diveTimes.x, diveTimes.y);
        rb = GetComponent<Rigidbody2D>();
        moveDirection = MoveDir();
    }

    void Update()
    {
        if (timeBeforeDive > 0 && GetComponent<BoxCollider2D>().enabled)
        {
            timeBeforeDive -= Time.deltaTime;
            rb.velocity = moveDirection * Time.deltaTime * moveSpeed;
        }
        if (timeBeforeDive <= 0)
        {
            StartCoroutine(Dive());
        }
    }

    Vector2 MoveDir()
    {
        Vector2 dir = Vector2.zero;
        int rdir = Random.Range(1, 4);

        if (rdir == 1)
            dir = new Vector2(0, 1);
        if (rdir == 2)
            dir = new Vector2(0, -1);
        if (rdir == 3)
            dir = new Vector2(1, 0);
        if (rdir == 4)
            dir = new Vector2(-1, 0);

        return dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 11)
            timeBeforeDive = 0;
    }

    IEnumerator Dive()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentsInChildren<BoxCollider2D>()[1].enabled = false;
        GetComponentInChildren<Animator>().SetBool("Diving", true);

        timeBeforeDive = Random.Range(diveTimes.x, diveTimes.y);
        
        //Find a new surfacing spot
        Vector3 position = new Vector2(cam.position.x + (Random.Range(-6, 7) * 0.16f) - 0.08f, cam.position.y + (Random.Range(-3, 3) * 0.16f) - 0.0232f);
        int a = 0;
        while (Physics2D.OverlapCircle(position, 0.12f))
        {
            a++;
            position = new Vector2(cam.position.x + (Random.Range(-6, 7) * 0.16f) - 0.08f, cam.position.y + (Random.Range(-3, 3) * 0.16f) - 0.0232f);
            if (a > 500)
                break;
        }

        yield return StartCoroutine(WaitFor.Frames(70));
        transform.position = position;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        yield return StartCoroutine(WaitFor.Frames(Random.Range(30, 140)));
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return StartCoroutine(WaitFor.Frames(70));

        //Raycast to pick a direction not towards a wall
        moveDirection = MoveDir();
        for (int i = 0; i < 20; i++)
        {
            Debug.DrawRay(transform.position, moveDirection, Color.blue, 5f);
            if (Physics2D.Raycast(transform.position, moveDirection, 0.32f))
            {
                moveDirection = MoveDir();
            }
            else
                break;
        }

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponentsInChildren<BoxCollider2D>()[1].enabled = true;
        GetComponentInChildren<Animator>().SetBool("Diving", false);
    }
}
