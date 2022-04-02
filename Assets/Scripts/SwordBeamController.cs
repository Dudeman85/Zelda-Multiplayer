using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBeamController : MonoBehaviour
{
    public GameObject hitParticle;
    private GameObject part1;
    private GameObject part2;
    private GameObject part3;
    private GameObject part4;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Projectile Wall")
        {
            if (!part1)
            {
                part1 = Instantiate(hitParticle, transform.position, Quaternion.identity);
                part1.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1);
                part1.transform.localScale = new Vector3(-1, 1, 0);
                part2 = Instantiate(hitParticle, transform.position, Quaternion.identity);
                part2.GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1);
                part2.transform.localScale = new Vector3(-1, -1, 0);
                part3 = Instantiate(hitParticle, transform.position, Quaternion.identity);
                part3.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1);
                part4 = Instantiate(hitParticle, transform.position, Quaternion.identity);
                part4.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1);
                part4.transform.localScale = new Vector3(1, -1, 0);
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(Destroy());
            }
        }
    }
    IEnumerator Destroy()
    {
        yield return StartCoroutine(WaitFor.Frames(15));
        Destroy(part1);
        Destroy(part2);
        Destroy(part3);
        Destroy(part4);
        Destroy(gameObject);
    }
}
