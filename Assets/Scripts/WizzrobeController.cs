using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizzrobeController : MonoBehaviour
{

    private GameObject[] players;
    private GameObject target;
    private Vector2 direction;
    private float distanceFromPlayer;

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Attack()
    {
        while (true)
        {
            //Get target player
            target = players[Random.Range(0, players.Length)];

            //Get Direction
            int dir = Random.Range(1, 4);
            if (dir == 1)
                direction = new Vector2(0, 1);
            if (dir == 2)
                direction = new Vector2(1, 0);
            if (dir == 3)
                direction = new Vector2(0, -1);
            if (dir == 4)
                direction = new Vector2(-1, 0);
            
            RaycastHit2D hit = Physics2D.Raycast(target.transform.position, direction);

            Debug.Log(hit.distance);
            distanceFromPlayer = Random.Range(0.16f, hit.distance - 0.16f);

            Debug.DrawRay(target.transform.position, direction * distanceFromPlayer, Color.blue);

            transform.position = (Vector2)target.transform.position + (direction * distanceFromPlayer);

            yield return StartCoroutine(WaitFor.Frames(240));
        }
    }
}
