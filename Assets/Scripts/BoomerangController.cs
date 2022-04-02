using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D move;
    private bool moveBack;
    private Coroutine sound;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponent<Rigidbody2D>();
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBack)
            transform.position = Vector2.MoveTowards(transform.position, transform.parent.position, Time.deltaTime * 1.5f);
    }

    public void Throw(Vector2 dir)
    {
        sound = StartCoroutine(FindObjectOfType<SFXManager>().PlaySoundEffect("Arrow Boomerang", 50));
        GetComponent<BoxCollider2D>().enabled = true;
        transform.position = transform.parent.position;
        moveBack = false;
        move.position += dir / 8;
        move.velocity = 1.5f * dir;
        if (PersistentManager.Instance.hasBoomerang2)
        {
            anim.SetBool("Animate 2", true);
        }
        else
        {
            anim.SetBool("Animate", true);
            StartCoroutine(distance());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject == transform.parent.gameObject)
        {
            StopCoroutine(sound);
            AudioSource[] a = FindObjectOfType<SFXManager>().GetComponents<AudioSource>();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].clip.name == "Arrow Boomerang")
                {
                    Destroy(a[i]);
                    break;
                }
            }
            collision.GetComponent<PlayerController>().BInUse = 10;
            collision.GetComponent<PlayerController>().movementPaused = 10;
            collision.GetComponent<PlayerController>().thrusting = true;
            StartCoroutine(thrust(collision));
            anim.SetBool("Animate", false);
            anim.SetBool("Animate 2", false);
        }
        if(collision.tag == "Projectile Wall" || collision.tag == "Enemy Sprite")
        {
            anim.SetBool("Collision", true);
            move.velocity = Vector2.zero;
            StopCoroutine(distance());
            StartCoroutine(hit());
        }
    }
    IEnumerator distance()
    {
        yield return StartCoroutine(WaitFor.Frames(32));
        move.velocity = Vector2.zero;
        moveBack = true;
    }
    IEnumerator hit()
    {
        yield return StartCoroutine(WaitFor.Frames(3));
        anim.SetBool("Collision", false);
        moveBack = true;
    }
    IEnumerator thrust(Collider2D player)
    {
        yield return StartCoroutine(WaitFor.Frames(10));
        player.GetComponent<PlayerController>().thrusting = false;
    }
}