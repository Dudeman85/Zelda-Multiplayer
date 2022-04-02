using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{

    public int cost;
    public string item;
    private PlayerController player;
    private Animator playerAnim;
    public Texture2D playerTexture;

    // Use this for initialization
    void Start()
    {
        if (item == "Bomb Upgrade A13" && PersistentManager.Instance.A13Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
            Destroy(gameObject);
        if (item == "Bomb Upgrade H2" && PersistentManager.Instance.H2Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<PlayerController>();
            playerAnim = collision.GetComponent<Animator>();
            if (player.rupees >= cost)
            {
                if (item == "Bomb Upgrade A13" && !PersistentManager.Instance.A13Used.Contains(collision.name))
                {
                    PersistentManager.Instance.A13Used.Add(collision.name);
                    player.maxBombs += 4;
                    player.AddBombs(player.maxBombs - player.bombs);
                    player.AddRupees(-cost);
                    if (item == "Bomb Upgrade A13" && PersistentManager.Instance.A13Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
                    {
                        GameObject.Find("Text Cover A13").GetComponent<SpriteRenderer>().enabled = true;
                        Destroy(GameObject.Find("A13"));
                        Debug.Log("destroy");
                        Destroy(gameObject);
                    }
                }
                if (item == "Bomb Upgrade H2" && !PersistentManager.Instance.H2Used.Contains(collision.name))
                {
                    PersistentManager.Instance.H2Used.Add(collision.name);
                    player.maxBombs += 4;
                    player.AddBombs(player.maxBombs - player.bombs);
                    player.AddRupees(-cost);
                    if (item == "Bomb Upgrade H2" && PersistentManager.Instance.H2Used.Count == GameObject.FindGameObjectsWithTag("Player").Length)
                    {
                        GameObject.Find("Text Cover H2").GetComponent<SpriteRenderer>().enabled = true;
                        Destroy(GameObject.Find("H2"));
                        Debug.Log("destroy");
                        Destroy(gameObject);
                    }
                }
                if (item == "Potion" && !player.hasPotion1 && !player.hasPotion2)
                {
                    StartCoroutine(Lift());
                    player.hasPotion1 = true;
                    player.AddRupees(-cost);
                    GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                    GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
                }
                if (item == "Potion 2" && !player.hasPotion2 && !player.hasPotion1)
                {
                    StartCoroutine(Lift());
                    player.hasPotion2 = true;
                    player.AddRupees(-cost);
                    GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                    GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
                }
                if (item == "Shield" && !player.hasMagicalShield)
                {
                    StartCoroutine(Lift());
                    player.hasMagicalShield = true;
                    player.AddRupees(-cost);
                }
                if (item == "Ring" && !player.hasRing1)
                {
                    if (!PersistentManager.Instance.hasRing2)
                    {
                        playerTexture = player.gameObject.GetComponent<SpriteRenderer>().sprite.texture;
                        StartCoroutine(Lift());
                        PersistentManager.Instance.hasRing = true;
                        player.hasRing1 = true;
                        player.AddRupees(-cost);
                        GameObject.Find("Menu Blue Ring").GetComponent<SpriteRenderer>().enabled = true;
                        //Change Player color
                        Color32 oldColor = new Color32();
                        Color32 newColor = new Color32();
                        if(playerTexture.name == "Link Sprites")
                        {
                            Debug.Log("Link");
                            oldColor = new Color32(128, 208, 16, 255);
                            newColor = new Color32(184, 184, 248, 255);
                        }
                        if(playerTexture.name == "Viking Sprites")
                        {
                            Debug.Log("viking");
                            oldColor = new Color32(47, 31, 5, 255);
                            newColor = new Color32(27, 58, 141, 255);
                        }
                        if(playerTexture.name == "Pirate Sprites")
                        {
                            Debug.Log("pirate");
                            oldColor = new Color32(26, 26, 26, 255);
                            newColor = new Color32(27, 58, 141, 255);
                        }
                        if(playerTexture.name == "Knight Sprites")
                        {
                            Debug.Log("knight");
                            oldColor = new Color32(128, 208, 16, 255);
                            newColor = new Color32(184, 184, 248, 255);
                        }
                        
                        Color32[] cols = playerTexture.GetPixels32();
                        for (var i = 0; i < cols.Length; ++i)
                            if (cols[i].Equals(oldColor))
                                cols[i] = newColor;

                        playerTexture.SetPixels32(cols);
                        playerTexture.Apply(true);
                    }
                }
                if (item == "Meat" && !player.hasMeat)
                {
                    StartCoroutine(Lift());
                    player.hasMeat = true;
                    player.AddRupees(-cost);
                    GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                    GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
                }
                if (item == "Key" && PersistentManager.Instance.keys < 9)
                {
                    StartCoroutine(Lift());
                    PersistentManager.Instance.keys++;
                    player.AddRupees(-cost);
                }
                if (item == "Heart" && player.GetComponent<PlayerHealthManager>().health != PersistentManager.Instance.maxHealth)
                {
                    StartCoroutine(Lift());
                    player.GetComponent<PlayerHealthManager>().health = PersistentManager.Instance.maxHealth;
                    player.AddRupees(-cost);
                }
                if (item == "Arrow" && !player.hasArrows)
                {
                    StartCoroutine(Lift());
                    player.hasArrows = true;
                    player.AddRupees(-cost);
                }
                if (item == "Bomb" && player.bombs != player.maxBombs && PersistentManager.Instance.hasBombs)
                {
                    StartCoroutine(Lift());
                    if (player.maxBombs - player.bombs >= 4)
                    {
                        player.bombs += 4;
                    }
                    else
                    {
                        player.bombs = player.maxBombs;
                    }
                    player.AddRupees(-cost);
                }
                if (item == "Candle" && !player.hasCandle)
                {
                    StartCoroutine(Lift());
                    player.hasCandle = true;
                    player.AddRupees(-cost);
                    GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                    GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
                }
            }
        }
    }
    IEnumerator Lift()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position = new Vector2(player.transform.position.x - 0.04f, player.transform.position.y + 0.16f);
        playerAnim.SetBool("LiftItem", true);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        player.enabled = false;
        yield return StartCoroutine(WaitFor.Frames(120));
        playerAnim.SetBool("LiftItem", false);
        player.enabled = true;
        if (item == "Shield")
            player.GetComponent<Animator>().runtimeAnimatorController = player.largeShield;
        Destroy(gameObject);
    }
}
