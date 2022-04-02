using System.Collections;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    private Animator player;
    private PlayerController control;
    private Rigidbody2D move;
    private PlayerController[] players;
    public string item;
    public Texture2D[] textures;
    public Color32[] colors = { new Color32(128, 208, 16, 255), new Color32(184, 184, 248, 255), new Color32(47, 31, 5, 255), new Color32(26, 26, 26, 255), new Color32(128, 208, 16, 255), new Color32(27, 58, 141, 255), new Color32(184, 184, 248, 255), new Color32(255, 255, 255, 255) };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            players = FindObjectsOfType<PlayerController>();
            control = collision.GetComponent<PlayerController>();
            player = collision.GetComponent<Animator>();
            move = collision.GetComponent<Rigidbody2D>();
            if (item == "Key")
            {
                PersistentManager.Instance.keys++;
                Destroy(gameObject);
            }
            if (item == "Compass")
            {
                PersistentManager.Instance.dungeonCompasses.Add(PersistentManager.Instance.inLevel);
                GameObject.Find("UI Compass").GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("Dungeon Compass Marker").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item == "Map")
            {
                PersistentManager.Instance.dungeonMaps.Add(PersistentManager.Instance.inLevel);
                PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel] = PersistentManager.Instance.dungeonRooms[PersistentManager.Instance.inLevel].ToList();
                PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel] = PersistentManager.Instance.dungeonDoors[PersistentManager.Instance.inLevel].ToList();
                GameObject.Find("UI Map").GetComponent<SpriteRenderer>().enabled = true;
                foreach (Vector2 i in PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel])
                {
                    GameObject room = Instantiate(Resources.Load("Map Room") as GameObject);
                    room.transform.parent = GameObject.Find("Map Data").transform;
                    room.transform.localPosition = Vector2.zero;
                    room.transform.localScale = Vector3.one;
                    room.transform.Translate(new Vector2(i.x * 0.08f, i.y * 0.04f));
                }
                foreach (Vector2 i in PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel])
                {
                    GameObject door = Instantiate(Resources.Load("Map Door") as GameObject);
                    door.transform.parent = GameObject.Find("Map Data").transform;
                    door.transform.localPosition = Vector2.zero;
                    door.transform.localScale = Vector3.one;
                    door.transform.Translate(new Vector2(i.x, i.y));
                }
            }
            if (item == "Heart Container")
            {
                PersistentManager.Instance.maxHealth += 4;
                for (var i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PlayerHealthManager>().health = PersistentManager.Instance.maxHealth;
                }
            }
            if (item == "Potion")
            {
                for (var i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<PlayerHealthManager>().health = PersistentManager.Instance.maxHealth;
                    players[i].hasPotion1 = false;
                    players[i].hasPotion2 = true;
                }
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
            }
            if (item == "Sword")
            {
                PersistentManager.Instance.hasSword1 = true;
            }
            if (item == "White Sword" && PersistentManager.Instance.maxHealth >= 20)
            {
                PersistentManager.Instance.hasSword2 = true;
            }
            if (item == "Master Sword" && PersistentManager.Instance.maxHealth >= 48)
            {
                PersistentManager.Instance.hasSword3 = true;
            }
            if (item == "Boomerang")
            {
                PersistentManager.Instance.hasBoomerang = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Magical Boomerang")
            {
                PersistentManager.Instance.hasBoomerang2 = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Bow")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasBow = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Silver Arrows")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasBow2 = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Red Candle")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasRedCandle = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Recorder")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasFlute = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Letter")
            {
                PersistentManager.Instance.hasLetter = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Wand")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasWand = true;
                GameObject.Find("UI").GetComponent<UIController>().DefineItems();
                GameObject.Find("UI").GetComponent<UIController>().NormalizeItems();
            }
            if (item == "Raft")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasRaft = true;
                GameObject.Find("Menu Raft").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item == "Book")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasBook = true;
                GameObject.Find("Menu Book").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item == "Red Ring")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasRing2 = true;
                GameObject.Find("Menu Red Ring").GetComponent<SpriteRenderer>().enabled = true;
                //Change color
                for (int i = 0; i < 4; i++)
                {
                    Color32[] cols = textures[i].GetPixels32();
                    for (var a = 0; a < cols.Length; a++)
                    {
                        //Debug.Log(cols[a]);
                        if (colors.Contains(cols[a]))
                            cols[a] = new Color32(179, 42, 11, 255);
                        if(cols[a].Equals(new Color32(206, 27, 27, 255)))
                            cols[a] = new Color32(255, 255, 255, 255);
                    }
                    textures[i].SetPixels32(cols);
                    textures[i].Apply(true);
                }
            }
            if (item == "Ladder")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasLadder = true;
                GameObject.Find("Menu Ladder").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item == "Skeleton Key")
            {
                PersistentManager.Instance.basementItemsCompleted.Add(name);
                PersistentManager.Instance.hasSkeletonKey = true;
                GameObject.Find("Menu Skeleton Key").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item == "Power Bracelet")
            {
                PersistentManager.Instance.hasPowerBracelet = true;
                GameObject.Find("Menu Power Bracelet").GetComponent<SpriteRenderer>().enabled = true;
            }
            if (item != "Key")
            {
                transform.position = new Vector2(collision.transform.position.x - 0.04f, collision.transform.position.y + 0.16f);
                player.SetBool("LiftItem", true);
                move.velocity = new Vector2(0f, 0f);
                control.enabled = false;
                StartCoroutine(wait());
            }
        }
    }
    IEnumerator wait()
    {
        PersistentManager.Instance.disableUI = true;
        FindObjectOfType<SFXManager>().PlaySound("Get Item");
        FindObjectOfType<MusicManager>().GetComponent<AudioSource>().Pause();
        yield return StartCoroutine(WaitFor.Frames(120));
        FindObjectOfType<MusicManager>().GetComponent<AudioSource>().UnPause();
        player.SetBool("LiftItem", false);
        control.enabled = true;
        GameObject.Find("UI").GetComponent<UIController>().DefineItems();
        PersistentManager.Instance.disableUI = false;
        Destroy(gameObject);
    }
}
