using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GameData
{
    //Public Data
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
    public List<string> basementItemsCompleted;

    public int maxHealth;
    public int keys;
    public bool letterDelivered;

    public List<int> dungeonMaps;
    public List<int> dungeonCompasses;
    public List<int> levelsCompleted;
    public List<string> grottosCompleted;
    public List<int[]> coversCleared = new List<int[]>();
    public List<int[]> doorsOpened = new List<int[]>();
    public List<List<int[]>> dungeonRoomsExplored = new List<List<int[]>>();
    public List<List<int[]>> dungeonMarkers = new List<List<int[]>>();
    public List<List<float[]>> dungeonDoorsExplored = new List<List<float[]>>();

    public List<string> H2Used;
    public List<string> A13Used;
    public bool A11Done;
    public bool O15Done;

    //Individual Player Data
    //Player 1
    public int[] health = new int[4];
    public int[] bombs = new int[4];
    public int[] maxBombs = new int[4];
    public int[] rupees = new int[4];
    public bool[] hasPotion1 = new bool[4];
    public bool[] hasPotion2 = new bool[4];
    public bool[] hasRing1 = new bool[4];
    public bool[] hasMagicalShield = new bool[4];
    public bool[] hasArrows = new bool[4];
    public bool[] hasMeat = new bool[4];
    public bool[] hasCandle = new bool[4];

    public GameData(PersistentManager sharedVars, PlayerController[] playerVars)
    {
        playerVars = playerVars.OrderBy(a => a.name).ToArray();
        for (int i = 0; i < 4; i++)
        {
            health[i] = playerVars[i].GetComponent<PlayerHealthManager>().health;
            bombs[i] = playerVars[i].bombs;
            maxBombs[i] = playerVars[i].maxBombs;
            rupees[i] = playerVars[i].rupees;
            hasPotion1[i] = playerVars[i].hasPotion1;
            hasPotion2[i] = playerVars[i].hasPotion2;
            hasRing1[i] = playerVars[i].hasRing1;
            hasMagicalShield[i] = playerVars[i].hasMagicalShield;
            hasArrows[i] = playerVars[i].hasArrows;
            hasMeat[i] = playerVars[i].hasMeat;
            hasCandle[i] = playerVars[i].hasCandle;
        }

        //Persistent Manager Vasr
        fileName = sharedVars.fileName;
        saveSlot = sharedVars.saveSlot;
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
        basementItemsCompleted = sharedVars.basementItemsCompleted;

        maxHealth = sharedVars.maxHealth;
        keys = sharedVars.keys;

        dungeonMaps = sharedVars.dungeonMaps;
        dungeonCompasses = sharedVars.dungeonCompasses;
        levelsCompleted = sharedVars.levelsCompleted;
        grottosCompleted = sharedVars.grottosCompleted;

        //Convert Vector2 to int[] for map data
        for (int i = 0; i < 11; i++)
        {
            dungeonRoomsExplored.Add(new List<int[]>());
            foreach (Vector2Int location in sharedVars.dungeonRoomsExplored[i])
            {
                int[] tempInt = new int[2];
                tempInt[0] = location.x;
                tempInt[1] = location.y;
                dungeonRoomsExplored[i].Add(tempInt);
            }
        }
        for (int i = 0; i < 11; i++)
        {
            dungeonDoorsExplored.Add(new List<float[]>());
            foreach (Vector2 location in sharedVars.dungeonDoorsExplored[i])
            {
                float[] tempInt = new float[2];
                tempInt[0] = location.x;
                tempInt[1] = location.y;
                dungeonDoorsExplored[i].Add(tempInt);
            }
        }
        for (int i = 0; i < 11; i++)
        {
            dungeonMarkers.Add(new List<int[]>());
            foreach (Vector2Int location in sharedVars.dungeonMarkers[i])
            {
                int[] tempInt = new int[2];
                tempInt[0] = location.x;
                tempInt[1] = location.y;
                dungeonMarkers[i].Add(tempInt);
            }
        }

        //Conver Vector3 to int[] for tilemap blockers
        for (int i = 0; i < sharedVars.coversCleared.Count; i++)
        {
            int[] tempInt = new int[3];
            tempInt[0] = sharedVars.coversCleared[i].x;
            tempInt[1] = sharedVars.coversCleared[i].y;
            tempInt[2] = sharedVars.coversCleared[i].z;
            coversCleared.Add(tempInt);
        }
        for (int i = 0; i < sharedVars.doorsOpened.Count; i++)
        {
            int[] tempInt = new int[3];
            tempInt[0] = sharedVars.doorsOpened[i].x;
            tempInt[1] = sharedVars.doorsOpened[i].y;
            tempInt[2] = sharedVars.doorsOpened[i].z;
            doorsOpened.Add(tempInt);
        }
        H2Used = sharedVars.H2Used;
        A13Used = sharedVars.A13Used;
        A11Done = sharedVars.A11Done;
        O15Done = sharedVars.O15Done;
        letterDelivered = sharedVars.letterDelivered;
    }
}
