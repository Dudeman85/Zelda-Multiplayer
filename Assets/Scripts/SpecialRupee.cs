using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRupee : MonoBehaviour
{

    public int amount;
    public bool everyone;
    public bool door;

    // Use this for initialization
    void Start()
    {
        if (door)
        {
            StartCoroutine(Door());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        if (collision.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            if (everyone)
            {
                if(amount < 0 && collision.name == "Link")
                {
                    while (amount % players.Length != 0)
                        amount++;
                    for (var i = 0; i < players.Length; i++)
                    {
                        if (players[i].rupees + (amount / players.Length) > 0)
                        {
                            players[i].AddRupees(amount / players.Length);
                        }
                        else
                        {
                            players[i].RupeeCounter(0);
                        }
                    }
                }
                else if(amount > 0)
                {
                    while (amount % players.Length != 0)
                        amount++;
                    for (var i = 0; i < players.Length; i++)
                    {
                        if (players[i].rupees + (amount / players.Length) > 0)
                        {
                            players[i].AddRupees(amount / players.Length);
                        }
                        else
                        {
                            players[i].RupeeCounter(0);
                        }
                    }
                }
            }
            else
            {
                if (collision.GetComponent<PlayerController>().rupees + amount > 0)
                {
                    collision.GetComponent<PlayerController>().AddRupees(amount);
                }
                else
                {
                    collision.GetComponent<PlayerController>().RupeeCounter(0);
                }
            }
            if (door)
            {
                PersistentManager.Instance.grottosCompleted.Add(PersistentManager.Instance.spawnpoint);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Door()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return StartCoroutine(WaitFor.Frames(220));
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
