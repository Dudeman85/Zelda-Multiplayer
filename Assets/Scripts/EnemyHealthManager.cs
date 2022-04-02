using System.Collections;
using UnityEngine;
using System.Linq;

public class EnemyHealthManager : MonoBehaviour
{

    public int maxHealth;
    public int health;
    public bool isWeak;
    public bool invulnerable;
    public float invincibility;
    public int defaultSprite;
    private Rigidbody2D move;
    private EnemyController control;
    private SpriteRenderer shader;
    private bool dead = false;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        control = GetComponent<EnemyController>();
        move = GetComponent<Rigidbody2D>();
        shader = GetComponentInChildren<SpriteRenderer>();
        if (shader == null)
            shader = GetComponent<SpriteRenderer>();
        StartCoroutine(Startup());
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibility > 0)
            invincibility -= 1;
    }

    public void HitEnemy(int damage, int direction)
    {
        if (invincibility <= 0f && !invulnerable)
        {
            if (health - damage > 0)
            {
                FindObjectOfType<SFXManager>().PlaySound("Enemy Hit");
                health -= damage;
                if (move)
                {
                    //Stun
                    if (direction == -1)
                    {
                        move.velocity = new Vector2(0f, 0f);
                        Stun();
                    }
                    else
                    {
                        invincibility = 30;
                        StartCoroutine(Flash());
                        StartCoroutine(Knockback());
                    }
                    //Up
                    if (direction == 1)
                    {
                        move.velocity = new Vector2(0f, 1.5f);
                    }
                    //Down
                    if (direction == 3)
                    {
                        move.velocity = new Vector2(0f, -1.5f);
                    }
                    //Left
                    if (direction == 4)
                    {
                        move.velocity = new Vector2(1.5f, 0f);
                    }
                    //Right
                    if (direction == 2)
                    {
                        move.velocity = new Vector2(-1.5f, 0f);
                    }
                }
                else
                {
                    invincibility = 30;
                    StartCoroutine(Flash());
                }
            }
            else if (!dead)
            {
                FindObjectOfType<SFXManager>().PlaySound("Enemy Die");
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Startup()
    {
        yield return StartCoroutine(WaitFor.Frames(35));
        shader.material.SetInt("_CurrentPallette", defaultSprite);
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 5; i++)
        {
            shader.material.SetInt("_CurrentPallette", 1);
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 2);
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 3);
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 4);
            yield return StartCoroutine(WaitFor.Frames(2));
        }
        shader.material.SetInt("_CurrentPallette", defaultSprite);
    }

    public IEnumerator Die()
    {
        dead = true;
        GameObject[] enemy = FindObjectOfType<EnemySpawnManager>().currentEnemies[PersistentManager.Instance.playerLocation.x - 1].y[PersistentManager.Instance.playerLocation.y - 1].enemies;

        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i])
            {
                if (enemy[i].name == gameObject.name.Replace("(Clone)", ""))
                {
                    enemy[i] = null;
                    break;
                }
            }
        }

        GetComponentInChildren<Animator>().enabled = true;
        GetComponentInChildren<Animator>().SetBool("Die", true);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (GetComponent<EnemyController>())
            GetComponent<EnemyController>().enabled = false;
        if (GetComponent<FlyingEnemyController>())
            GetComponent<FlyingEnemyController>().enabled = false;
        if (GetComponent<JumpyEnemyController>())
            GetComponent<JumpyEnemyController>().enabled = false;
        if (GetComponent<LeeverController>())
            GetComponent<LeeverController>().enabled = false;
        if (GetComponent<BossGhini>())
            GetComponent<BossGhini>().Die();
        if (GetComponentInChildren<Animator>())
            yield return StartCoroutine(WaitFor.Frames(15));
        Destroy(gameObject);
    }

    IEnumerator Knockback()
    {
        if (control)
        {
            control.enabled = false;
            yield return StartCoroutine(WaitFor.Frames(15));
            move.velocity = new Vector2(0f, 0f);
            control.enabled = true;
        }
    }

    IEnumerator Stun()
    {
        if (control)
        {
            control.enabled = false;
            yield return StartCoroutine(WaitFor.Frames(300));
            control.enabled = true;
        }
    }
}
