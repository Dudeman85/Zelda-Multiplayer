using UnityEngine;

public class TempItemManager : MonoBehaviour
{

    private GameObject[] enemies;
    private int itemLife = 600;
    private float tilesToMove;
    private int moveDirection;
    private Rigidbody2D rb;
    public float moveSpeed;
    private int lastMove;
    public string item;
    public bool noLife;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Item Life Manager
        itemLife--;
        if (itemLife < 0 && !noLife)
        {
            Destroy(gameObject);
        }
        //Fairy Movement
        if (item == "Fairy")
        {
            if (tilesToMove > 0)
            {
                tilesToMove -= Time.deltaTime * 2;
                //Up
                if (moveDirection == 1)
                    rb.velocity = new Vector3(0f, 1 * moveSpeed * Time.deltaTime, 0f);
                //Up Right
                if (moveDirection == 2)
                    rb.velocity = new Vector3(0.7f * moveSpeed * Time.deltaTime, 0.7f * moveSpeed * Time.deltaTime, 0f);
                //Right
                if (moveDirection == 3)
                    rb.velocity = new Vector3(1 * moveSpeed * Time.deltaTime, 0f, 0f);
                //Down Right
                if (moveDirection == 4)
                    rb.velocity = new Vector3(0.7f * moveSpeed * Time.deltaTime, -0.7f * moveSpeed * Time.deltaTime, 0f);
                //Down
                if (moveDirection == 5)
                    rb.velocity = new Vector3(0f, -1f * moveSpeed * Time.deltaTime, 0f);
                //Down Left
                if (moveDirection == 6)
                    rb.velocity = new Vector3(-0.7f * moveSpeed * Time.deltaTime, -0.7f * moveSpeed * Time.deltaTime, 0f);
                //Left
                if (moveDirection == 7)
                    rb.velocity = new Vector3(-1f * moveSpeed * Time.deltaTime, 0f, 0f);
                //Up Left
                if (moveDirection == 8)
                    rb.velocity = new Vector3(-0.7f * moveSpeed * Time.deltaTime, 0.7f * moveSpeed * Time.deltaTime, 0f);
            }
            //Choose New Movement Direction for fairy
            else
            {
                rb.velocity = Vector2.zero;
                moveDirection = Random.Range(1, 8);
                if (moveDirection != lastMove)
                {
                    lastMove = moveDirection;
                    tilesToMove = Random.Range(0.5f, 3f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.name == "Sword")
        {
            //Heart
            if (item == "Heart" && collision.GetComponentInParent<PlayerHealthManager>().health != PersistentManager.Instance.maxHealth)
            {
                if (PersistentManager.Instance.maxHealth - collision.GetComponentInParent<PlayerHealthManager>().health >= 4)
                {
                    collision.GetComponentInParent<PlayerHealthManager>().health += 4;
                }
                else
                {
                    collision.GetComponentInParent<PlayerHealthManager>().health = PersistentManager.Instance.maxHealth;
                }
                Destroy(gameObject);
            }
            //Fairy
            if (item == "Fairy" && collision.GetComponentInParent<PlayerHealthManager>().health != PersistentManager.Instance.maxHealth)
            {
                if (PersistentManager.Instance.maxHealth - collision.GetComponentInParent<PlayerHealthManager>().health >= 12)
                {
                    collision.GetComponentInParent<PlayerHealthManager>().health += 12;
                }
                else
                {
                    collision.GetComponentInParent<PlayerHealthManager>().health = PersistentManager.Instance.maxHealth;
                }
                Destroy(gameObject);
            }
            //Bomb
            if (item == "Bomb" && collision.GetComponentInParent<PlayerController>().bombs != collision.GetComponentInParent<PlayerController>().maxBombs)
            {
                PersistentManager.Instance.hasBombs = true;
                if (collision.GetComponentInParent<PlayerController>().maxBombs - collision.GetComponentInParent<PlayerController>().bombs >= 4)
                {
                    collision.GetComponentInParent<PlayerController>().AddBombs(4);
                }
                else
                {
                    collision.GetComponentInParent<PlayerController>().BombCounter(collision.GetComponentInParent<PlayerController>().maxBombs);
                }
                Destroy(gameObject);
            }
            //1 Rupee
            if (item == "Rupee" && collision.GetComponentInParent<PlayerController>().rupees != 999)
            {
                collision.GetComponentInParent<PlayerController>().AddRupees(1);
                Destroy(gameObject);
            }
            //5 Rupees
            if (item == "5 Rupees" && collision.GetComponentInParent<PlayerController>().rupees != 999)
            {
                if (999 - collision.GetComponentInParent<PlayerController>().rupees >= 5)
                {
                    collision.GetComponentInParent<PlayerController>().AddRupees(5);
                }
                else
                {
                    collision.GetComponentInParent<PlayerController>().RupeeCounter(999);
                }
                Destroy(gameObject);
            }
            //Clock
            if (item == "Clock")
            {
                Destroy(gameObject);
                //Disable All Enemy Movement
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i <= enemies.Length; i++)
                {
                    enemies[i].GetComponentsInChildren<BoxCollider2D>()[1].enabled = false;
                    enemies[i].GetComponentInParent<EnemyController>().enabled = false;
                    enemies[i].GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    enemies[i].GetComponentInParent<EnemyHealthManager>().health = 1;
                }
            }
        }
    }
}