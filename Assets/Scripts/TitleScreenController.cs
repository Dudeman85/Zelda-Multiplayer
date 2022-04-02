using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class SavedSettings
{
    public int[] playerChars;
    public int numPlayers;
}

public class TitleScreenController : MonoBehaviour
{
    public GameObject[] allPlayers = new GameObject[4];
    public GameObject[] players;
    private GameObject cam;
    private GameObject ui;

    //wtf
    public int menuScreen;
    public int selectedOption;
    public int P2SelectedOption;
    public int P3SelectedOption;
    public int P4SelectedOption;
    public int options;
    public bool selecting;
    public bool P2Selecting;
    public bool P3Selecting;
    public bool P4Selecting;
    public int[] playerCharacters;
    public SpriteRenderer P1CharacterSprite;
    public SpriteRenderer P2CharacterSprite;
    public SpriteRenderer P3CharacterSprite;
    public SpriteRenderer P4CharacterSprite;
    public RuntimeAnimatorController[] animControllers;
    public RuntimeAnimatorController[] anim2Controllers;
    public RuntimeAnimatorController[] ghostAnims;
    public Sprite[] sword1Sprites;
    public Sprite[] sword2Sprites;
    public Sprite[] sword3Sprites;
    public Sprite[] sword4Sprites;
    public GameObject[] swordBeams;
    public RuntimeAnimatorController[] boomerangAnimControllers;
    public SpriteRenderer[] P1Input;
    public SpriteRenderer[] P2Input;
    public SpriteRenderer[] P3Input;
    public SpriteRenderer[] P4Input;
    public Sprite[] characterSprites;
    public bool selectingLetter;
    public bool inTitleCrawl = true;
    public Vector2Int selectedLetter;
    public List<string> text = new List<string>();
    public int letterInsertPoint;
    public GameObject UI;
    public SpriteRenderer fadeToBlack;
    public GameObject fileDisplays;
    public GameObject fileScreen;
    public GameObject settingsSprites;
    public Sprite fileSelection;
    public Sprite fileNaming;
    public Sprite fileDeletion;
    public Sprite settings;
    public GameObject selector;
    public GameObject P2Selector;
    public GameObject P3Selector;
    public GameObject P4Selector;
    public GameObject letterSelector;
    public GameObject letterPointer;
    public GameObject playerNumPointer;
    public Text file1Name;
    public Slider file1HeartCover;
    public Slider file1HeartCover2;
    public Text file2Name;
    public Slider file2HeartCover;
    public Slider file2HeartCover2;
    public Text file3Name;
    public Slider file3HeartCover;
    public Slider file3HeartCover2;
    public GameObject[] playerUIs;
    public Texture2D[] textures;
    public Color32[] colors = { new Color32(128, 208, 16, 255), new Color32(184, 184, 248, 255), new Color32(47, 31, 5, 255), new Color32(26, 26, 26, 255), new Color32(128, 208, 16, 255), new Color32(27, 58, 141, 255), new Color32(184, 184, 248, 255), new Color32(255, 255, 255, 255) };

    //Map Vars
    private GameObject map;
    private GameObject overworldMap;
    private GameObject posMarker;
    private GameObject roomMarker;
    private GameObject doorMarker;
    private SpriteRenderer overworldPointer;
    private SpriteRenderer dungeonPointer;
    private GameObject dungeonCompassPointer;

    // Use this for initialization
    void Start()
    {
        //Map Vars
        map = GameObject.Find("Map Data");
        posMarker = GameObject.Find("Dungeon Position Marker");
        overworldMap = GameObject.Find("Menu Map");
        overworldPointer = GameObject.Find("Position Marker").GetComponent<SpriteRenderer>();
        dungeonPointer = GameObject.Find("Dungeon Position Marker").GetComponent<SpriteRenderer>();
        dungeonCompassPointer = GameObject.Find("Dungeon Compass Marker");
        roomMarker = Resources.Load("Map Room") as GameObject;
        doorMarker = Resources.Load("Map Door") as GameObject;
        playerUIs[0] = GameObject.Find("P2 UI");
        playerUIs[1] = GameObject.Find("P3 UI");
        playerUIs[2] = GameObject.Find("P4 UI");

        //Persistent Object Vars
        for (int i = 0; i < 4; i++)
            allPlayers[i] = Resources.FindObjectsOfTypeAll<PlayerController>()[i].gameObject;
        allPlayers = allPlayers.OrderBy(p => p.name).ToArray();
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(p => p.name).ToArray();
        cam = GameObject.Find("Camera");
        ui = GameObject.Find("UI");
        if (cam)
        {
            cam.SetActive(false);
            ui.SetActive(false);
            for (int i = 0; i < players.Length; i++)
                players[i].SetActive(false);
        }
        //Current UI Vars
        letterPointer.GetComponent<Animator>().enabled = false;
        letterPointer.GetComponent<SpriteRenderer>().enabled = false;
        fileScreen.GetComponent<SpriteRenderer>().sprite = fileSelection;
        //Set initial UI variables (Health & Name)
        if (SaveSystem.LoadGame("/save1.despacito") != null)
        {
            GameData file1 = SaveSystem.LoadGame("/save1.despacito");
            file1Name.text = file1.fileName;
            if (file1.maxHealth < 32)
            {
                file1HeartCover.value = 8 - file1.maxHealth / 4;
                file1HeartCover2.value = 8;
            }
            else
            {
                file1HeartCover.value = 8;
                file1HeartCover2.value = 8 - file1.maxHealth / 4 - 8;
            }
        }
        if (SaveSystem.LoadGame("/save2.despacito") != null)
        {
            GameData file2 = SaveSystem.LoadGame("/save2.despacito");
            file2Name.text = file2.fileName;
            if (file2.maxHealth < 32)
            {
                file2HeartCover.value = 8 - file2.maxHealth / 4;
                file2HeartCover2.value = 8;
            }
            else
            {
                file2HeartCover.value = 8;
                file2HeartCover2.value = 8 - file2.maxHealth / 4 - 8;
            }
        }
        if (SaveSystem.LoadGame("/save3.despacito") != null)
        {
            GameData file3 = SaveSystem.LoadGame("/save3.despacito");
            file3Name.text = file3.fileName;
            if (file3.maxHealth < 32)
            {
                file3HeartCover.value = 8 - file3.maxHealth / 4;
                file3HeartCover2.value = 8;
            }
            else
            {
                file3HeartCover.value = 8;
                file3HeartCover2.value = file3.maxHealth / 4 - 8;
            }

        }
        selector.transform.localPosition = new Vector2(-185.4f, 80f);
        fileDisplays.transform.localPosition = new Vector2(0, 79);
        settingsSprites.SetActive(false);
        UI.SetActive(false);
        StartCoroutine(TextCrawl());
    }

    // Update is called once per frame
    void Update()
    {
        //Exit title crawl
        if (Input.GetAxisRaw("Start") == 1 && inTitleCrawl)
        {
            inTitleCrawl = false;
            selecting = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            transform.position = new Vector3(4, 0, -1);
            UI.SetActive(true);
            FindObjectOfType<MusicManager>().ChangeSong("none");
            StopAllCoroutines();
        }
        //Return Inputs to neutral
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Select") == 0 && Input.GetAxisRaw("Start") == 0)
            selecting = false;
        if (Input.GetAxisRaw("Vertical2") == 0 && Input.GetAxisRaw("Horizontal2") == 0 && Input.GetAxisRaw("Select2") == 0 && Input.GetAxisRaw("Start2") == 0)
            P2Selecting = false;
        if (Input.GetAxisRaw("Vertical3") == 0 && Input.GetAxisRaw("Horizontal3") == 0 && Input.GetAxisRaw("Select3") == 0 && Input.GetAxisRaw("Start3") == 0)
            P3Selecting = false;
        if (Input.GetAxisRaw("Vertical4") == 0 && Input.GetAxisRaw("Horizontal4") == 0 && Input.GetAxisRaw("Select4") == 0 && Input.GetAxisRaw("Start4") == 0)
            P4Selecting = false;
        if (Input.GetAxisRaw("Attack") == 0 && Input.GetAxisRaw("Item") == 0)
            selectingLetter = false;
        //Menu scrolling and selection
        if (!inTitleCrawl && !selecting && menuScreen != 4)
        {
            if (Input.GetAxisRaw("Start") == 1)
                SelectOption();
            if (menuScreen != 2)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                    SelectUp();
                if (Input.GetAxisRaw("Vertical") < 0)
                    SelectDown();
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Abs(Input.GetAxisRaw("Vertical")))
                    NameSelectUp();
                if (Input.GetAxisRaw("Vertical") < 0 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < Mathf.Abs(Input.GetAxisRaw("Vertical")))
                    NameSelectDown();
                if (Input.GetAxisRaw("Horizontal") > 0 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > Mathf.Abs(Input.GetAxisRaw("Vertical")))
                    NameSelectRight();
                if (Input.GetAxisRaw("Horizontal") < 0 && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > Mathf.Abs(Input.GetAxisRaw("Vertical")))
                    NameSelectLeft();
            }
            if (Input.GetAxisRaw("Select") == 1)
                SelectDown();
        }
        //Name Entry Letter Selection
        if (Input.GetAxisRaw("Attack") == 1 && !selectingLetter && menuScreen == 2)
            SelectLetter();
        if (Input.GetAxisRaw("Item") == 1 && !selectingLetter && menuScreen == 2)
            DeleteLetter();
        //Settings Scrolling
        if (menuScreen == 4)
        {
            if (Input.GetAxisRaw("Start") == 1)
                SelectOption();
            if (Input.GetAxisRaw("Select") == 1)
                SettingsSelection(1);
            if (Input.GetAxisRaw("Select2") == 1)
                SettingsSelection(2);
            if (Input.GetAxisRaw("Select3") == 1)
                SettingsSelection(3);
            if (Input.GetAxisRaw("Select4") == 1)
                SettingsSelection(4);
            if (Input.GetAxisRaw("Horizontal") < 0)
                SettingsSelectLeft(1);
            if (Input.GetAxisRaw("Horizontal") > 0)
                SettingsSelectRight(1);
            if (Input.GetAxisRaw("Horizontal2") < 0)
                SettingsSelectLeft(2);
            if (Input.GetAxisRaw("Horizontal2") > 0)
                SettingsSelectRight(2);
            if (Input.GetAxisRaw("Horizontal3") < 0)
                SettingsSelectLeft(3);
            if (Input.GetAxisRaw("Horizontal3") > 0)
                SettingsSelectRight(3);
            if (Input.GetAxisRaw("Horizontal4") < 0)
                SettingsSelectLeft(4);
            if (Input.GetAxisRaw("Horizontal4") > 0)
                SettingsSelectRight(4);
        }

        //Input listener and display
        if (menuScreen == 4)
        {
            //P1 Input
            if (selectedOption == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    P1Input[i].enabled = false;
                }
                if (Input.GetAxisRaw("Vertical") > 0)
                    P1Input[0].enabled = true;
                if (Input.GetAxisRaw("Vertical") < 0)
                    P1Input[1].enabled = true;
                if (Input.GetAxisRaw("Horizontal") < 0)
                    P1Input[2].enabled = true;
                if (Input.GetAxisRaw("Horizontal") > 0)
                    P1Input[3].enabled = true;
                if (Input.GetAxisRaw("Select") == 1)
                    P1Input[4].enabled = true;
                if (Input.GetAxisRaw("Start") == 1)
                    P1Input[5].enabled = true;
                if (Input.GetAxisRaw("Item") == 1)
                    P1Input[6].enabled = true;
                if (Input.GetAxisRaw("Attack") == 1)
                    P1Input[7].enabled = true;
            }
            //P2 Input
            if (P2SelectedOption == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    P2Input[i].enabled = false;
                }
                if (Input.GetAxisRaw("Vertical2") > 0)
                    P2Input[0].enabled = true;
                if (Input.GetAxisRaw("Vertical2") < 0)
                    P2Input[1].enabled = true;
                if (Input.GetAxisRaw("Horizontal2") < 0)
                    P2Input[2].enabled = true;
                if (Input.GetAxisRaw("Horizontal2") > 0)
                    P2Input[3].enabled = true;
                if (Input.GetAxisRaw("Select2") == 1)
                    P2Input[4].enabled = true;
                if (Input.GetAxisRaw("Start2") == 1)
                    P2Input[5].enabled = true;
                if (Input.GetAxisRaw("Item2") == 1)
                    P2Input[6].enabled = true;
                if (Input.GetAxisRaw("Attack2") == 1)
                    P2Input[7].enabled = true;
            }
            //P3 Input
            if (P3SelectedOption == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    P3Input[i].enabled = false;
                }
                if (Input.GetAxisRaw("Vertical3") > 0)
                    P3Input[0].enabled = true;
                if (Input.GetAxisRaw("Vertical3") < 0)
                    P3Input[1].enabled = true;
                if (Input.GetAxisRaw("Horizontal3") < 0)
                    P3Input[2].enabled = true;
                if (Input.GetAxisRaw("Horizontal3") > 0)
                    P3Input[3].enabled = true;
                if (Input.GetAxisRaw("Select3") == 1)
                    P3Input[4].enabled = true;
                if (Input.GetAxisRaw("Start3") == 1)
                    P3Input[5].enabled = true;
                if (Input.GetAxisRaw("Item3") == 1)
                    P3Input[6].enabled = true;
                if (Input.GetAxisRaw("Attack3") == 1)
                    P3Input[7].enabled = true;
            }
            //P4 Input
            if (P4SelectedOption == 2)
            {
                for (int i = 0; i < 8; i++)
                {
                    P4Input[i].enabled = false;
                }
                if (Input.GetAxisRaw("Vertical4") > 0)
                    P4Input[0].enabled = true;
                if (Input.GetAxisRaw("Vertical4") < 0)
                    P4Input[1].enabled = true;
                if (Input.GetAxisRaw("Horizontal4") < 0)
                    P4Input[2].enabled = true;
                if (Input.GetAxisRaw("Horizontal4") > 0)
                    P4Input[3].enabled = true;
                if (Input.GetAxisRaw("Select4") == 1)
                    P4Input[4].enabled = true;
                if (Input.GetAxisRaw("Start4") == 1)
                    P4Input[5].enabled = true;
                if (Input.GetAxisRaw("Item4") == 1)
                    P4Input[6].enabled = true;
                if (Input.GetAxisRaw("Attack4") == 1)
                    P4Input[7].enabled = true;
            }
        }
    }

    void SelectOption()
    {
        //File Select Screen
        if (menuScreen == 1 && !selecting)
        {
            selecting = true;
            //Load File 1
            if (selectedOption == 1 && File.Exists(Application.persistentDataPath + "/save1.despacito"))
            {
                PersistentManager.Instance.saveSlot = 1;
                PersistentManager.Instance.LoadData("/save1.despacito");
                LoadGameVariables();
                LoadMapVars();
                if (PersistentManager.Instance.inLevel == 0)
                {
                    SceneManager.LoadScene("Overworld");
                    FindObjectOfType<MusicManager>().ChangeSong("Overworld");
                }
                else
                {
                    SceneManager.LoadScene("Dungeon");
                    if (PersistentManager.Instance.inLevel == 9)
                        FindObjectOfType<MusicManager>().ChangeSong("Level 9");
                    else
                        FindObjectOfType<MusicManager>().ChangeSong("Dungeon");
                }
            }
            //Load File 2
            if (selectedOption == 2 && File.Exists(Application.persistentDataPath + "/save2.despacito"))
            {
                PersistentManager.Instance.saveSlot = 2;
                PersistentManager.Instance.LoadData("/save2.despacito");
                LoadGameVariables();
                LoadMapVars();
                if (PersistentManager.Instance.inLevel == 0)
                {
                    SceneManager.LoadScene("Overworld");
                    FindObjectOfType<MusicManager>().ChangeSong("Overworld");
                }
                else
                {
                    SceneManager.LoadScene("Dungeon");
                    if (PersistentManager.Instance.inLevel == 9)
                        FindObjectOfType<MusicManager>().ChangeSong("Level 9");
                    else
                        FindObjectOfType<MusicManager>().ChangeSong("Dungeon");
                }
            }
            //Load File 3
            if (selectedOption == 3 && File.Exists(Application.persistentDataPath + "/save3.despacito"))
            {
                PersistentManager.Instance.saveSlot = 3;
                PersistentManager.Instance.LoadData("/save3.despacito");
                LoadGameVariables();
                LoadMapVars();
                if (PersistentManager.Instance.inLevel == 0)
                {
                    SceneManager.LoadScene("Overworld");
                    FindObjectOfType<MusicManager>().ChangeSong("Overworld");
                }
                else
                {
                    SceneManager.LoadScene("Dungeon");
                    if (PersistentManager.Instance.inLevel == 9)
                        FindObjectOfType<MusicManager>().ChangeSong("Level 9");
                    else
                        FindObjectOfType<MusicManager>().ChangeSong("Dungeon");
                }
            }
            //File Creation
            if (selectedOption == 4)
            {
                menuScreen = 2;
                selectedOption = 1;
                options = 4;
                letterPointer.GetComponent<Animator>().enabled = true;
                letterPointer.GetComponent<SpriteRenderer>().enabled = true;
                if (selectedOption == 1 && File.Exists(Application.persistentDataPath + "/save1.despacito"))
                    letterPointer.transform.localPosition = new Vector2(10, 0);
                fileScreen.GetComponent<SpriteRenderer>().sprite = fileNaming;
                fileDisplays.transform.localPosition = new Vector2(72, 136);
                selector.transform.localPosition = new Vector2(-115.4f, 137.5f);
            }
            //File Deletion
            if (selectedOption == 5)
            {
                menuScreen = 3;
                selectedOption = 4;
                options = 4;
                fileScreen.GetComponent<SpriteRenderer>().sprite = fileDeletion;
                fileDisplays.transform.localPosition = new Vector2(86, 145.2f);
                selector.transform.localPosition = new Vector2(-115.4f, -23);
            }
            //Goto Settings
            if (selectedOption == 6)
            {
                //Load Menu vars
                if (File.Exists(Application.persistentDataPath + "\\settings.txt"))
                {
                    using (StreamReader file = new StreamReader(Application.persistentDataPath + "\\settings.txt"))
                    {
                        string settings = file.ReadToEnd();
                        if (settings.Length != 0)
                        {
                            PersistentManager.Instance.players = JsonUtility.FromJson<SavedSettings>(settings).numPlayers;
                            playerCharacters = JsonUtility.FromJson<SavedSettings>(settings).playerChars;
                        }
                    }
                }
                else
                {
                    File.Create(Application.persistentDataPath + "\\settings.txt");
                }
                //Do some random things
                menuScreen = 4;
                selectedOption = 1;
                P2SelectedOption = 2;
                P3SelectedOption = 2;
                P4SelectedOption = 2;
                options = 4;
                P1CharacterSprite.sprite = characterSprites[playerCharacters[0] - 1];
                P2CharacterSprite.sprite = characterSprites[playerCharacters[1] - 1];
                P3CharacterSprite.sprite = characterSprites[playerCharacters[2] - 1];
                P4CharacterSprite.sprite = characterSprites[playerCharacters[3] - 1];
                settingsSprites.SetActive(true);
                playerNumPointer.transform.localPosition = new Vector2(-0.0044f, 0);
                playerNumPointer.transform.Translate(0.16f * (PersistentManager.Instance.players - 1), 0, 0);
                fileScreen.GetComponent<SpriteRenderer>().sprite = settings;
                fileDisplays.transform.localPosition = new Vector2(1000, 1000);
                selector.transform.localPosition = new Vector2(-75.81f, 155.87f);
                P2Selector.transform.localPosition = new Vector2(-1.1423f, -0.2152f);
                P3Selector.transform.localPosition = new Vector2(0.3077f, 0.3945f);
                P4Selector.transform.localPosition = new Vector2(0.3074f, -0.2152f);
            }
        }
        //File Name Registry
        if (menuScreen == 2 && !selecting)
        {
            selecting = true;
            if (selectedOption == 4)
            {
                foreach (GameObject player in allPlayers)
                {
                    player.GetComponent<PlayerHealthManager>().health = 12;
                }
                menuScreen = 1;
                selectedOption = 1;
                options = 6;
                if (file1Name.text != "________" && !File.Exists(Application.persistentDataPath + "/save1.despacito"))
                {
                    file1HeartCover.value = 5;
                    PersistentManager.Instance.fileName = file1Name.text;
                    SaveSystem.SaveGame("/save1.despacito");
                }
                if (file2Name.text != "________" && !File.Exists(Application.persistentDataPath + "/save2.despacito"))
                {
                    file2HeartCover.value = 5;
                    PersistentManager.Instance.fileName = file2Name.text;
                    SaveSystem.SaveGame("/save2.despacito");
                }
                if (file3Name.text != "________" && !File.Exists(Application.persistentDataPath + "/save3.despacito"))
                {
                    file3HeartCover.value = 5;
                    PersistentManager.Instance.fileName = file3Name.text;
                    SaveSystem.SaveGame("/save3.despacito");
                }
                letterPointer.GetComponent<Animator>().enabled = false;
                letterPointer.GetComponent<SpriteRenderer>().enabled = false;
                fileScreen.GetComponent<SpriteRenderer>().sprite = fileSelection;
                selector.transform.localPosition = new Vector2(-185.4f, 80f);
                fileDisplays.transform.localPosition = new Vector2(0, 79);
            }
        }
        //File Deletion
        if (menuScreen == 3 && !selecting)
        {
            selecting = true;
            if (selectedOption == 1 && File.Exists(Application.persistentDataPath + "/save1.despacito"))
            {
                FindObjectOfType<SFXManager>().PlaySound("Link Hurt");
                File.Delete(Application.persistentDataPath + "/save1.despacito");
                file1Name.text = "________";
                file1HeartCover.value = 8;
                file1HeartCover2.value = 8;
            }
            if (selectedOption == 2 && File.Exists(Application.persistentDataPath + "/save2.despacito"))
            {
                FindObjectOfType<SFXManager>().PlaySound("Link Hurt");
                File.Delete(Application.persistentDataPath + "/save2.despacito");
                file2Name.text = "________";
                file2HeartCover.value = 8;
                file2HeartCover2.value = 8;
            }
            if (selectedOption == 3 && File.Exists(Application.persistentDataPath + "/save3.despacito"))
            {
                FindObjectOfType<SFXManager>().PlaySound("Link Hurt");
                File.Delete(Application.persistentDataPath + "/save3.despacito");
                file3Name.text = "________";
                file3HeartCover.value = 8;
                file3HeartCover2.value = 8;
            }
            if (selectedOption == 4)
            {
                menuScreen = 1;
                selectedOption = 1;
                options = 6;
                fileScreen.GetComponent<SpriteRenderer>().sprite = fileSelection;
                selector.transform.localPosition = new Vector2(-185.4f, 80f);
                fileDisplays.transform.localPosition = new Vector2(0, 79);
            }
        }
        //Exit Settings
        if (menuScreen == 4 && !selecting && selectedOption == 4)
        {
            allPlayers = allPlayers.Where(c => c != null).ToArray();
            //Apply Character Sprites
            for (int i = 0; i < 4; i++)
            {
                allPlayers[i].GetComponent<PlayerController>().smallShield = animControllers[playerCharacters[i] - 1];
                allPlayers[i].GetComponent<PlayerController>().largeShield = anim2Controllers[playerCharacters[i] - 1];
                allPlayers[i].GetComponent<PlayerController>().swordBeam = swordBeams[playerCharacters[i] - 1];
                allPlayers[i].GetComponentsInChildren<Animator>(false)[1].runtimeAnimatorController = boomerangAnimControllers[playerCharacters[i] - 1];
                if (playerCharacters[i] == 1)
                    allPlayers[i].GetComponent<PlayerController>().swords = sword1Sprites;
                if (playerCharacters[i] == 2)
                    allPlayers[i].GetComponent<PlayerController>().swords = sword2Sprites;
                if (playerCharacters[i] == 3)
                    allPlayers[i].GetComponent<PlayerController>().swords = sword3Sprites;
                if (playerCharacters[i] == 4)
                    allPlayers[i].GetComponent<PlayerController>().swords = sword4Sprites;
            }
            //Apply # of players
            foreach (var player in allPlayers)
                player.SetActive(false);
            for (int i = 0; i < PersistentManager.Instance.players; i++)
                allPlayers[i].SetActive(true);
            players = GameObject.FindGameObjectsWithTag("Player").OrderBy(p => p.name).ToArray();
            //Save settings
            SavedSettings settings = new SavedSettings();
            settings.numPlayers = PersistentManager.Instance.players;
            settings.playerChars = playerCharacters;
            string json = JsonUtility.ToJson(settings);
            using (StreamWriter file = new StreamWriter(Application.persistentDataPath + "\\settings.txt", false))
                file.WriteLine(json);
            //Move To Main Menu
            selecting = true;
            menuScreen = 1;
            selectedOption = 1;
            options = 6;
            fileScreen.GetComponent<SpriteRenderer>().sprite = fileSelection;
            settingsSprites.SetActive(false);
            selector.transform.localPosition = new Vector2(-185.4f, 80f);
            fileDisplays.transform.localPosition = new Vector2(0, 79);
        }
    }
    //Initialize game for both first start and change file
    void LoadGameVariables()
    {
        allPlayers = allPlayers.Where(c => c != null).ToArray();

        //Load Menu vars
        if (File.Exists(Application.persistentDataPath + "\\settings.txt"))
        {
            using (StreamReader file = new StreamReader(Application.persistentDataPath + "\\settings.txt"))
            {
                string settings = file.ReadToEnd();
                if (settings.Length != 0)
                {
                    PersistentManager.Instance.players = JsonUtility.FromJson<SavedSettings>(settings).numPlayers;
                    playerCharacters = JsonUtility.FromJson<SavedSettings>(settings).playerChars;
                }
            }
        }

        //Apply Character Sprites
        for (int i = 0; i < 4; i++)
        {
            allPlayers[i].GetComponent<PlayerController>().smallShield = animControllers[playerCharacters[i] - 1];
            allPlayers[i].GetComponent<PlayerController>().largeShield = anim2Controllers[playerCharacters[i] - 1];
            allPlayers[i].GetComponent<PlayerController>().swordBeam = swordBeams[playerCharacters[i] - 1];
            allPlayers[i].GetComponent<PlayerHealthManager>().ghostAnim = ghostAnims[playerCharacters[i] - 1];
            allPlayers[i].GetComponentsInChildren<Animator>(false)[1].runtimeAnimatorController = boomerangAnimControllers[playerCharacters[i] - 1];
            if (playerCharacters[i] == 1)
                allPlayers[i].GetComponent<PlayerController>().swords = sword1Sprites;
            if (playerCharacters[i] == 2)
                allPlayers[i].GetComponent<PlayerController>().swords = sword2Sprites;
            if (playerCharacters[i] == 3)
                allPlayers[i].GetComponent<PlayerController>().swords = sword3Sprites;
            if (playerCharacters[i] == 4)
                allPlayers[i].GetComponent<PlayerController>().swords = sword4Sprites;
        }

        //Apply # of players
        foreach (var player in allPlayers)
            player.SetActive(false);
        for (int i = 0; i < PersistentManager.Instance.players; i++)
            allPlayers[i].SetActive(true);
        players = GameObject.FindGameObjectsWithTag("Player").OrderBy(p => p.name).ToArray();

        //Hide UIs
        foreach (GameObject ui in playerUIs)
            ui.GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < PersistentManager.Instance.players - 1; i++)
            playerUIs[i].GetComponent<SpriteRenderer>().enabled = false;

        //Initialize for changing file
        cam.SetActive(true);
        ui.SetActive(true);
        if (PersistentManager.Instance.inLevel == 0)
        {
            cam.transform.position = new Vector3(-1.28f, -5.986f, -10);
            foreach (GameObject player in players)
            {
                if (player)
                {
                    player.SetActive(true);
                    player.transform.position = new Vector2(-1.8f + ((int.Parse(player.name[7].ToString()) - 1) / 5f), -6.3f);
                }
            }
        }
        else
        {
            cam.transform.position = new Vector3(PersistentManager.Instance.dungeonCamOffsets[PersistentManager.Instance.inLevel].x, PersistentManager.Instance.dungeonCamOffsets[PersistentManager.Instance.inLevel].y, -10);
            PersistentManager.Instance.playerLocation = PersistentManager.Instance.dungeonScreenOffsets[PersistentManager.Instance.inLevel];
            foreach (GameObject player in players)
            {
                if (player)
                {
                    player.SetActive(true);
                    player.transform.position = cam.transform.position + new Vector3(0, -0.7f, 10);
                }
            }
        }

        //Dead Players
        foreach (GameObject player in allPlayers)
        {
            if (player.GetComponent<PlayerHealthManager>().health == 0)
            {
                player.GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<PlayerHealthManager>().ghostAnim;
                player.GetComponent<PlayerController>().attackingAnim = true;
                player.GetComponent<PlayerController>().dead = true;
                player.GetComponent<PlayerController>().attacking = -1;
                player.layer = 18;
            }
            else
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
        //Change Colors for red rings
        if (PersistentManager.Instance.hasRing2)
        {
            for (int i = 0; i < 4; i++)
            {
                Color32[] cols = textures[i].GetPixels32();
                for (var a = 0; a < cols.Length; a++)
                {
                    //Debug.Log(cols[a]);
                    if (colors.Contains(cols[a]))
                        cols[a] = new Color32(179, 42, 11, 255);
                    if (cols[a].Equals(new Color32(206, 27, 27, 255)))
                        cols[a] = new Color32(255, 255, 254, 255);
                }
                textures[i].SetPixels32(cols);
                textures[i].Apply(true);
            }
        }
        //Graphics
        ui.GetComponent<UIController>().DefineItems();
        ui.GetComponent<UIController>().NormalizeItems();
        ui.GetComponent<UIController>().DisplaySharedItems();
        fadeToBlack.transform.position = new Vector3(-1.28f, -5.986f, -10f);
        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1);
    }
    //Initialize map
    void LoadMapVars()
    {
        //Delete Dungeon Map Items
        for (int i = 0; i < map.transform.childCount; i++)
            Destroy(map.transform.GetChild(i).gameObject);
        if (PersistentManager.Instance.inLevel == 0)
        {
            //Setup Map for Overworld
            overworldMap.GetComponent<SpriteRenderer>().enabled = true;
            map.GetComponent<SpriteRenderer>().enabled = false;
            overworldPointer.enabled = true;
            dungeonPointer.enabled = false;
            PersistentManager.Instance.inLevel = 0;
        }
        else
        {
            PersistentManager.Instance.localDungeonPos = Vector2Int.zero;
            posMarker.transform.localPosition = new Vector2(PersistentManager.Instance.dungeonMapOffsets[PersistentManager.Instance.inLevel] * 0.075f, -0.1537f);
            map.transform.localPosition = new Vector2(PersistentManager.Instance.dungeonMapOffsets[PersistentManager.Instance.inLevel] * 0.075f, -0.1537f);
            overworldMap.GetComponent<SpriteRenderer>().enabled = false;
            map.GetComponent<SpriteRenderer>().enabled = true;
            overworldPointer.enabled = false;
            dungeonPointer.enabled = true;
            if (PersistentManager.Instance.dungeonMaps.Contains(PersistentManager.Instance.inLevel))
                GameObject.Find("UI Map").GetComponent<SpriteRenderer>().enabled = true;
            if (PersistentManager.Instance.dungeonCompasses.Contains(PersistentManager.Instance.inLevel))
            {
                GameObject.Find("UI Compass").GetComponent<SpriteRenderer>().enabled = true;
                dungeonCompassPointer.GetComponent<SpriteRenderer>().enabled = true;
            }
            dungeonCompassPointer.transform.position = dungeonPointer.transform.position;
            dungeonCompassPointer.transform.Translate(new Vector2(PersistentManager.Instance.dungeonCompassPositions[PersistentManager.Instance.inLevel].x * 0.08f, PersistentManager.Instance.dungeonCompassPositions[PersistentManager.Instance.inLevel].y * 0.04f));
            //Create Dungeon Map
            foreach (Vector2 i in PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel])
            {
                GameObject room = Instantiate(roomMarker);
                room.transform.parent = map.transform;
                room.transform.localPosition = Vector2.zero;
                room.transform.localScale = Vector3.one;
                room.transform.Translate(new Vector2(i.x * 0.08f, i.y * 0.04f));
            }
            foreach (Vector2 i in PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel])
            {
                GameObject door = Instantiate(doorMarker);
                door.transform.parent = map.transform;
                door.transform.localPosition = Vector2.zero;
                door.transform.localScale = Vector3.one;
                door.transform.Translate(new Vector2(i.x, i.y));
            }
        }
    }

    //Select/Delete Letter
    void SelectLetter()
    {
        FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
        selectingLetter = true;
        if (letterInsertPoint == 7)
            letterPointer.transform.Translate(new Vector2(-0.64f, 0));
        if (letterInsertPoint > 7)
            letterInsertPoint = 0;
        if (selectedOption == 1 && !File.Exists(Application.persistentDataPath + "/save1.despacito"))
        {
            letterPointer.transform.Translate(new Vector2(0.08f, 0));
            file1Name.text = file1Name.text.Remove(letterInsertPoint, 1);
            file1Name.text = file1Name.text.Insert(letterInsertPoint, text[selectedLetter.y][selectedLetter.x].ToString());
            letterInsertPoint++;
        }
        if (selectedOption == 2 && !File.Exists(Application.persistentDataPath + "/save2.despacito"))
        {
            letterPointer.transform.Translate(new Vector2(0.08f, 0));
            file2Name.text = file2Name.text.Remove(letterInsertPoint, 1);
            file2Name.text = file2Name.text.Insert(letterInsertPoint, text[selectedLetter.y][selectedLetter.x].ToString());
            letterInsertPoint++;
        }
        if (selectedOption == 3 && !File.Exists(Application.persistentDataPath + "/save3.despacito"))
        {
            letterPointer.transform.Translate(new Vector2(0.08f, 0));
            file3Name.text = file3Name.text.Remove(letterInsertPoint, 1);
            file3Name.text = file3Name.text.Insert(letterInsertPoint, text[selectedLetter.y][selectedLetter.x].ToString());
            letterInsertPoint++;
        }
    }
    void DeleteLetter()
    {
        FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
        selectingLetter = true;
        if (letterInsertPoint == 0 || letterInsertPoint == 8)
        {
            letterInsertPoint = 8;
            letterPointer.transform.Translate(new Vector2(0.64f, 0));
        }
        if (selectedOption == 1 && !File.Exists(Application.persistentDataPath + "/save1.despacito"))
        {
            letterInsertPoint--;
            letterPointer.transform.Translate(new Vector2(-0.08f, 0));
            file1Name.text = file1Name.text.Remove(letterInsertPoint, 1);
            file1Name.text = file1Name.text.Insert(letterInsertPoint, "_");
        }
        if (selectedOption == 2 && !File.Exists(Application.persistentDataPath + "/save2.despacito"))
        {
            letterInsertPoint--;
            letterPointer.transform.Translate(new Vector2(-0.08f, 0));
            file2Name.text = file2Name.text.Remove(letterInsertPoint, 1);
            file2Name.text = file2Name.text.Insert(letterInsertPoint, "_");
        }
        if (selectedOption == 3 && !File.Exists(Application.persistentDataPath + "/save3.despacito"))
        {
            letterInsertPoint--;
            letterPointer.transform.Translate(new Vector2(-0.08f, 0));
            file3Name.text = file3Name.text.Remove(letterInsertPoint, 1);
            file3Name.text = file3Name.text.Insert(letterInsertPoint, "_");
        }
    }
    //Menu Selecting
    void SelectUp()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedOption == 1)
        {
            selectedOption = options;
            if (options == 4)
                selector.transform.Translate(new Vector2(0, -0.75f));
            else
                selector.transform.Translate(new Vector2(0, -1.07f));
        }
        else if (selectedOption == 5 || selectedOption == 6)
        {
            selectedOption--;
            selector.transform.Translate(new Vector2(0, 0.16f));
        }
        else if (selectedOption == 4)
        {
            selectedOption--;
            selector.transform.Translate(new Vector2(0, 0.27f));
        }
        else
        {
            selectedOption--;
            selector.transform.Translate(new Vector2(0, 0.24f));
        }
    }
    void SelectDown()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedOption == options)
        {
            selectedOption = 1;
            if (options == 4)
                selector.transform.Translate(new Vector2(0, 0.75f));
            else
                selector.transform.Translate(new Vector2(0, 1.07f));
        }
        else if (selectedOption == 4 || selectedOption == 5)
        {
            selectedOption++;
            selector.transform.Translate(new Vector2(0, -0.16f));
        }
        else if (selectedOption == 3)
        {
            selectedOption++;
            selector.transform.Translate(new Vector2(0, -0.27f));
        }
        else
        {
            selectedOption++;
            selector.transform.Translate(new Vector2(0, -0.24f));
        }
        if (menuScreen == 2)
        {
            letterPointer.transform.localPosition = (new Vector2(0.35f, 0));
            letterInsertPoint = 0;
            if (selectedOption == 1 && File.Exists(Application.persistentDataPath + "/save1.despacito"))
                letterPointer.transform.localPosition = (new Vector2(10, 0));
            if (selectedOption == 2 && File.Exists(Application.persistentDataPath + "/save2.despacito"))
                letterPointer.transform.localPosition = (new Vector2(10, 0));
            if (selectedOption == 3 && File.Exists(Application.persistentDataPath + "/save3.despacito"))
                letterPointer.transform.localPosition = (new Vector2(10, 0));
        }
    }
    //Name Entry Selecting
    void NameSelectUp()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedLetter.y == 0)
        {
            selectedLetter.y = 3;
            letterSelector.transform.Translate(new Vector2(0, -0.48f));
        }
        else
        {
            selectedLetter.y--;
            letterSelector.transform.Translate(new Vector2(0, 0.16f));
        }
        StopAllCoroutines();
        StartCoroutine(NameSelect());
    }
    void NameSelectDown()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedLetter.y == 3)
        {
            selectedLetter.y = 0;
            letterSelector.transform.Translate(new Vector2(0, 0.48f));
        }
        else
        {
            selectedLetter.y++;
            letterSelector.transform.Translate(new Vector2(0, -0.16f));
        }
        StopAllCoroutines();
        StartCoroutine(NameSelect());
    }
    void NameSelectLeft()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedLetter.x == 0)
        {
            selectedLetter.x = 10;
            letterSelector.transform.Translate(new Vector2(1.6f, 0));
        }
        else
        {
            selectedLetter.x--;
            letterSelector.transform.Translate(new Vector2(-0.16f, 0));
        }
        StopAllCoroutines();
        StartCoroutine(NameSelect());
    }
    void NameSelectRight()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedLetter.x == 10)
        {
            selectedLetter.x = 0;
            letterSelector.transform.Translate(new Vector2(-1.6f, 0));
        }
        else
        {
            selectedLetter.x++;
            letterSelector.transform.Translate(new Vector2(0.16f, 0));
        }
        StopAllCoroutines();
        StartCoroutine(NameSelect());
    }
    //Settings
    void SettingsSelection(int player)
    {
        if (player == 1 && !selecting)
        {
            FindObjectOfType<SFXManager>().PlaySound("Rupee");
            selecting = true;
            if (selectedOption == 1)
                selector.transform.localPosition = new Vector2(-231.7f, 75.72f);
            if (selectedOption == 2)
                selector.transform.localPosition = new Vector2(-29.88f, -4.21f);
            if (selectedOption == 3)
                selector.transform.localPosition = new Vector2(-118.5f, -189.02f);
            if (selectedOption == 4)
            {
                selector.transform.localPosition = new Vector2(-75.81f, 155.87f);
                selectedOption = 0;
            }
            selectedOption++;
        }
        if (player == 2 && !P2Selecting)
        {
            FindObjectOfType<SFXManager>().PlaySound("Rupee");
            P2Selecting = true;
            if (P2SelectedOption == 2)
                P2Selector.transform.localPosition = new Vector2(-0.202f, -0.5907f);
            if (P2SelectedOption == 3)
                P2Selector.transform.localPosition = new Vector2(-1.1423f, -0.2152f);
            P2SelectedOption++;
            if (P2SelectedOption == 4)
                P2SelectedOption = 2;
        }
        if (player == 3 && !P3Selecting)
        {
            FindObjectOfType<SFXManager>().PlaySound("Rupee");
            P3Selecting = true;
            if (P3SelectedOption == 2)
                P3Selector.transform.localPosition = new Vector2(0.1f, 0.01909f);
            if (P3SelectedOption == 3)
                P3Selector.transform.localPosition = new Vector2(0.3077f, 0.3945f);
            P3SelectedOption++;
            if (P3SelectedOption == 4)
                P3SelectedOption = 2;
        }
        if (player == 4 && !P4Selecting)
        {
            FindObjectOfType<SFXManager>().PlaySound("Rupee");
            P4Selecting = true;
            if (P4SelectedOption == 2)
                P4Selector.transform.localPosition = new Vector2(0.098f, -0.5907f);
            if (P4SelectedOption == 3)
                P4Selector.transform.localPosition = new Vector2(0.3074f, -0.2152f);
            P4SelectedOption++;
            if (P4SelectedOption == 4)
                P4SelectedOption = 2;
        }
    }
    void SettingsSelectLeft(int player)
    {
        if (player == 1 && !selecting)
        {
            selecting = true;
            //Player amount selector
            if (selectedOption == 1)
            {
                playerNumPointer.transform.Translate(-0.16f, 0, 0);
                PersistentManager.Instance.players--;
                if (PersistentManager.Instance.players == 0)
                {
                    PersistentManager.Instance.players = 4;
                    playerNumPointer.transform.Translate(0.64f, 0, 0);
                }
                FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            }
            //Character Selector
            if (selectedOption == 3)
            {
                playerCharacters[0]--;
                if (playerCharacters[0] == 0)
                    playerCharacters[0] = 4;
                P1CharacterSprite.sprite = characterSprites[playerCharacters[0] - 1];
                FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
                PersistentManager.Instance.player1Char = playerCharacters[0];
            }
        }
        //Character Selector
        if (player == 2 && !P2Selecting && P2SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P2Selecting = true;
            playerCharacters[1]--;
            if (playerCharacters[1] == 0)
                playerCharacters[1] = 4;
            P2CharacterSprite.sprite = characterSprites[playerCharacters[1] - 1];
            PersistentManager.Instance.player2Char = playerCharacters[1];
        }
        //Character Selector
        if (player == 3 && !P3Selecting && P3SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P3Selecting = true;
            playerCharacters[2]--;
            if (playerCharacters[2] == 0)
                playerCharacters[2] = 4;
            P3CharacterSprite.sprite = characterSprites[playerCharacters[2] - 1];
            PersistentManager.Instance.player3Char = playerCharacters[2];
        }
        //Character Selector
        if (player == 4 && !P4Selecting && P4SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P4Selecting = true;
            playerCharacters[3]--;
            if (playerCharacters[3] == 0)
                playerCharacters[3] = 4;
            P4CharacterSprite.sprite = characterSprites[playerCharacters[3] - 1];
            PersistentManager.Instance.player4Char = playerCharacters[3];
        }
    }
    void SettingsSelectRight(int player)
    {
        if (player == 1 && !selecting)
        {
            selecting = true;
            //Player amount selector
            if (selectedOption == 1)
            {
                playerNumPointer.transform.Translate(0.16f, 0, 0);
                PersistentManager.Instance.players++;
                if (PersistentManager.Instance.players == 5)
                {
                    PersistentManager.Instance.players = 1;
                    playerNumPointer.transform.Translate(-0.64f, 0, 0);
                }
                FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            }
            //Character Selector
            if (selectedOption == 3)
            {
                playerCharacters[0]++;
                if (playerCharacters[0] == 5)
                    playerCharacters[0] = 1;
                P1CharacterSprite.sprite = characterSprites[playerCharacters[0] - 1];
                FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            }
        }
        //Character Selector
        if (player == 2 && !P2Selecting && P2SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P2Selecting = true;
            playerCharacters[1]++;
            if (playerCharacters[1] == 5)
                playerCharacters[1] = 1;
            P2CharacterSprite.sprite = characterSprites[playerCharacters[1] - 1];
        }
        //Character Selector
        if (player == 3 && !P3Selecting && P3SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P3Selecting = true;
            playerCharacters[2]++;
            if (playerCharacters[2] == 5)
                playerCharacters[2] = 1;
            P3CharacterSprite.sprite = characterSprites[playerCharacters[2] - 1];
        }
        //Character Selector
        if (player == 4 && !P4Selecting && P4SelectedOption == 3)
        {
            FindObjectOfType<SFXManager>().PlaySound("Bomb Drop");
            P4Selecting = true;
            playerCharacters[3]++;
            if (playerCharacters[3] == 5)
                playerCharacters[3] = 1;
            P4CharacterSprite.sprite = characterSprites[playerCharacters[3] - 1];
        }
    }

    IEnumerator TextCrawl()
    {
        while (inTitleCrawl)
        {
            transform.position = Vector3.zero;
            yield return StartCoroutine(WaitFor.Frames(650));
            for (var i = 0; i < 25; i++)
            {
                fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a + 0.04f);
                yield return StartCoroutine(WaitFor.Frames(5));
            }
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -.36f);
            yield return StartCoroutine(WaitFor.Frames(565));
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            yield return StartCoroutine(WaitFor.Frames(600));
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -.32f);
            yield return StartCoroutine(WaitFor.Frames(2500));
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            yield return StartCoroutine(WaitFor.Frames(541));
        }
    }
    IEnumerator NameSelect()
    {
        yield return StartCoroutine(WaitFor.Frames(20));
        selecting = false;
    }
}