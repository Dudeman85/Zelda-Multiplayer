using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; set; }

    //Refrence Variables (Not Saved)
    public int[] dungeonMapOffsets;
    public Vector2[] dungeonCompassPositions;
    public Vector2[] dungeonCamOffsets;
    public Vector2Int[] dungeonScreenOffsets;
    public List<Vector2Int[]> dungeonRooms = new List<Vector2Int[]>();
    public List<Vector2[]> dungeonDoors = new List<Vector2[]>();

    //Dynamic Variables (Not Saved)
    public bool fireUsedInRoom;
    public string spawnpoint;
    public Vector2Int playerLocation;

    public int[] directionsTraveled;
    public string tunnelEntry;
    public string tunnelExit;
    public Vector2Int newTunnelPos;

    public bool pausePlayer;
    public bool disableUI;
    public bool disableTransitions;
    public bool disablePausing;

    public int players = 1;
    public int player1Char = 1;
    public int player2Char = 2;
    public int player3Char = 3;
    public int player4Char = 4;

    public Vector2Int localDungeonPos;

    //Save Variables (Saved)
    public string fileName;
    public int saveSlot;
    public int inLevel;

    public bool hasSword1;
    public bool hasSword2;
    public bool hasSword3;
    public bool hasBoomerang;
    public bool hasBoomerang2;
    public bool hasBombs;
    public bool hasBow;
    public bool hasBow2;
    public bool hasRedCandle;
    public bool hasFlute;
    public bool hasLetter;
    public bool hasWand;
    public bool hasRaft;
    public bool hasBook;
    public bool hasRing;
    public bool hasRing2;
    public bool hasLadder;
    public bool hasSkeletonKey;
    public bool hasPowerBracelet;

    public int maxHealth;
    public int keys;
    public bool letterDelivered;

    public List<int> dungeonCompasses = new List<int>();
    public List<int> dungeonMaps = new List<int>();
    public List<string> basementItemsCompleted = new List<string>();
    public List<int> levelsCompleted = new List<int>();
    public List<string> grottosCompleted = new List<string>();

    public List<List<Vector2Int>> dungeonRoomsExplored = new List<List<Vector2Int>>();
    public List<List<Vector2Int>> dungeonMarkers = new List<List<Vector2Int>>();
    public List<List<Vector2>> dungeonDoorsExplored = new List<List<Vector2>>();

    public List<Vector3Int> coversCleared = new List<Vector3Int>();
    public List<Vector3Int> doorsOpened = new List<Vector3Int>();

    public List<string> H2Used;
    public List<string> A13Used;
    public bool A11Done;
    public bool O15Done;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        foreach (string i in Input.GetJoystickNames())
            Debug.Log(i);
        
        //Initialize Variables
        playerLocation = new Vector2Int(8, 8);
        dungeonMapOffsets = new int[10] { 0, 0, 0, 1, 0, 1, -1, -1, 1, 3 };
        dungeonCamOffsets = new Vector2[10] { new Vector2(), new Vector2(-11.52f, 1.055f), new Vector2(14.08f, 1.055f), new Vector2(11.52f, 1.055f), new Vector2(-16.64f, 1.055f), new Vector2(-3.84f, 1.055f), new Vector2(3.84f, 1.055f), new Vector2(-16.64f, -13.025f), new Vector2(-3.84f, -13.025f), new Vector2(16.64f, -13.025f) };
        dungeonScreenOffsets = new Vector2Int[10] { new Vector2Int(8, 8), new Vector2Int(4, 8), new Vector2Int(14, 8), new Vector2Int(13, 8), new Vector2Int(2, 8), new Vector2Int(7, 8), new Vector2Int(10, 8), new Vector2Int(2, 16), new Vector2Int(7, 16), new Vector2Int(15, 16) };
        dungeonCompassPositions = new Vector2[10] { new Vector2(), new Vector2(2, 4), new Vector2(1, 7), new Vector2(1, 3), new Vector2(2, 6), new Vector2(-2, 5), new Vector2(3, 6), new Vector2(1, 5), new Vector2(-2, 4), new Vector2(-4, 3) };
        //Full Dungeon Map Rooms
        dungeonRooms.Add(new Vector2Int[0]);
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(-1, 2), new Vector2Int(-1, 3), new Vector2Int(-2, 3), new Vector2Int(0, 3), new Vector2Int(0, 4), new Vector2Int(0, 5), new Vector2Int(-1, 5), new Vector2Int(1, 3), new Vector2Int(1, 2), new Vector2Int(2, 3), new Vector2Int(2, 4), new Vector2Int(3, 4) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(2, 3), new Vector2Int(1, 3), new Vector2Int(2, 4), new Vector2Int(1, 4), new Vector2Int(1, 5), new Vector2Int(2, 5), new Vector2Int(2, 6), new Vector2Int(1, 6), new Vector2Int(1, 7), new Vector2Int(0, 7) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2), new Vector2Int(-1, 3), new Vector2Int(0, 3), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(1, 4), new Vector2Int(-1, 4), new Vector2Int(-1, 5), new Vector2Int(-2, 5), new Vector2Int(-2, 3), new Vector2Int(-3, 3), new Vector2Int(-3, 2), new Vector2Int(-3, 1), new Vector2Int(-2, 2) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(-1, 2), new Vector2Int(-1, 3), new Vector2Int(-1, 4), new Vector2Int(0, 4), new Vector2Int(1, 4), new Vector2Int(0, 5), new Vector2Int(0, 6), new Vector2Int(-1, 6), new Vector2Int(-1, 5), new Vector2Int(-1, 7), new Vector2Int(0, 7), new Vector2Int(1, 6), new Vector2Int(1, 7), new Vector2Int(2, 7), new Vector2Int(2, 6) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(-2, 1), new Vector2Int(-1, 2), new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(1, 3), new Vector2Int(0, 3), new Vector2Int(1, 4), new Vector2Int(1, 5), new Vector2Int(1, 6), new Vector2Int(0, 6), new Vector2Int(0, 7), new Vector2Int(-1, 7), new Vector2Int(-1, 6), new Vector2Int(-2, 6), new Vector2Int(-2, 5), new Vector2Int(-2, 4), new Vector2Int(-1, 5), new Vector2Int(0, 5) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2), new Vector2Int(-1, 3), new Vector2Int(-1, 4), new Vector2Int(-1, 5), new Vector2Int(-1, 6), new Vector2Int(0, 6), new Vector2Int(0, 5), new Vector2Int(0, 4), new Vector2Int(1, 4), new Vector2Int(0, 7), new Vector2Int(1, 7), new Vector2Int(1, 6), new Vector2Int(2, 6), new Vector2Int(2, 7), new Vector2Int(3, 7), new Vector2Int(3, 6), new Vector2Int(3, 5), new Vector2Int(3, 4), new Vector2Int(4, 5), new Vector2Int(4, 6) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(2, 2), new Vector2Int(1, 2), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int(-1, 2), new Vector2Int(-1, 3), new Vector2Int(-1, 4), new Vector2Int(-1, 5), new Vector2Int(-1, 6), new Vector2Int(0, 6), new Vector2Int(-1, 7), new Vector2Int(0, 7), new Vector2Int(1, 7), new Vector2Int(2, 7), new Vector2Int(3, 7), new Vector2Int(4, 7), new Vector2Int(3, 6), new Vector2Int(2, 6), new Vector2Int(1, 6), new Vector2Int(1, 5), new Vector2Int(2, 5), new Vector2Int(1, 4), new Vector2Int(0, 4), new Vector2Int(0, 5), new Vector2Int(0, 3) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-2, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(-1, 2), new Vector2Int(-2, 2), new Vector2Int(-2, 3), new Vector2Int(-3, 3), new Vector2Int(-3, 4), new Vector2Int(-2, 4), new Vector2Int(-2, 5), new Vector2Int(-1, 5), new Vector2Int(-1, 6), new Vector2Int(0, 6), new Vector2Int(0, 7), new Vector2Int(1, 6), new Vector2Int(0, 5), new Vector2Int(0, 4), new Vector2Int(-1, 4), new Vector2Int(-1, 3), new Vector2Int(0, 3), new Vector2Int(1, 2), new Vector2Int(1, 4) });
        dungeonRooms.Add(new Vector2Int[] { new Vector2Int(0, 1), new Vector2Int(-1, 1), new Vector2Int(-2, 1), new Vector2Int(-3, 1), new Vector2Int(-4, 1), new Vector2Int(-5, 1), new Vector2Int(-5, 0), new Vector2Int(-5, 2), new Vector2Int(-6, 2), new Vector2Int(-6, 3), new Vector2Int(-6, 4), new Vector2Int(-6, 5), new Vector2Int(-6, 6), new Vector2Int(-5, 6), new Vector2Int(-5, 7), new Vector2Int(-4, 7), new Vector2Int(-3, 7), new Vector2Int(-2, 7), new Vector2Int(-1, 7), new Vector2Int(0, 7), new Vector2Int(0, 6), new Vector2Int(1, 6), new Vector2Int(1, 5), new Vector2Int(1, 4), new Vector2Int(1, 3), new Vector2Int(1, 2), new Vector2Int(0, 2), new Vector2Int(0, 3), new Vector2Int(0, 4), new Vector2Int(0, 5), new Vector2Int(-1, 5), new Vector2Int(-1, 6), new Vector2Int(-2, 6), new Vector2Int(-3, 6), new Vector2Int(-4, 6), new Vector2Int(-4, 5), new Vector2Int(-5, 5), new Vector2Int(-5, 4), new Vector2Int(-5, 3), new Vector2Int(-4, 3), new Vector2Int(-4, 2), new Vector2Int(-3, 2), new Vector2Int(-3, 3), new Vector2Int(-3, 4), new Vector2Int(-3, 5), new Vector2Int(-2, 5), new Vector2Int(-2, 4), new Vector2Int(-2, 3), new Vector2Int(-1, 3), new Vector2Int(-1, 2), new Vector2Int(-2, 2) });
        //Full Dungeon Map Doors
        dungeonDoors.Add(new Vector2[0]);
        dungeonDoors.Add(new Vector2[] { new Vector2(-0.04f, 0), new Vector2(0.04f, 0), new Vector2(0, 0.02f), new Vector2(0, 0.06f), new Vector2(-0.04f, 0.08f), new Vector2(-0.08f, 0.1f), new Vector2(-0.12f, 0.12f), new Vector2(-0.04f, 0.12f), new Vector2(0, 0.1f), new Vector2(0.04f, 0.08f), new Vector2(0.08f, 0.1f), new Vector2(0.12f, 0.12f), new Vector2(0.16f, 0.14f), new Vector2(0.2f, 0.16f), new Vector2(0.04f, 0.12f), new Vector2(0, 0.14f), new Vector2(0, 0.18f), new Vector2(-0.04f, 0.2f) });
        dungeonDoors.Add(new Vector2[] { new Vector2(0, 0.02f), new Vector2(-0.04f, 0.04f), new Vector2(0.04f, 0.04f), new Vector2(0.08f, 0.02f), new Vector2(0.04f, 0), new Vector2(0.12f, 0.04f), new Vector2(0.16f, 0.06f), new Vector2(0.16f, 0.1f), new Vector2(0.16f, 0.14f), new Vector2(0.16f, 0.18f), new Vector2(0.16f, 0.22f), new Vector2(0.12f, 0.24f), new Vector2(0.08f, 0.22f), new Vector2(0.12f, 0.2f), new Vector2(0.12f, 0.16f), new Vector2(0.08f, 0.18f), new Vector2(0.08f, 0.14f), new Vector2(0.12f, 0.12f), new Vector2(0.12f, 0.08f), new Vector2(0.08f, 0.06f), new Vector2(0.08f, 0.1f), new Vector2(0.08f, 0.22f), new Vector2(0.08f, 0.26f), new Vector2(0.04f, 0.28f) });
        dungeonDoors.Add(new Vector2[] { new Vector2(-0.04f, 0), new Vector2(-0.08f, 0.02f), new Vector2(-0.08f, 0.06f), new Vector2(-0.04f, 0.08f), new Vector2(0.04f, 0.08f), new Vector2(0.08f, 0.1f), new Vector2(0.08f, 0.14f), new Vector2(0.04f, 0.12f), new Vector2(-0.04f, 0.12f), new Vector2(-0.08f, 0.1f), new Vector2(-0.12f, 0.08f), new Vector2(-0.2f, 0.08f), new Vector2(-0.24f, 0.06f), new Vector2(-0.24f, 0.1f), new Vector2(-0.2f, 0.12f), new Vector2(-0.16f, 0.1f), new Vector2(-0.12f, 0.12f), new Vector2(-0.08f, 0.14f), new Vector2(-0.08f, 0.18f), new Vector2(-0.12f, 0.2f), new Vector2(0, 0.1f) });
        dungeonDoors.Add(new Vector2[] { new Vector2(-0.04f, 0), new Vector2(0, 0.02f), new Vector2(0.04f, 0.04f), new Vector2(0, 0.06f), new Vector2(-0.04f, 0.08f), new Vector2(-0.08f, 0.1f), new Vector2(-0.08f, 0.14f), new Vector2(-0.04f, 0.16f), new Vector2(0.04f, 0.16f), new Vector2(0, 0.18f), new Vector2(-0.04f, 0.2f), new Vector2(-0.08f, 0.18f), new Vector2(-0.08f, 0.22f), new Vector2(-0.08f, 0.26f), new Vector2(-0.04f, 0.28f), new Vector2(0, 0.26f), new Vector2(-0.04f, 0.24f), new Vector2(0, 0.22f), new Vector2(0, 0.22f), new Vector2(0.04f, 0.24f), new Vector2(0.08f, 0.26f), new Vector2(0.04f, 0.28f), new Vector2(0.12f, 0.24f), new Vector2(0.16f, 0.26f)});
        dungeonDoors.Add(new Vector2[] { new Vector2(0, 0.02f), new Vector2(-0.04f, 0.04f), new Vector2(-0.12f, 0.04f), new Vector2(-0.04f, 0.28f), new Vector2(0, 0.26f), new Vector2(0.04f, 0.24f), new Vector2(-0.08f, 0.06f), new Vector2(-0.04f, 0.08f), new Vector2(0, 0.06f), new Vector2(0.04f, 0.04f), new Vector2(0.08f, 0.02f), new Vector2(0.04f, 0), new Vector2(0.04f, 0.08f), new Vector2(0.08f, 0.1f), new Vector2(0.04f, 0.12f), new Vector2(0, 0.1f), new Vector2(0.08f, 0.14f), new Vector2(0.08f, 0.18f), new Vector2(0.04f, 0.2f), new Vector2(-0.04f, 0.2f), new Vector2(-0.08f, 0.22f), new Vector2(-0.08f, 0.22f), new Vector2(-0.12f, 0.2f), new Vector2(-0.16f, 0.18f), new Vector2(-0.16f, 0.22f)});
        dungeonDoors.Add(new Vector2[] { new Vector2(0.04f, 0), new Vector2(0.08f, 0.02f), new Vector2(-0.04f, 0), new Vector2(-0.08f, 0.02f), new Vector2(-0.08f, 0.06f), new Vector2(-0.08f, 0.1f), new Vector2(-0.08f, 0.14f), new Vector2(-0.08f, 0.18f), new Vector2(-0.08f, 0.22f), new Vector2(-0.04f, 0.24f), new Vector2(0, 0.22f), new Vector2(-0.04f, 0.2f), new Vector2(0, 0.18f), new Vector2(0.04f, 0.16f), new Vector2(0, 0.22f), new Vector2(0, 0.26f), new Vector2(0.04f, 0.28f), new Vector2(0.12f, 0.28f), new Vector2(0.16f, 0.26f), new Vector2(0.12f, 0.24f), new Vector2(0.08f, 0.26f), new Vector2(0.04f, 0.24f), new Vector2(0.32f, 0.22f), new Vector2(0.28f, 0.2f), new Vector2(0.24f, 0.18f), new Vector2(0.24f, 0.22f), new Vector2(0.24f, 0.26f)});
        dungeonDoors.Add(new Vector2[] { new Vector2(0.04f, 0), new Vector2(0.08f, 0.02f), new Vector2(0.12f, 0.04f), new Vector2(0.2f, 0.04f), new Vector2(0.28f, 0.04f), new Vector2(0.16f, 0.06f), new Vector2(0.08f, 0.06f), new Vector2(0.04f, 0.08f), new Vector2(0, 0.06f), new Vector2(0.04f, 0.04f), new Vector2(0, 0.02f), new Vector2(-0.04f, 0.04f), new Vector2(-0.08f, 0.02f), new Vector2(-0.08f, 0.06f), new Vector2(-0.04f, 0.08f), new Vector2(0, 0.1f), new Vector2(-0.08f, 0.1f), new Vector2(0, 0.14f), new Vector2(0.04f, 0.16f), new Vector2(-0.04f, 0.16f), new Vector2(-0.08f, 0.18f), new Vector2(-0.08f, 0.22f), new Vector2(-0.04f, 0.24f), new Vector2(0.04f, 0.24f), new Vector2(0.12f, 0.24f), new Vector2(0.2f, 0.24f), new Vector2(0.24f, 0.26f), new Vector2(0.28f, 0.28f), new Vector2(0.04f, 0.2f), new Vector2(0.12f, 0.2f), new Vector2(0.16f, 0.26f), new Vector2(0, 0.26f), new Vector2(0.04f, 0.28f), new Vector2(-0.04f, 0.28f), new Vector2(-0.08f, 0.26f)});
        dungeonDoors.Add(new Vector2[] { new Vector2(-0.04f, 0), new Vector2(-0.12f, 0), new Vector2(0.04f, 0), new Vector2(0, 0.02f), new Vector2(0, 0.06f), new Vector2(0.04f, 0.08f), new Vector2(-0.04f, 0.08f), new Vector2(-0.12f, 0.08f), new Vector2(-0.08f, 0.1f), new Vector2(-0.08f, 0.14f), new Vector2(-0.04f, 0.12f), new Vector2(0, 0.1f), new Vector2(0, 0.14f), new Vector2(0.04f, 0.16f), new Vector2(0, 0.18f), new Vector2(0, 0.22f), new Vector2(0.04f, 0.24f), new Vector2(0, 0.26f), new Vector2(-0.04f, 0.24f), new Vector2(-0.08f, 0.22f), new Vector2(-0.04f, 0.2f), new Vector2(0, 0.22f), new Vector2(-0.16f, 0.14f), new Vector2(-0.16f, 0.18f), new Vector2(-0.2f, 0.16f), new Vector2(-0.24f, 0.14f), new Vector2(-0.2f, 0.12f)});
        dungeonDoors.Add(new Vector2[] { new Vector2(0, 0.02f), new Vector2(-0.04f, 0.04f), new Vector2(-0.12f, 0.04f), new Vector2(-0.16f, 0.06f), new Vector2(-0.12f, 0.08f), new Vector2(-0.08f, 0.06f), new Vector2(-0.08f, 0.1f), new Vector2(-0.12f, 0.12f), new Vector2(-0.16f, 0.1f), new Vector2(-0.04f, 0.12f), new Vector2(0, 0.1f), new Vector2(-0.04f, 0.08f), new Vector2(0, 0.06f), new Vector2(-0.16f, 0.22f), new Vector2(-0.2f, 0.2f), new Vector2(-0.24f, 0.22f), new Vector2(-0.28f, 0.24f), new Vector2(-0.32f, 0.26f), new Vector2(-0.36f, 0.28f), new Vector2(-0.4f, 0.26f), new Vector2(-0.4f, 0.22f), new Vector2(-0.4f, 0.18f), new Vector2(-0.44f, 0.16f), new Vector2(-0.48f, 0.14f), new Vector2(-0.48f, 0.1f), new Vector2(-0.44f, 0.08f), new Vector2(-0.4f, 0.1f), new Vector2(-0.4f, 0.14f), new Vector2(-0.4f, 0.06f), new Vector2(-0.4f, 0.02f), new Vector2(-0.36f, 0.04f), new Vector2(-0.28f, 0.04f), new Vector2(-0.24f, 0.02f), new Vector2(-0.2f, 0), new Vector2(-0.24f, 0.06f), new Vector2(-0.24f, 0.1f), new Vector2(-0.04f, 0.28f), new Vector2(0, 0.26f), new Vector2(0, 0.22f), new Vector2(0.04f, 0.2f), new Vector2(0.08f, 0.22f), new Vector2(0.08f, 0.22f), new Vector2(0.08f, 0.18f), new Vector2(0.08f, 0.14f), new Vector2(0.08f, 0.1f), new Vector2(0.04f, 0.16f), new Vector2(0, 0.18f), new Vector2(-0.04f, 0.2f), new Vector2(-0.08f, 0.22f), new Vector2(-0.04f, 0.24f), new Vector2(-0.12f, 0.24f), new Vector2(-0.08f, 0.22f), new Vector2(-0.08f, 0.18f), new Vector2(-0.16f, 0.18f), new Vector2(-0.2f, 0.16f), new Vector2(-0.24f, 0.18f), new Vector2(-0.24f, 0.22f), new Vector2(-0.28f, 0.2f), new Vector2(-0.32f, 0.22f), new Vector2(-0.32f, 0.22f), new Vector2(-0.36f, 0.2f), new Vector2(-0.4f, 0.22f), new Vector2(-0.48f, 0.22f), new Vector2(-0.48f, 0.22f), new Vector2(-0.44f, 0.08f), new Vector2(-0.2f, 0.28f), new Vector2(-0.32f, 0.1f), new Vector2(-0.32f, 0.14f)});

        for (int i = 0; i < 11; i++)
        {
            dungeonRoomsExplored.Add(new List<Vector2Int>());
            dungeonMarkers.Add(new List<Vector2Int>());
            dungeonDoorsExplored.Add(new List<Vector2>());
        }
        //Persist through scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadData(string file)
    {
        GameData sharedVars = SaveSystem.LoadGame(file);
        PlayerController[] players = Resources.FindObjectsOfTypeAll<PlayerController>().OrderBy(a => a.name).ToArray();

        //Player Vars
        for (int i = 0; i < 4; i++)
        {
            players[i].gameObject.GetComponent<PlayerHealthManager>().health = sharedVars.health[i];
            players[i].bombs = sharedVars.bombs[i];
            players[i].maxBombs = sharedVars.maxBombs[i];
            players[i].rupees = sharedVars.rupees[i];
            players[i].hasPotion1 = sharedVars.hasPotion1[i];
            players[i].hasPotion2 = sharedVars.hasPotion2[i];
            players[i].hasRing1 = sharedVars.hasRing1[i];
            players[i].hasMagicalShield = sharedVars.hasMagicalShield[i];
            players[i].hasArrows = sharedVars.hasArrows[i];
            players[i].hasMeat = sharedVars.hasMeat[i];
            players[i].hasCandle = sharedVars.hasCandle[i];
        }

        //Persistent manager vars
        fileName = sharedVars.fileName;
        inLevel = sharedVars.inLevel;

        hasSword1 = sharedVars.hasSword1;
        hasSword2 = sharedVars.hasSword2;
        hasSword3 = sharedVars.hasSword3;
        hasBoomerang = sharedVars.hasBoomerang;
        hasBoomerang2 = sharedVars.hasBoomerang2;
        hasBombs = sharedVars.hasBombs;
        hasBow = sharedVars.hasBow;
        hasBow2 = sharedVars.hasBow2;
        hasRedCandle = sharedVars.hasRedCandle;
        hasFlute = sharedVars.hasFlute;
        hasLetter = sharedVars.hasLetter;
        hasWand = sharedVars.hasWand;
        hasRaft = sharedVars.hasRaft;
        hasBook = sharedVars.hasBook;
        hasRing = sharedVars.hasRing;
        hasRing2 = sharedVars.hasRing2;
        hasLadder = sharedVars.hasLadder;
        hasSkeletonKey = sharedVars.hasSkeletonKey;
        hasPowerBracelet = sharedVars.hasPowerBracelet;

        maxHealth = sharedVars.maxHealth;
        keys = sharedVars.keys;

        dungeonCompasses = sharedVars.dungeonCompasses;
        dungeonMaps = sharedVars.dungeonMaps;
        basementItemsCompleted = sharedVars.basementItemsCompleted;
        levelsCompleted = sharedVars.levelsCompleted;
        grottosCompleted = sharedVars.grottosCompleted;

        //Convert int[] to Vector 2 for tilemap blockers
        dungeonRoomsExplored = new List<List<Vector2Int>>();
        dungeonMarkers = new List<List<Vector2Int>>();
        dungeonDoorsExplored = new List<List<Vector2>>();
        for (int i = 0; i < 11; i++)
        {
            dungeonRoomsExplored.Add(new List<Vector2Int>());
            foreach (int[] pos in sharedVars.dungeonRoomsExplored[i])
                dungeonRoomsExplored[i].Add(new Vector2Int(pos[0], pos[1]));
        }
        for (int i = 0; i < 11; i++)
        {
            dungeonDoorsExplored.Add(new List<Vector2>());
            foreach (float[] pos in sharedVars.dungeonDoorsExplored[i])
                dungeonDoorsExplored[i].Add(new Vector2(pos[0], pos[1]));
        }
        for (int i = 0; i < 11; i++)
        {
            dungeonMarkers.Add(new List<Vector2Int>());
            foreach (int[] pos in sharedVars.dungeonMarkers[i])
                dungeonMarkers[i].Add(new Vector2Int(pos[0], pos[1]));
        }

        //Conver int[] to Vector3 for tilemap blockers
        coversCleared = new List<Vector3Int>();
        doorsOpened = new List<Vector3Int>();
        for (int i = 0; i < sharedVars.coversCleared.Count; i++)
            coversCleared.Add(new Vector3Int(sharedVars.coversCleared[i][0], sharedVars.coversCleared[i][1], sharedVars.coversCleared[i][2]));
        for (int i = 0; i < sharedVars.doorsOpened.Count; i++)
            doorsOpened.Add(new Vector3Int(sharedVars.doorsOpened[i][0], sharedVars.doorsOpened[i][1], sharedVars.doorsOpened[i][2]));

        H2Used = sharedVars.H2Used;
        A13Used = sharedVars.A13Used;
        A11Done = sharedVars.A11Done;
        O15Done = sharedVars.O15Done;
        letterDelivered = sharedVars.letterDelivered;
    }
}

public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount -= 1;
            yield return null;
        }
    }
}