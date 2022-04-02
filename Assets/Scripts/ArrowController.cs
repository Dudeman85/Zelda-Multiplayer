using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    private Sprite[] allitemsprites;
    public GameObject player;

	// Use this for initialization
	void Start ()
    {
        allitemsprites = Resources.LoadAll<Sprite>("Object Sprites");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile Wall" || collision.tag == "Enemy Sprite")
        {
            GetComponent<SpriteRenderer>().sprite = allitemsprites[21];
            StartCoroutine(delete());
        }
    }

    IEnumerator delete()
    {
        yield return StartCoroutine(WaitFor.Frames(3));
        player.GetComponent<PlayerController>().BInUse = 0;
        Destroy(gameObject);
    }
}
