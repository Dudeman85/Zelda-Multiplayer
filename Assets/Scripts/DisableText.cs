using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PersistentManager.Instance.grottosCompleted.Add(PersistentManager.Instance.spawnpoint);
        GameObject.Find("Prices").GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return StartCoroutine(WaitFor.Frames(120));
        GameObject.Find("NPC Cover").GetComponent<SpriteRenderer>().enabled = true;
    }
}
