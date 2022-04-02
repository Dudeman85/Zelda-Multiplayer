using System.Collections;
using UnityEngine;

public class RockController : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool stop = true;
    private bool retard = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Rock());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile Wall" && retard && collision.contacts[0].point.y <= -7.0f)
        {
            retard = false;
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Relocate());
        }
    }

    IEnumerator Rock()
    {
        yield return StartCoroutine(WaitFor.Frames(30));

        int timeBeforeJump = Random.Range(20, 35);
        int dir = Random.Range(-1, 2);
        while (dir == 0)
            dir = Random.Range(-1, 2);

        while (stop)
        {
            dir = Random.Range(-1, 2);
            while (dir == 0)
                dir = Random.Range(-1, 2);
            timeBeforeJump = Random.Range(20, 40);

            rb.velocity = new Vector2(0.45f * dir, 0.60f);
            yield return StartCoroutine(WaitFor.Frames(10));
            rb.velocity = new Vector2(0.35f * dir, -1f);
            yield return StartCoroutine(WaitFor.Frames(timeBeforeJump));
        }
    }

    IEnumerator Relocate()
    {
        yield return StartCoroutine(WaitFor.Frames(60));
        stop = false;
        transform.Translate(new Vector2(100, 100));

        yield return StartCoroutine(WaitFor.Frames(Random.Range(20, 180)));

        rb.velocity = Vector2.zero;
        retard = true;
        stop = true;
        GetComponent<BoxCollider2D>().enabled = true;

        //Get New Position
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.position = new Vector2(cam.position.x + (Random.Range(-7, 7) * 0.16f), cam.position.y + 0.60f);

        StartCoroutine(Rock());
    }
}
