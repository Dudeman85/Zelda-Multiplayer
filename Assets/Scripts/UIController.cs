using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIController : MonoBehaviour
{

    public static UIController Instance { get; set; }

    private static bool UIExists;

    //UI Components
    public Animator animateP1;
    public Animator animateP2;
    public Animator animateP3;
    public Animator animateP4;
    public GameObject P1ItemCover;
    public GameObject P2ItemCover;
    public GameObject P3ItemCover;
    public GameObject P4ItemCover;
    public GameObject P1Selector;
    public GameObject P2Selector;
    public GameObject P3Selector;
    public GameObject P4Selector;
    public List<GameObject> P1MenuItems;
    public List<GameObject> P2MenuItems;
    public List<GameObject> P3MenuItems;
    public List<GameObject> P4MenuItems;

    //Player Variables
    public GameObject[] players;
    public bool player1Paused;
    public bool player2Paused;
    public bool player3Paused;
    public bool player4Paused;
    public int p1Cycling;
    public int p2Cycling;
    public int p3Cycling;
    public int p4Cycling;
    public int P1CurrentItem;
    public int P2CurrentItem;
    public int P3CurrentItem;
    public int P4CurrentItem;
    public string[] itemNames = new string[4];

    //Items
    public int numOfSharedItems;
    public List<string> sharedItems;
    public List<Sprite> sharedItemSprites;
    public int[] numOfItems = new int[4];
    public List<List<string>> items = new List<List<string>>(4);
    public List<List<Sprite>> itemSprites = new List<List<Sprite>>(4);

    //Resources
    private Sprite[] allItemSprites;
    private Sprite[] numbers;

    //Key Counter
    public SpriteRenderer KeyDigit1;

    private void Awake()
    {
        P1Selector.SetActive(false);
        P2Selector.SetActive(false);
        P3Selector.SetActive(false);
        P4Selector.SetActive(false);
        allItemSprites = Resources.LoadAll<Sprite>("Object Sprites");
        numbers = Resources.LoadAll<Sprite>("Numbers");
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(a => a.name).ToArray();
        DefineItems();
        NormalizeItems();
    }

    // Use this for initialization
    void Start()
    {
        if (!UIExists)
        {
            UIExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(go => go.name).ToArray();
    }
    // Update is called once per frame
    void Update()
    {
        KeyDigit1.sprite = numbers[PersistentManager.Instance.keys];
        if (items[0].Count > 0)
            itemNames[0] = items[0][P1CurrentItem - 1];
        if (items[1].Count > 0)
            itemNames[1] = items[1][P2CurrentItem - 1];
        if (items[2].Count > 0)
            itemNames[2] = items[2][P3CurrentItem - 1];
        if (items[3].Count > 0)
            itemNames[3] = items[3][P4CurrentItem - 1];
        if (p1Cycling > 0)
            p1Cycling--;
        if (p2Cycling > 0)
            p2Cycling--;
        if (p3Cycling > 0)
            p3Cycling--;
        if (p4Cycling > 0)
            p4Cycling--;
        //P1 Item Selector
        if (player1Paused && p1Cycling == 0 && numOfItems[0] > 1)
        {
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                StartCoroutine(SelectItemRight("P1"));
                p1Cycling = 20;
            }
            if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                StartCoroutine(SelectItemLeft("P1"));
                p1Cycling = 20;
            }
        }
        //P2 Item Selector
        if (player2Paused && p2Cycling == 0 && numOfItems[1] > 1)
        {
            if (Input.GetAxisRaw("Horizontal2") > 0f)
            {
                StartCoroutine(SelectItemRight("P2"));
                p2Cycling = 20;
            }
            if (Input.GetAxisRaw("Horizontal2") < 0f)
            {
                StartCoroutine(SelectItemLeft("P2"));
                p2Cycling = 20;
            }
        }
        //P3 Item Selector
        if (player3Paused && p3Cycling == 0 && numOfItems[2] > 1)
        {
            if (Input.GetAxisRaw("Horizontal3") > 0f)
            {
                StartCoroutine(SelectItemRight("P3"));
                p3Cycling = 20;
            }
            if (Input.GetAxisRaw("Horizontal3") < 0f)
            {
                StartCoroutine(SelectItemLeft("P3"));
                p3Cycling = 20;
            }
        }
        //P4 Item Selector
        if (player4Paused && p4Cycling == 0 && numOfItems[3] > 1)
        {
            if (Input.GetAxisRaw("Horizontal4") > 0f)
            {
                StartCoroutine(SelectItemRight("P4"));
                p4Cycling = 20;
            }
            if (Input.GetAxisRaw("Horizontal4") < 0f)
            {
                StartCoroutine(SelectItemLeft("P4"));
                p4Cycling = 20;
            }
        }

        //Check if UI control is allowed
        if (!PersistentManager.Instance.disableUI)
        {
            //Check if player 1 exists and presses pause
            if (Input.GetButtonDown("Start") && players[0] && players[0].GetComponent<PlayerHealthManager>().invincibility - 20 <= 0)
            {
                //unpause
                if (player1Paused)
                {
                    player1Paused = false;
                    P1Selector.SetActive(false);
                    P1ItemCover.SetActive(true);
                    players[0].GetComponent<PlayerController>().enabled = true;
                }
                //pause
                else
                {
                    player1Paused = true;
                    P1Selector.SetActive(true);
                    P1ItemCover.SetActive(false);
                    players[0].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    players[0].GetComponent<Animator>().SetBool("Moving", false);
                    players[0].GetComponent<PlayerController>().enabled = false;
                }
            }
            //Check if player 2 exists and presses pause
            if (Input.GetButtonDown("Start2") && players[1].activeInHierarchy && players[1].GetComponent<PlayerHealthManager>().invincibility - 20 <= 0)
            {
                //unpause
                if (player2Paused)
                {
                    player2Paused = false;
                    P2Selector.SetActive(false);
                    P2ItemCover.SetActive(true);
                    players[1].GetComponent<PlayerController>().enabled = true;
                }
                //pause
                else
                {
                    player2Paused = true;
                    P2Selector.SetActive(true);
                    P2ItemCover.SetActive(false);
                    players[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    players[1].GetComponent<Animator>().SetBool("Moving", false);
                    players[1].GetComponent<PlayerController>().enabled = false;
                }
            }
            //Check if player 3 exists and presses pause
            if (Input.GetButtonDown("Start3") && players[2].activeInHierarchy && players[2].GetComponent<PlayerHealthManager>().invincibility - 20 <= 0)
            {
                //unpause
                if (player3Paused)
                {
                    player3Paused = false;
                    P3Selector.SetActive(false);
                    P3ItemCover.SetActive(true);
                    players[2].GetComponent<PlayerController>().enabled = true;
                }
                //pause
                else
                {
                    player3Paused = true;
                    P3Selector.SetActive(true);
                    P3ItemCover.SetActive(false);
                    players[2].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    players[2].GetComponent<Animator>().SetBool("Moving", false);
                    players[2].GetComponent<PlayerController>().enabled = false;
                }
            }
            //Check if player 4 exists and presses pause
            if (Input.GetButtonDown("Start4") && players[3].activeInHierarchy && players[3].GetComponent<PlayerHealthManager>().invincibility - 20 <= 0)
            {
                //unpause
                if (player4Paused)
                {
                    player4Paused = false;
                    P4Selector.SetActive(false);
                    P4ItemCover.SetActive(true);
                    players[3].GetComponent<PlayerController>().enabled = true;
                }
                //pause
                else
                {
                    player4Paused = true;
                    P4Selector.SetActive(true);
                    P4ItemCover.SetActive(false);
                    players[3].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    players[3].GetComponent<Animator>().SetBool("Moving", false);
                    players[3].GetComponent<PlayerController>().enabled = false;
                }
            }
        }
    }

    IEnumerator SelectItemLeft(string a)
    {
        if (a == "P1")
        {
            P1MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Underflow(-1, 1)];
            P1MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[0][P1CurrentItem];
            P1MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Underflow(-2, 1)];
            P1MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Overflow(1, 1)];
            if (P1CurrentItem > 1)
                P1CurrentItem--;
            else
                P1CurrentItem += numOfItems[0] - 1;
            animateP1.SetBool("Scroll Left", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP1.SetBool("Scroll Left", false);
        }
        if (a == "P2")
        {
            P2MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Underflow(-1, 2)];
            P2MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[1][P2CurrentItem];
            P2MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Underflow(-2, 2)];
            P2MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Overflow(1, 2)];
            if (P2CurrentItem > 1)
                P2CurrentItem--;
            else
                P2CurrentItem += numOfItems[1] - 1;
            animateP2.SetBool("Scroll Left", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP2.SetBool("Scroll Left", false);
        }
        if (a == "P3")
        {
            P3MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Underflow(-1, 3)];
            P3MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[2][P3CurrentItem];
            P3MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Underflow(-2, 3)];
            P3MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Overflow(1, 3)];
            if (P3CurrentItem > 1)
                P3CurrentItem--;
            else
                P3CurrentItem += numOfItems[2] - 1;
            animateP3.SetBool("Scroll Left", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP3.SetBool("Scroll Left", false);
        }
        if (a == "P4")
        {
            P4MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Underflow(-1, 4)];
            P4MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[3][P4CurrentItem];
            P4MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Underflow(-2, 4)];
            P4MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Overflow(1, 4)];
            if (P4CurrentItem > 1)
                P4CurrentItem--;
            else
                P4CurrentItem += numOfItems[3] - 1;
            animateP4.SetBool("Scroll Left", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP4.SetBool("Scroll Left", false);
        }
    }

    IEnumerator SelectItemRight(string a)
    {
        if (a == "P1")
        {
            P1MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[0][P1CurrentItem];
            P1MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Overflow(1, 1)];
            P1MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Underflow(-1, 1)];
            P1MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Overflow(2, 1)];
            if (P1CurrentItem < numOfItems[0])
                P1CurrentItem++;
            else
                P1CurrentItem -= numOfItems[0] - 1;
            animateP1.SetBool("Scroll Right", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP1.SetBool("Scroll Right", false);
        }
        if (a == "P2")
        {
            P2MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[1][P2CurrentItem];
            P2MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Overflow(1, 2)];
            P2MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Underflow(-1, 2)];
            P2MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Overflow(2, 2)];
            if (P2CurrentItem < numOfItems[1])
                P2CurrentItem++;
            else
                P2CurrentItem -= numOfItems[1] - 1;
            animateP2.SetBool("Scroll Right", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP2.SetBool("Scroll Right", false);
        }
        if (a == "P3")
        {
            P3MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[2][P3CurrentItem];
            P3MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Overflow(1, 3)];
            P3MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Underflow(-1, 3)];
            P3MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Overflow(2, 3)];
            if (P3CurrentItem < numOfItems[2])
                P3CurrentItem++;
            else
                P3CurrentItem -= numOfItems[2] - 1;
            animateP3.SetBool("Scroll Right", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP3.SetBool("Scroll Right", false);
        }
        if (a == "P4")
        {
            P4MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[3][P4CurrentItem];
            P4MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Overflow(1, 4)];
            P4MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Underflow(-1, 4)];
            P4MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Overflow(2, 4)];
            if (P4CurrentItem < numOfItems[3])
                P4CurrentItem++;
            else
                P4CurrentItem -= numOfItems[3] - 1;
            animateP4.SetBool("Scroll Right", true);
            yield return StartCoroutine(WaitFor.Frames(10));
            animateP4.SetBool("Scroll Right", false);
        }
    }

    int Underflow(int a, int p)
    {
        if (p == 1)
        {
            if (P1CurrentItem + a > 0)
                return P1CurrentItem + a;
            else
                return P1CurrentItem + a + numOfItems[0];
        }
        if (p == 2)
        {
            if (P2CurrentItem + a > 0)
                return P2CurrentItem + a;
            else
                return P2CurrentItem + a + numOfItems[1];
        }
        if (p == 3)
        {
            if (P3CurrentItem + a > 0)
                return P3CurrentItem + a;
            else
                return P3CurrentItem + a + numOfItems[2];
        }
        if (p == 4)
        {
            if (P4CurrentItem + a > 0)
                return P4CurrentItem + a;
            else
                return P4CurrentItem + a + numOfItems[3];
        }
        else
        {
            Debug.Log("218: Bad input in underflow");
            return 400;
        }
    }

    int Overflow(int a, int p)
    {
        if (p == 1)
        {
            if (P1CurrentItem + a <= numOfItems[0])
                return P1CurrentItem + a;
            else
                return P1CurrentItem + a - numOfItems[0];
        }
        if (p == 2)
        {
            if (P2CurrentItem + a <= numOfItems[1])
                return P2CurrentItem + a;
            else
                return P2CurrentItem + a - numOfItems[1];
        }
        if (p == 3)
        {
            if (P3CurrentItem + a <= numOfItems[2])
                return P3CurrentItem + a;
            else
                return P3CurrentItem + a - numOfItems[2];
        }
        if (p == 4)
        {
            if (P4CurrentItem + a <= numOfItems[3])
                return P4CurrentItem + a;
            else
                return P4CurrentItem + a - numOfItems[3];
        }
        else
        {
            Debug.Log("242: Bad input in overflow");
            return 400;
        }
    }

    public void DefineItems()
    {
        //Reset Variables
        numOfSharedItems = 0;
        sharedItems.Clear();
        sharedItemSprites.Clear();
        sharedItemSprites.Add(allItemSprites[0]);
        for (int i = 0; i < 4; i++)
        {
            numOfItems[i] = 0;
            items.Add(new List<string>());
            itemSprites.Add(new List<Sprite>());
            items[i].Clear();
            itemSprites[i].Clear();
        }
        //Define Shared Items
        //Boomerang 1 & 2
        if (PersistentManager.Instance.hasBoomerang)
        {
            numOfSharedItems++;
            if (PersistentManager.Instance.hasBoomerang2)
            {
                sharedItems.Add("Boomerang 2");
                sharedItemSprites.Add(allItemSprites[2]);
            }
            else
            {
                sharedItems.Add("Boomerang");
                sharedItemSprites.Add(allItemSprites[1]);
            }
        }
        //1st Bombs
        if (PersistentManager.Instance.hasBombs)
        {
            numOfSharedItems++;
            sharedItems.Add("Bombs");
            sharedItemSprites.Add(allItemSprites[3]);
        }
        //Bow & Silver Arrows
        if (PersistentManager.Instance.hasBow)
        {
            numOfSharedItems++;
            if (PersistentManager.Instance.hasBow2)
            {
                sharedItems.Add("Bow 2");
                sharedItemSprites.Add(allItemSprites[5]);
            }
            else
            {
                sharedItems.Add("Bow 1");
                sharedItemSprites.Add(allItemSprites[4]);
            }
        }
        //Red Candle
        if (PersistentManager.Instance.hasRedCandle)
        {
            numOfSharedItems++;
            sharedItems.Add("Candle 2");
            sharedItemSprites.Add(allItemSprites[7]);
        }
        //Recorder
        if (PersistentManager.Instance.hasFlute)
        {
            numOfSharedItems++;
            sharedItems.Add("Flute");
            sharedItemSprites.Add(allItemSprites[8]);
        }
        //Wand
        if (PersistentManager.Instance.hasWand)
        {
            numOfSharedItems++;
            sharedItems.Add("Wand");
            sharedItemSprites.Add(allItemSprites[13]);
        }
        //Set player specific item list to shared item list
        for (int i = 0; i < 4; i++)
        {
            numOfItems[i] = numOfSharedItems;
            items[i] = new List<string>(sharedItems);
            itemSprites[i] = new List<Sprite>(sharedItemSprites);
        }
        //Order player specific item sprites and names for UI
        for (var i = 0; i < 4; i++)
        {
            //Blue Candle
            if (players[i].GetComponent<PlayerController>().hasCandle && !PersistentManager.Instance.hasRedCandle)
            {
                numOfItems[i]++;
                items[i].Add("Candle");
                itemSprites[i].Add(allItemSprites[6]);
            }
            //Meat
            if (players[i].GetComponent<PlayerController>().hasMeat)
            {
                numOfItems[i]++;
                items[i].Add("Meat");
                itemSprites[i].Add(allItemSprites[9]);
            }
            //Letter and Potions
            if (PersistentManager.Instance.hasLetter)
            {
                //Letter
                if (PersistentManager.Instance.hasLetter && !players[i].GetComponent<PlayerController>().hasPotion1 && !players[i].GetComponent<PlayerController>().hasPotion2)
                {
                    numOfItems[i]++;
                    items[i].Add("Letter");
                    itemSprites[i].Add(allItemSprites[10]);
                }
                //Blue Potion
                if (players[i].GetComponent<PlayerController>().hasPotion1)
                {
                    numOfItems[i]++;
                    items[i].Add("Potion");
                    itemSprites[i].Add(allItemSprites[11]);
                }
                //Red Potion
                if (players[i].GetComponent<PlayerController>().hasPotion2)
                {
                    numOfItems[i]++;
                    items[i].Add("Potion 2");
                    itemSprites[i].Add(allItemSprites[12]);
                }
            }
        }
    }

    //Define initial UI item sprites
    public void NormalizeItems()
    {
        //P1
        //Special case for no items
        if (numOfItems[0] == 0)
        {
            P1MenuItems[0].GetComponent<SpriteRenderer>().sprite = null;
            P1MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P1MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P1MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Special case for 1 item
        if (numOfItems[0] == 1)
        {
            P1MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[0][P1CurrentItem];
            P1MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P1MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P1MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Any amount more than 1 item
        else if (numOfItems[0] > 1)
        {
            P1MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[0][P1CurrentItem];
            P1MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Overflow(1, 1)];
            P1MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Underflow(-1, 1)];
            P1MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[0][Overflow(2, 1)];
        }
        //P2
        //Special case for no items
        if (numOfItems[1] == 0)
        {
            P2MenuItems[0].GetComponent<SpriteRenderer>().sprite = null;
            P2MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P2MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P2MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Special case for 1 item
        if (numOfItems[1] == 1)
        {
            P2MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[1][P2CurrentItem];
            P2MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P2MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P2MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Any amount more than 1 item
        else if (numOfItems[1] > 1)
        {
            P2MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[1][P2CurrentItem];
            P2MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Overflow(1, 2)];
            P2MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Underflow(-1, 2)];
            P2MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[1][Overflow(2, 2)];
        }
        //P3
        //Special case for no items
        if (numOfItems[2] == 0)
        {
            P3MenuItems[0].GetComponent<SpriteRenderer>().sprite = null;
            P3MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P3MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P3MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Special case for 1 item
        if (numOfItems[2] == 1)
        {
            P3MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[2][P3CurrentItem];
            P3MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P3MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P3MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Any amount more than 1 item
        else if (numOfItems[2] > 1)
        {
            P3MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[2][P3CurrentItem];
            P3MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Overflow(1, 3)];
            P3MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Underflow(-1, 3)];
            P3MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[2][Overflow(2, 3)];
        }
        //P4
        //Special case for no items
        if (numOfItems[3] == 0)
        {
            P4MenuItems[0].GetComponent<SpriteRenderer>().sprite = null;
            P4MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P4MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P4MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Special case for 1 item
        if (numOfItems[3] == 1)
        {
            P4MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[3][P4CurrentItem];
            P4MenuItems[1].GetComponent<SpriteRenderer>().sprite = null;
            P4MenuItems[2].GetComponent<SpriteRenderer>().sprite = null;
            P4MenuItems[3].GetComponent<SpriteRenderer>().sprite = null;
        }
        //Any amount more than 1 item
        else if (numOfItems[3] > 1)
        {
            P4MenuItems[0].GetComponent<SpriteRenderer>().sprite = itemSprites[3][P4CurrentItem];
            P4MenuItems[1].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Overflow(1, 4)];
            P4MenuItems[2].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Underflow(-1, 4)];
            P4MenuItems[3].GetComponent<SpriteRenderer>().sprite = itemSprites[3][Overflow(2, 4)];
        }
    }

    public void DisplaySharedItems()
    {
        LoadingManager[] items = GameObject.Find("Shared Items").GetComponentsInChildren<LoadingManager>();
        for (int i = 0; i < items.Length; i++)
            items[i].ReverseLoadUiItems();
    }
}