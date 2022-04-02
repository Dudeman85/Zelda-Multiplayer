using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{

    //Movement
    public float moveSpeed;
    private bool moving;
    public int attacking;
    public bool thrusting;
    public bool attackingAnim;
    private bool APressed;
    private bool BPressed;
    public int BInUse;
    public bool levelTransitionsEnabled = true;
    public int facing = 1; //Up = 1, Left = 2, Down = 3, Right = 4
    public Vector2Int facingVectors;
    public Vector2 lastMove = new Vector2(0f, 1f);
    private Vector2 moveVectors;
    public int movementPaused;
    public bool ladderInUse;
    public bool dead;

    //Items
    public int bombs;
    public int maxBombs;
    public int rupees;
    public bool hasPotion1;
    public bool hasPotion2;
    public bool hasRing1;
    public bool hasMagicalShield;
    public bool hasArrows;
    public bool hasMeat;
    public bool hasCandle;

    //Components
    private Animator anim;
    private GameObject cam;
    public Rigidbody2D rb;
    private UIController UIData;
    public GameObject sword;
    public GameObject[] players;

    //Recources
    public Sprite[] allItemSprites;
    public Sprite[] numbers;
    public Sprite[] swords;
    public RuntimeAnimatorController smallShield;
    public RuntimeAnimatorController largeShield;

    //UI
    public SpriteRenderer rupeeDigit1;
    public SpriteRenderer rupeeDigit2;
    public SpriteRenderer rupeeDigit3;
    public SpriteRenderer bombDigit1;
    public SpriteRenderer bombDigit2;

    //Exists
    public string player;
    private int p;
    private static bool[] exists = new bool[4];

    //Prefabs
    public GameObject swordBeam;
    private GameObject beamClone;
    public GameObject tornado;
    public GameObject bomb;
    public GameObject fire;
    public GameObject meat;
    public GameObject arrow;
    public GameObject magicBeam;
    public GameObject letter;
    private BoomerangController boomerang;
    private GameObject arrowClone;
    private GameObject magicBeamClone;
    public bool magicBeamExists;
    private GameObject meatClone;
    private static bool meatExists;
    public GameObject[] fireClones;
    private int fires = -1;

    //Initialize on scene load
    void Start()
    {
        p = int.Parse(player) - 1;
        if (player == "1")
            player = null;
        //Check if already exists
        if (!exists[p])
        {
            exists[p] = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Initialize Variables
        players = GameObject.FindGameObjectsWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        boomerang = GetComponentInChildren<BoomerangController>();
        allItemSprites = Resources.LoadAll<Sprite>("Object Sprites");
        numbers = Resources.LoadAll<Sprite>("Numbers");
        UIData = GameObject.Find("UI").GetComponent<UIController>();
        //Animator
        if (hasMagicalShield)
            GetComponent<Animator>().runtimeAnimatorController = largeShield;
        else
            GetComponent<Animator>().runtimeAnimatorController = smallShield;
        //UI Counters
        RupeeCounter(rupees);
        BombCounter(bombs);
    }

    private void OnEnable()
    {
        //Animator
        if (!dead)
        {
            if (hasMagicalShield)
                GetComponent<Animator>().runtimeAnimatorController = largeShield;
            else
                GetComponent<Animator>().runtimeAnimatorController = smallShield;
        }
        attackingAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical4") != 0)
            Debug.Log("aa");
        //Player Rendering Order
        players = players.OrderByDescending(t => t.transform.position.y).ToArray();
        for (var i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<SpriteRenderer>().sortingOrder = i * 2;
            players[i].GetComponent<PlayerController>().sword.GetComponent<SpriteRenderer>().sortingOrder = i * 2 - 1;
        }
        //Attack Timer
        if (attacking > 0)
            attacking--;
        if (BInUse > 0)
            BInUse--;
        if (movementPaused > 0)
        {
            movementPaused--;
            rb.velocity = Vector2.zero;
        }
        //Player Movement
        moving = false;
        moveVectors.x = Mathf.Round(Input.GetAxisRaw("Horizontal" + player));
        moveVectors.y = Mathf.Round(Input.GetAxisRaw("Vertical" + player));
        if (attacking <= 0 && movementPaused == 0)
        {
            //Horizontal Movement
            //Move Right
            if (Input.GetAxisRaw("Horizontal" + player) > 0f && Mathf.Abs(Input.GetAxisRaw("Horizontal" + player)) > Mathf.Abs(Input.GetAxisRaw("Vertical" + player)))
            {
                rb.velocity = new Vector2(10 * moveSpeed * Time.deltaTime, 0f);
                moving = true;
                lastMove = new Vector2(1f, 0f);
                facing = 4;
                facingVectors = new Vector2Int(1, 0);
            }
            //Move Left
            if (Input.GetAxisRaw("Horizontal" + player) < 0f && Mathf.Abs(Input.GetAxisRaw("Horizontal" + player)) > Mathf.Abs(Input.GetAxisRaw("Vertical" + player)))
            {
                rb.velocity = new Vector2(-10 * moveSpeed * Time.deltaTime, 0f);
                moving = true;
                lastMove = new Vector2(-1f, 0f);
                facing = 2;
                facingVectors = new Vector2Int(-1, 0);
            }
            //Not Moving
            if (Input.GetAxisRaw("Horizontal" + player) == 0)
            {
                rb.velocity = new Vector2(0f, rb.velocity.x);
            }
            //Vertical Movement
            //Move Up
            if (Input.GetAxisRaw("Vertical" + player) > 0f && Mathf.Abs(Input.GetAxisRaw("Horizontal" + player)) < Mathf.Abs(Input.GetAxisRaw("Vertical" + player)))
            {
                rb.velocity = new Vector2(0f, 10 * moveSpeed * Time.deltaTime);
                moving = true;
                lastMove = new Vector2(0f, 1f);
                facing = 1;
                facingVectors = new Vector2Int(0, 1);
            }
            //Move Down
            if (Input.GetAxisRaw("Vertical" + player) < 0f && Mathf.Abs(Input.GetAxisRaw("Horizontal" + player)) < Mathf.Abs(Input.GetAxisRaw("Vertical" + player)))
            {
                rb.velocity = new Vector2(0f, -10 * moveSpeed * Time.deltaTime);
                moving = true;
                lastMove = new Vector2(0f, -1f);
                facing = 3;
                facingVectors = new Vector2Int(0, -1);
            }
            //Not Moving
            if (Input.GetAxisRaw("Vertical" + player) == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }
        //Attacking
        if (Input.GetAxisRaw("Attack" + player) == 1 && PersistentManager.Instance.hasSword1)
        {
            if (attacking == 0 && !APressed)
            {
                if (PersistentManager.Instance.hasSword1)
                {
                    sword.GetComponent<SpriteRenderer>().sprite = swords[0];
                    sword.GetComponent<HitEnemy>().damage = 2;
                    swordBeam.GetComponent<HitEnemy>().damage = 2;
                }
                if (PersistentManager.Instance.hasSword2)
                {
                    sword.GetComponent<SpriteRenderer>().sprite = swords[1];
                    sword.GetComponent<HitEnemy>().damage = 4;
                    swordBeam.GetComponent<HitEnemy>().damage = 4;
                }
                if (PersistentManager.Instance.hasSword3)
                {
                    sword.GetComponent<SpriteRenderer>().sprite = swords[2];
                    sword.GetComponent<HitEnemy>().damage = 8;
                    swordBeam.GetComponent<HitEnemy>().damage = 8;
                }
                if (GetComponent<PlayerHealthManager>().health >= PersistentManager.Instance.maxHealth - 1 && !beamClone)
                {
                    FindObjectOfType<SFXManager>().PlaySound("Sword Beam");
                    beamClone = Instantiate(swordBeam, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), transform.rotation);
                    beamClone.GetComponent<Rigidbody2D>().velocity = lastMove * 1.5f;
                    if (facing == 4)
                        beamClone.transform.eulerAngles = new Vector3(0, 0, 90);
                    if (facing == 2)
                        beamClone.transform.eulerAngles = new Vector3(0, 0, 270);
                    if (facing == 1)
                        beamClone.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                FindObjectOfType<SFXManager>().PlaySound("Sword Slash");
                attacking = 20;
                attackingAnim = true;
                rb.velocity = new Vector2(0f, 0f);
                StartCoroutine(Attack());
                APressed = true;
            }
        }
        if (Input.GetAxisRaw("Attack" + player) == 0)
        {
            APressed = false;
        }
        //B Item
        if (Input.GetAxisRaw("Item" + player) == 1 && BInUse == 0 && !BPressed && attacking == 0)
        {
            BPressed = true;
            //Blocked in grotto
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Grotto"))
            {
                //Boomerang
                if (UIData.itemNames[p] == "Boomerang" || UIData.itemNames[p] == "Boomerang 2")
                {
                    StartCoroutine(BItem());
                    BInUse = 120;
                    movementPaused = 18;
                    thrusting = true;
                    boomerang.GetComponent<SpriteRenderer>().enabled = true;
                    if (!moving || moveVectors.x == 0 && moveVectors.y == 0)
                        boomerang.Throw(lastMove);
                    else
                    {
                        boomerang.Throw(moveVectors);
                    }
                }
                //Bombs
                if (UIData.itemNames[p] == "Bombs" && bombs > 0)
                {
                    FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
                    StartCoroutine(BItem());
                    BInUse = 85;
                    movementPaused = 18;
                    thrusting = true;
                    bombs--;
                    BombCounter(bombs);
                    Instantiate(bomb, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), transform.rotation);
                }
                //Bow
                if (UIData.itemNames[p] == "Bow" || UIData.itemNames[p] == "Bow 2" && hasArrows && rupees > 0)
                {
                    FindObjectOfType<SFXManager>().PlaySound("Arrow Boomerang");
                    AddRupees(-1);
                    StartCoroutine(BItem());
                    BInUse = 300;
                    movementPaused = 18;
                    thrusting = true;
                    if (facing == 1)
                        arrowClone = Instantiate(arrow, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), Quaternion.Euler(0, 0, 0));
                    if (facing == 2)
                        arrowClone = Instantiate(arrow, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), Quaternion.Euler(0, 0, 90));
                    if (facing == 3)
                        arrowClone = Instantiate(arrow, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), Quaternion.Euler(0, 0, 180));
                    if (facing == 4)
                        arrowClone = Instantiate(arrow, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), Quaternion.Euler(0, 0, 270));
                    arrowClone.GetComponent<Rigidbody2D>().velocity = new Vector2(lastMove.x * 2, lastMove.y * 2);
                    arrowClone.GetComponent<ArrowController>().player = gameObject;
                }
                //Candle
                if (fires < 1)
                {
                    //Blue Candle
                    if (UIData.itemNames[p] == "Candle" && !PersistentManager.Instance.fireUsedInRoom)
                    {
                        StartCoroutine(BItem());
                        BInUse = 30;
                        fires++;
                        movementPaused = 18;
                        thrusting = true;
                        PersistentManager.Instance.fireUsedInRoom = true;
                        FindObjectOfType<SFXManager>().PlaySound("Candle");
                        fireClones[fires] = Instantiate(fire, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 5, 0), transform.rotation);
                        fireClones[fires].GetComponent<Rigidbody2D>().velocity = new Vector2(lastMove.x / 2, lastMove.y / 2);
                        StartCoroutine(Fire(fireClones[fires]));
                    }
                    //Red Candle
                    if (UIData.itemNames[p] == "Candle 2")
                    {
                        StartCoroutine(BItem());
                        BInUse = 30;
                        fires++;
                        movementPaused = 18;
                        thrusting = true;
                        fireClones[fires] = Instantiate(fire, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 5, 0), transform.rotation);
                        fireClones[fires].GetComponent<Rigidbody2D>().velocity = new Vector2(lastMove.x / 2, lastMove.y / 2);
                        StartCoroutine(Fire(fireClones[fires]));
                    }
                }
                //Flute
                if (UIData.itemNames[p] == "Flute")
                {
                    BInUse = 100;
                    movementPaused = 100;
                    FindObjectOfType<SFXManager>().PlaySound("Recorder");
                    if (PersistentManager.Instance.playerLocation == new Vector2(3, 5) && !PersistentManager.Instance.coversCleared.Contains(new Vector3Int(3, 5, 0)))
                    {
                        PersistentManager.Instance.coversCleared.Add(new Vector3Int(3, 5, 0));
                        Destroy(GameObject.Find("Level 7 Cover"));
                    }
                    else if (PersistentManager.Instance.levelsCompleted.Count > 0)
                        Instantiate(tornado, new Vector3(cam.transform.position.x - 1.17f, transform.position.y, 0), transform.rotation);
                }
                //Meat
                if (UIData.itemNames[p] == "Meat" && !meatExists && hasMeat)
                {
                    StartCoroutine(BItem());
                    BInUse = 30;
                    movementPaused = 18;
                    thrusting = true;
                    meatExists = true;
                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Dungeon") && PersistentManager.Instance.playerLocation == new Vector2(1, 11))
                    {
                        meatClone = Instantiate(meat, new Vector3(-19.2f, -4.45f, 0), transform.rotation);
                        GameObject.Find("Text Cover A11").GetComponent<SpriteRenderer>().enabled = true;
                        Destroy(GameObject.Find("A11"));
                        PersistentManager.Instance.A11Done = true;
                    }
                    else
                    {
                        meatClone = Instantiate(meat, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), transform.rotation);
                        StartCoroutine(Meat());
                    }
                }
                //Magic Wand
                if (UIData.itemNames[p] == "Wand")
                {
                    StartCoroutine(Attack());
                    BInUse = 16;
                    movementPaused = 18;
                    attackingAnim = true;
                    sword.GetComponent<SpriteRenderer>().sprite = allItemSprites[14];
                    if (!magicBeamExists)
                    {
                        magicBeamExists = true;
                        magicBeamClone = Instantiate(magicBeam, new Vector3(transform.position.x + lastMove.x / 6, transform.position.y + lastMove.y / 6, 0), transform.rotation);
                        magicBeamClone.GetComponent<Animator>().SetFloat("DirectionX", lastMove.x);
                        magicBeamClone.GetComponent<Animator>().SetFloat("DirectionY", lastMove.y);
                        magicBeamClone.GetComponent<Rigidbody2D>().velocity = lastMove;
                        magicBeamClone.GetComponent<MagicController>().player = gameObject;
                    }
                }
            }
            //Letter
            if (UIData.itemNames[p] == "Letter" && !PersistentManager.Instance.letterDelivered)
            {
                if ("D4E1E7H3I8L5N1".Contains(PersistentManager.Instance.spawnpoint) && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Grotto"))
                {
                    for (var i = 0; i < FindObjectsOfType<LoadingManager>().Length; i++)
                    {
                        if (FindObjectsOfType<LoadingManager>()[i].name == "Potion")
                        {
                            FindObjectsOfType<LoadingManager>()[i].GetComponent<BoxCollider2D>().enabled = true;
                            FindObjectsOfType<LoadingManager>()[i].GetComponent<SpriteRenderer>().enabled = true;
                        }
                        if (FindObjectsOfType<LoadingManager>()[i].name == "Prices")
                            FindObjectsOfType<LoadingManager>()[i].GetComponent<SpriteRenderer>().enabled = true;
                    }
                    PersistentManager.Instance.letterDelivered = true;
                    StartCoroutine(GameObject.Find(PersistentManager.Instance.spawnpoint).GetComponent<PlayerSpawn>().text());
                    StartCoroutine(Letter(Instantiate(letter, cam.transform.position - new Vector3(0, 0.24f), Quaternion.identity)));
                }
            }
            if (GetComponent<PlayerHealthManager>().health != PersistentManager.Instance.maxHealth)
            {
                //Blue Potion
                if (UIData.itemNames[p] == "Potion")
                {
                    PersistentManager.Instance.disablePausing = true;
                    StartCoroutine(GetComponent<PlayerHealthManager>().Heal(gameObject, 64));
                    hasPotion1 = false;
                    UIData.DefineItems();
                    UIData.NormalizeItems();
                }
                //Red Potion
                if (UIData.itemNames[p] == "Potion 2")
                {
                    PersistentManager.Instance.disablePausing = true;
                    StartCoroutine(GetComponent<PlayerHealthManager>().Heal(gameObject, 64));
                    hasPotion1 = true;
                    hasPotion2 = false;
                    UIData.DefineItems();
                    UIData.NormalizeItems();
                }
            }
        }
        if (Input.GetAxisRaw("Item") == 0)
        {
            BPressed = false;
        }
        //Set Animations
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal" + player));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical" + player));
        anim.SetBool("Moving", moving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        anim.SetBool("Attacking", attackingAnim);
        anim.SetBool("Thrusting", thrusting);
    }

    //Functions
    //Add Rupees
    public void AddRupees(int amount)
    {
        StartCoroutine(CountRupees(amount));
    }
    //Rupee Increments
    IEnumerator CountRupees(int amount)
    {
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<PlayerController>().levelTransitionsEnabled = false;
        FindObjectOfType<SFXManager>().PlaySound("Refill Health", Mathf.Abs(amount) * 2);
        while (Mathf.Abs(amount) > 0)
        {
            if (amount < 0)
            {
                rupees--;
                RupeeCounter(rupees);
                amount++;
            }
            else
            {
                rupees++;
                RupeeCounter(rupees);
                amount--;
            }
            yield return WaitFor.Frames(4);
        }
        FindObjectOfType<SFXManager>().PlaySound("Text");
        for (int i = 0; i < players.Length; i++)
            players[i].GetComponent<PlayerController>().levelTransitionsEnabled = true;
    }
    //Rupee Value Display
    public void RupeeCounter(int value)
    {
        rupees = value;
        rupeeDigit2.enabled = true;
        rupeeDigit3.enabled = true;
        char[] valueString;
        if (value < 10)
        {
            rupeeDigit1.sprite = numbers[value];
            rupeeDigit2.enabled = false;
            rupeeDigit3.enabled = false;
        }
        if (value < 100 && value >= 10)
        {
            valueString = value.ToString().ToCharArray();
            rupeeDigit1.sprite = numbers[int.Parse(valueString[0].ToString())];
            rupeeDigit2.sprite = numbers[int.Parse(valueString[1].ToString())];
            rupeeDigit3.enabled = false;
        }
        if (value >= 100)
        {
            valueString = value.ToString().ToCharArray();
            rupeeDigit1.sprite = numbers[int.Parse(valueString[0].ToString())];
            rupeeDigit2.sprite = numbers[int.Parse(valueString[1].ToString())];
            rupeeDigit3.sprite = numbers[int.Parse(valueString[2].ToString())];
        }
    }
    //Bomb Counter
    public void AddBombs(int amount)
    {
        bombs += amount;
        BombCounter(bombs);
    }
    //Bomb Value Display
    public void BombCounter(int value)
    {
        bombs = value;
        char[] valueString;
        bombDigit2.enabled = true;
        if (value < 10)
        {
            bombDigit1.sprite = numbers[value];
            bombDigit2.enabled = false;
        }
        if (value >= 10)
        {
            valueString = value.ToString().ToCharArray();
            bombDigit1.sprite = numbers[int.Parse(valueString[0].ToString())];
            bombDigit2.sprite = numbers[int.Parse(valueString[1].ToString())];
        }
    }
    //Attack animation countdown timer
    IEnumerator Attack()
    {
        yield return StartCoroutine(WaitFor.Frames(11));
        attackingAnim = false;
    }
    //B Item Timer
    public IEnumerator BItem()
    {
        yield return StartCoroutine(WaitFor.Frames(14));
        thrusting = false;
    }
    //Fire Timer
    IEnumerator Fire(GameObject fire)
    {
        yield return StartCoroutine(WaitFor.Frames(15));
        fire.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        fire.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        fire.GetComponents<BoxCollider2D>()[0].enabled = true;
        yield return StartCoroutine(WaitFor.Frames(60));
        Destroy(fire);
        fires--;
    }
    //Meat Timer
    IEnumerator Meat()
    {
        yield return StartCoroutine(WaitFor.Frames(700));
        Destroy(meatClone);
        meatExists = false;
    }
    IEnumerator Letter(GameObject clone)
    {
        yield return StartCoroutine(WaitFor.Frames(180));
        Destroy(clone);
    }
}