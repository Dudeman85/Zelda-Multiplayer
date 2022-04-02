using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : MonoBehaviour {

    private GameObject[] players;
    private bool hitPlayer;
    private int facing;

    // Use this for initialization
    void Start () {
        StartCoroutine(wait());
        players = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hitPlayer = true;
            facing = collision.GetComponent<PlayerController>().facing;
            for (var i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
                players[i].GetComponent<PlayerController>().enabled = false;
                players[i].GetComponent<SpriteRenderer>().enabled = false;
                players[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        if (collision.tag ==  "Projectile Wall")
        {
            Destroy(gameObject);
            if (hitPlayer)
                FindObjectOfType<FluteController>().TransitionLevels(facing);
        }
    }
    IEnumerator wait()
    {
        yield return StartCoroutine(WaitFor.Frames(15));
        GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, 0);
    }
}
