using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public RuntimeAnimatorController ghostAnim;
    public int health;
    public float invincibility;
    private Rigidbody2D move;
    private PlayerController control;
    public Slider healthBar;
    public Slider healthBar2;
    public Slider healthBar3;
    public Slider healthCover;
    public Slider healthCover2;
    public Slider healthCover3;
    private GameObject[] fairyFountainHeart = new GameObject[8];
    private BoxCollider2D playerCollider;
    private SpriteRenderer shader;
    private GameObject cam;
    private bool knockback;

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        shader = GetComponent<SpriteRenderer>();
        shader.material.SetInt("_CurrentPallette", 0);

        playerCollider = GetComponent<BoxCollider2D>();
        move = GetComponent<Rigidbody2D>();
        control = GetComponent<PlayerController>();

        healthCover.direction = Slider.Direction.RightToLeft;
        healthCover2.direction = Slider.Direction.RightToLeft;
        healthCover3.direction = Slider.Direction.RightToLeft;
    }

    private void OnLevelWasLoaded(int level)
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (knockback)
        {
            if(Mathf.Abs(cam.transform.position.x - gameObject.transform.position.x) > 1.15f || Mathf.Abs(cam.transform.position.y - gameObject.transform.position.y) > 0.6f)
            {
                knockback = false;
                control.enabled = true;
                move.velocity = new Vector2(0f, 0f);
                PersistentManager.Instance.disableTransitions = false;
            }
        }
        if (invincibility > 0)
        {
            invincibility -= 1;
        }
        if (health <= 24)
        {
            healthBar.value = health / 2;
            healthBar2.value = 0;
            healthBar3.value = 0;
        }
        else if (health <= 48)
        {
            healthBar.value = 12;
            healthBar2.value = health / 2 - 12;
            healthBar3.value = 0;
        }
        else if (health <= 72)
        {
            healthBar.value = 12;
            healthBar2.value = 12;
            healthBar3.value = health / 2 - 24;
        }
        if (PersistentManager.Instance.maxHealth <= 24)
        {
            healthCover.value = 6 - PersistentManager.Instance.maxHealth / 4;
            healthCover2.value = 6;
            healthCover3.value = 6;
        }
        else if (PersistentManager.Instance.maxHealth <= 48)
        {
            healthCover2.value = 12 - PersistentManager.Instance.maxHealth / 4;
            healthCover3.value = 6;
        }
        else if (PersistentManager.Instance.maxHealth <= 64)
        {
            healthCover3.value = 18 - PersistentManager.Instance.maxHealth / 4;
        }
    }

    public void HitPlayer(int damage, int deflectable, Collider2D collider)
    {
        if (invincibility <= 0f)
        {
            Vector2 direction = new Vector2(collider.bounds.center.x - playerCollider.bounds.center.x, collider.bounds.center.y - playerCollider.bounds.center.y);

            //Test for shield
            if ((deflectable == 1 && control.hasMagicalShield) || (deflectable == 2))
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) //If collider is on the left or right
                {
                    if (direction.x > 0) //If collider is on the right
                    {
                        if (control.facing == 4)
                        {
                            FindObjectOfType<SFXManager>().PlaySound("Block");
                            Destroy(collider.gameObject);
                            return;
                        }
                    }
                    else //If collider is on the left
                    {
                        if (control.facing == 2)
                        {
                            FindObjectOfType<SFXManager>().PlaySound("Block");
                            Destroy(collider.gameObject);
                            return;
                        }
                    }
                }
                else //If collider is on top or bottom
                {
                    if (direction.y > 0) //If collider is on the top
                    {
                        if (control.facing == 1)
                        {
                            FindObjectOfType<SFXManager>().PlaySound("Block");
                            Destroy(collider.gameObject);
                            return;
                        }
                    }
                    else //If collider is on the bottom
                    {
                        if (control.facing == 3)
                        {
                            FindObjectOfType<SFXManager>().PlaySound("Block");
                            Destroy(collider.gameObject);
                            return;
                        }
                    }
                }
            }
            
            FindObjectOfType<SFXManager>().PlaySound("Link Hurt");
            //Damage player
            if (health - damage > 0)
            {
                if (PersistentManager.Instance.hasRing2)
                    damage /= 4;
                else if (GetComponent<PlayerController>().hasRing1)
                    damage /= 2;
                if (damage == 0)
                    damage = 1;
                health -= damage;
                invincibility += 60;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) //If collider is on the left or right
                {
                    if (direction.x > 0) //If collider is on the right
                    {
                        move.velocity = new Vector2(-1.5f, 0);
                    }
                    else //If collider is on the left
                    {
                        move.velocity = new Vector2(1.5f, 0);
                    }
                }
                else //If collider is on top or bottom
                {
                    if (direction.y > 0) //If collider is on the top
                    {
                        move.velocity = new Vector2(0, -1.5f);
                    }
                    else //If collider is on the bottom
                    {
                        move.velocity = new Vector2(0, 1.5f);
                    }
                }

                knockback = true;
                control.enabled = false;
                StartCoroutine(Flash());
                StartCoroutine(Knockback());
            }
            else
            {
                //Kill the player
                FindObjectOfType<SFXManager>().PlaySound("Link Die");
                healthBar.value = 0;
                health = 0;
                StartCoroutine(Death());
            }
        }
    }
    IEnumerator Knockback()
    {
        PersistentManager.Instance.disableTransitions = true;
        yield return StartCoroutine(WaitFor.Frames(15));
        move.velocity = new Vector2(0f, 0f);
        PersistentManager.Instance.disableTransitions = false;
        control.enabled = true;
        knockback = false;
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
        shader.material.SetInt("_CurrentPallette", 0);
    }

    IEnumerator Death()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerController>().dead = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().runtimeAnimatorController = ghostAnim;
        yield return StartCoroutine(WaitFor.Frames(160));
        GetComponent<PlayerController>().attackingAnim = true;
        FindObjectOfType<SFXManager>().PlaySound("Text");
        GetComponent<PlayerController>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<PlayerController>().attacking = -1;
        gameObject.layer = 18;

    }

    public IEnumerator Heal(GameObject player, int amount)
    {
        if (amount > PersistentManager.Instance.maxHealth - health)
            amount = PersistentManager.Instance.maxHealth - health;
        FindObjectOfType<SFXManager>().PlaySound("Refill Health", amount * 4);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Animator>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        invincibility = 1000;
        for (int i = 0; i < amount; i += 2)
        {
            player.GetComponent<PlayerHealthManager>().health += 2;
            yield return StartCoroutine(WaitFor.Frames(16));
        }
        FindObjectOfType<SFXManager>().PlaySound("Text");
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Animator>().enabled = true;
        PersistentManager.Instance.disablePausing = false;
        invincibility = 0;
    }

    public IEnumerator FairyFountain(GameObject player, int amount, GameObject fountain)
    {
        PersistentManager.Instance.disableUI = true;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        player.GetComponent<Animator>().SetBool("Moving", false);
        FindObjectOfType<SFXManager>().PlaySound("Refill Health", amount * 4);
        for (int i = 0; i < amount; i += 2)
        {
            player.GetComponent<PlayerHealthManager>().health += 2;
            yield return StartCoroutine(WaitFor.Frames(16));
        }
        if (amount > 12)
        {
            PersistentManager.Instance.disableUI = false;
            player.GetComponent<PlayerController>().enabled = true;
            fountain.GetComponents<BoxCollider2D>()[1].enabled = false;
            for (int i = 0; i < 8; i++)
                Destroy(fairyFountainHeart[i]);
        }
        FindObjectOfType<SFXManager>().PlaySound("Text");

        if (player.GetComponent<PlayerController>().dead)
        {
            player.GetComponent<PlayerController>().attackingAnim = false;
            player.GetComponent<PlayerController>().dead = false;
            player.GetComponent<PlayerController>().attacking = 0;
            player.layer = 9;
            //Animator
            if (player.GetComponent<PlayerController>().hasMagicalShield)
                player.GetComponent<PlayerController>().GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<PlayerController>().largeShield;
            else
                player.GetComponent<PlayerController>().GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<PlayerController>().smallShield;
        }
    }

    public IEnumerator FairyFountainHearts(GameObject heart, GameObject player, int amount, GameObject fountain)
    {
        for (int i = 0; i < 8; i++)
        {
            fairyFountainHeart[i] = Instantiate(heart, fountain.transform, false);
            yield return StartCoroutine(WaitFor.Frames(15));
        }
        if (amount < 12)
        {
            yield return StartCoroutine(WaitFor.Frames(30));
            PersistentManager.Instance.disableUI = false;
            player.GetComponent<PlayerController>().enabled = true;
            fountain.GetComponents<BoxCollider2D>()[1].enabled = false;
            for (int i = 0; i < 8; i++)
                Destroy(fairyFountainHeart[i]);
        }
    }
}