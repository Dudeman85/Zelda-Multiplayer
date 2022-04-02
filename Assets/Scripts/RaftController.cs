using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftController : MonoBehaviour
{

    private GameObject[] players;
    public GameObject raft;
    private GameObject raftClone;
    public Vector2 direction;
    private bool collided = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (PersistentManager.Instance.hasRaft && !collided)
        {
            FindObjectOfType<SFXManager>().PlaySound("Secret");
            collided = true;
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<BoxCollider2D>().enabled = false;
                players[i].GetComponent<PlayerController>().enabled = false;
                players[i].GetComponent<Rigidbody2D>().velocity = direction;
                players[i].transform.position = transform.position;
                players[i].GetComponent<Animator>().SetBool("Moving", false);
                players[i].GetComponent<PlayerController>().facing = 1;
                players[i].GetComponent<Animator>().SetFloat("LastMoveY", 1);
                if (direction == new Vector2(0, -0.5f))
                {
                    players[i].GetComponent<PlayerController>().facing = 3;
                    players[i].GetComponent<Animator>().SetFloat("LastMoveY", -1);
                }
            }
            PersistentManager.Instance.disableUI = true;
            raftClone = Instantiate(raft, transform.position, transform.rotation);
            raftClone.GetComponent<Rigidbody2D>().velocity = direction;
            StartCoroutine(Raft());
        }
    }
    IEnumerator Raft()
    {
        yield return StartCoroutine(WaitFor.Frames(82));
        Destroy(raftClone);
        PersistentManager.Instance.disableUI = false;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<BoxCollider2D>().enabled = true;
            players[i].GetComponent<PlayerController>().enabled = true;
            players[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        collided = false;
    }
}
