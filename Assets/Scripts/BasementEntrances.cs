using UnityEngine;
using UnityEngine.Tilemaps;

public class BasementEntrances : MonoBehaviour
{
    public int room;
    public bool exit;
    public string goToRoom;
    public int exitPosition;
    public GameObject item;
    private GameObject[] players;
    private GameObject cam;
    public static GameObject itemClone;
    public Vector2Int newLocalPos;
    private GameObject roomMarker;
    private GameObject map;
    private GameObject posMarker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        roomMarker = Resources.Load("Map Room") as GameObject;
        posMarker = GameObject.Find("Dungeon Position Marker");
        players = GameObject.FindGameObjectsWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        map = GameObject.Find("Map Data");
        if (room <= 2)
        {
            //Tunnel entry room
            PersistentManager.Instance.tunnelEntry = name;
            //Tunnel exit room
            PersistentManager.Instance.tunnelExit = goToRoom;
        }
        //Basement entrances
        //Item Room
        if (room == 1)
        {
            cam.transform.position = new Vector2(-1.28f, -0.706f);
            for (int i = 0; i < players.Length; i++)
                players[i].transform.position = new Vector2(-2, -0.22f);
            if (!PersistentManager.Instance.basementItemsCompleted.Contains(item.name) && item)
            {
                itemClone = Instantiate(item, new Vector3(-1.32f, -0.87f, 0), Quaternion.identity);
                itemClone.name = itemClone.name.Remove(itemClone.name.Length - 7);
            }
        }
        //Tunnel
        if (room == 2)
        {
            PersistentManager.Instance.newTunnelPos = newLocalPos;
            cam.transform.position = new Vector2(1.28f, -0.706f);
            for (int i = 0; i < players.Length; i++)
                players[i].transform.position = new Vector2(2, -0.22f);
        }
        //Basement exits
        if (exit)
        {
            FindObjectOfType<PushableBlocks>().ResetTiles();
            Destroy(itemClone);
            goToRoom = PersistentManager.Instance.tunnelEntry;
            if (room == 3)
            {
                PersistentManager.Instance.localDungeonPos = PersistentManager.Instance.newTunnelPos;
                //Map
                PersistentManager.Instance.dungeonRoomsExplored[PersistentManager.Instance.inLevel].Add(PersistentManager.Instance.localDungeonPos);
                GameObject room = Instantiate(roomMarker);
                room.transform.parent = map.transform;
                room.transform.localPosition = Vector2.zero;
                room.transform.Translate(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f));
                posMarker.transform.position = map.transform.position;
                posMarker.transform.Translate(new Vector2(PersistentManager.Instance.localDungeonPos.x * 0.08f, PersistentManager.Instance.localDungeonPos.y * 0.04f));

                goToRoom = PersistentManager.Instance.tunnelExit;
                if (GameObject.Find(goToRoom).name.Length == 2)
                    PersistentManager.Instance.playerLocation = new Vector2Int(char.ToUpper(GameObject.Find(goToRoom).name[0]) - 64, int.Parse(GameObject.Find(goToRoom).name[1].ToString()));
                else
                    PersistentManager.Instance.playerLocation = new Vector2Int(char.ToUpper(GameObject.Find(goToRoom).name[0]) - 64, int.Parse(GameObject.Find(goToRoom).name[2].ToString()) + 10);
            }
            Tilemap tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
            if (GameObject.Find(goToRoom).GetComponent<BasementEntrances>().exitPosition == 1)
            {
                cam.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(GameObject.Find(goToRoom).transform.position) + new Vector3Int(-5, -2, 0)) + new Vector3(-0.08f, 0.015f, 0);
                for (int i = 0; i < players.Length; i++)
                    players[i].transform.position = cam.transform.position + new Vector3(0.8f, 0.16f, 0);
            }
            if (GameObject.Find(goToRoom).GetComponent<BasementEntrances>().exitPosition == 2)
            {
                cam.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(GameObject.Find(goToRoom).transform.position) + new Vector3Int(0, 1, 0)) + new Vector3(-0.08f, 0.015f, 0);
                for (int i = 0; i < players.Length; i++)
                    players[i].transform.position = cam.transform.position + new Vector3(-0.16f, 0.16f, 0);
            }
            if (GameObject.Find(goToRoom).GetComponent<BasementEntrances>().exitPosition == 3)
            {
                cam.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(GameObject.Find(goToRoom).transform.position) + new Vector3Int(-5, 1, 0)) + new Vector3(-0.08f, 0.015f, 0);
                for (int i = 0; i < players.Length; i++)
                    players[i].transform.position = cam.transform.position + new Vector3(0.7f, -0.16f, 0);
            }
            if (GameObject.Find(goToRoom).GetComponent<BasementEntrances>().exitPosition == 4)
            {
                cam.transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(GameObject.Find(goToRoom).transform.position) + new Vector3Int(-1, 0, 0)) + new Vector3(-0.08f, 0.015f, 0);
                for (int i = 0; i < players.Length; i++)
                    players[i].transform.position = cam.transform.position + new Vector3(0.08f, 0, 0);
            }
        }
    }
}