using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{

    public GameObject player;
    public GameObject fire;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<SFXManager>().PlaySound("Magic Rod");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile Wall" || collision.tag == "Enemy")
        {
            GetComponent<Animator>().SetBool("Collision", true);
            if (PersistentManager.Instance.hasBook)
            {
                StartCoroutine(Fire());
            }
            else
            {
                StartCoroutine(Delete());
            }
        }
    }

    IEnumerator Delete()
    {
        yield return StartCoroutine(WaitFor.Frames(4));
        player.GetComponent<PlayerController>().magicBeamExists = false;
        player.GetComponent<PlayerController>().BInUse = 0;
        Destroy(gameObject);
    }
    IEnumerator Fire()
    {
        yield return StartCoroutine(WaitFor.Frames(4));
        GameObject fireClone = Instantiate(fire, transform.position, transform.rotation);
        transform.position = new Vector2(1000, 1000);
        player.GetComponent<PlayerController>().magicBeamExists = false;
        player.GetComponent<PlayerController>().BInUse = 0;
        yield return StartCoroutine(WaitFor.Frames(60));
        Destroy(fireClone);
        Destroy(gameObject);
    }
}
