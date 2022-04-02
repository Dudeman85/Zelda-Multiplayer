using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawnManager : MonoBehaviour
{
    public X[] overworld = new X[16];
    public X[] dungeon = new X[16];

    public X[] currentEnemies = new X[16];
    private X[] currDungeon = new X[16];
    private Y pos;
    private Transform cam;
    public List<GameObject> enemies = new List<GameObject>();
    private Tilemap tilemap;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        for (int x = 0; x < overworld.Length; x++)
        {
            for (int y = 0; y < overworld[x].y.Length; y++)
            {
                currentEnemies[x].y[y].enemies = new GameObject[overworld[x].y[y].enemies.Length];
                for (int i = 0; i < overworld[x].y[y].enemies.Length; i++)
                {
                    currentEnemies[x].y[y].enemies[i] = overworld[x].y[y].enemies[i].gameObject;
                }
            }
        }
    }

    public void Spawn()
    {
        tilemap = GameObject.Find("Special Spawns").GetComponent<Tilemap>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //Get enemies from list for current screen
        pos = currentEnemies[PersistentManager.Instance.playerLocation.x - 1].y[PersistentManager.Instance.playerLocation.y - 1];
        GameObject[] spawns = pos.enemies;

        foreach (GameObject enemy in spawns)
        {
            if (enemy)
            {
                if (enemy.name != "Zora")
                {
                    //Find Spot Without collision
                    Vector3 position = new Vector2(cam.position.x + (Random.Range(-6, 6) * 0.16f), cam.position.y + (Random.Range(-3, 3) * 0.16f));
                    while (Physics2D.OverlapCircle(position, 0.01f))
                        position = new Vector2(cam.position.x + (Random.Range(-6, 6) * 0.16f), cam.position.y + (Random.Range(-3, 3) * 0.16f));
                    GameObject temp = Instantiate(enemy, position, transform.rotation);

                    if (temp.GetComponent<EnemyController>())
                    {
                        temp.GetComponent<EnemyController>().enabled = false;
                        if (Random.Range(1, 4) == 1)
                            temp.GetComponent<EnemyController>().moveSpeed += 12;
                    }
                    if (temp.GetComponent<FlyingEnemyController>())
                        temp.GetComponent<FlyingEnemyController>().enabled = false;
                    if (temp.GetComponent<LeeverController>())
                        temp.GetComponent<LeeverController>().enabled = false;

                    enemies.Add(temp);
                }
                else
                {
                    Vector3 position = new Vector2(cam.position.x + (Random.Range(-7, 7) * 0.16f), cam.position.y + (Random.Range(-4, 4) * 0.16f));
                    int i = 0;
                    while (!tilemap.HasTile(tilemap.WorldToCell(position)) && i < 100)
                    {
                        position = new Vector2(cam.position.x + (Random.Range(-6, 6) * 0.16f), cam.position.y + (Random.Range(-3, 3) * 0.16f));
                        i++;
                    }
                    if (i < 100)
                        enemies.Add(Instantiate(enemy, tilemap.GetCellCenterWorld(tilemap.WorldToCell(position)), transform.rotation));
                }
            }
        }
        StartCoroutine(WaitToMove());
    }

    public void Despawn()
    {
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
        enemies.Clear();
    }

    IEnumerator WaitToMove()
    {
        yield return StartCoroutine(WaitFor.Frames(36));
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyController>())
                enemy.GetComponent<EnemyController>().enabled = true;
            if (enemy.GetComponent<FlyingEnemyController>())
                enemy.GetComponent<FlyingEnemyController>().enabled = true;
            if (enemy.GetComponent<LeeverController>())
                enemy.GetComponent<LeeverController>().enabled = true;
        }
    }
}

[System.Serializable]
public class X
{
    public Y[] y = new Y[8];
}
[System.Serializable]
public class Y
{
    public GameObject[] enemies;
}