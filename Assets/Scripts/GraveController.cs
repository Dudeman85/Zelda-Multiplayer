using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveController : MonoBehaviour
{

    private bool spawnDelay;
    public GameObject ghini;
    private SpriteRenderer shader;
    private GameObject ghostClone;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !spawnDelay)
        {
            spawnDelay = true;
            ghostClone = Instantiate(ghini, transform.position, Quaternion.identity);
            ghostClone.GetComponent<FlyingEnemyController>().enabled = false;
            StartCoroutine(Spawn());
        }
    }
    
    IEnumerator Spawn()
    {
        shader = ghostClone.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 30; i++)
        {
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 2);
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 4);
        }
        ghostClone.GetComponent<FlyingEnemyController>().enabled = true;
        FindObjectOfType<EnemySpawnManager>().enemies.Add(ghostClone);
        spawnDelay = false;
    }
}
