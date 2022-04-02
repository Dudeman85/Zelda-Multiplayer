using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    public float gameSpeed = 1;
    private Transform mapMarker;
    private GameObject[] players;

    private static bool cameraExists;

    //Debug
    public Text text;
    private string debugText;
    public bool debugOn = false;
    private bool freecam = true;
    private bool invincible = false;
    private bool collision = true;
    private float fov;

    private void Awake()
    {
        transform.position = new Vector3(-1.28f, -5.986f, -10f);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        mapMarker = GameObject.Find("Position Marker").transform;
        cam = GetComponent<Camera>();
        fov = 1.0538f;
        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mapMarker.localPosition = new Vector3(transform.position.x / 64, transform.position.y / 44, 0);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        cam.orthographicSize = fov;

        if (Input.GetKeyDown("[/]"))
        {
            debugOn = !debugOn;
        }

        //Debug
        if (debugOn)
        {
            if (freecam)
            {
                if (Input.GetAxisRaw("CamHorizontal") != 0)
                    transform.Translate(Input.GetAxisRaw("CamHorizontal") / 20, 0f, 0f);

                if (Input.GetAxisRaw("CamVertical") != 0)
                    transform.Translate(0f, Input.GetAxisRaw("CamVertical") / 20, 0f);
            }
            else
            {
                if (Input.GetAxisRaw("CamHorizontal") != 0)
                    transform.Translate(Input.GetAxisRaw("CamHorizontal") / 20, 0f, 0f);

                if (Input.GetAxisRaw("CamVertical") != 0)
                    transform.Translate(0f, Input.GetAxisRaw("CamVertical") / 20, 0f);
            }

            if (Input.GetKey("[7]"))
                fov += 0.02f;

            if (Input.GetKey("[9]"))
                fov -= 0.02f;

            if (Input.GetKeyDown("[-]"))
            {
                Time.timeScale -= 0.1f;
                gameSpeed -= 0.1f;
            }

            if (Input.GetKeyDown("[+]"))
            {
                Time.timeScale += 0.1f;
                gameSpeed += 0.1f;
            }

            if (Input.GetKeyDown("[0]"))
            {
                Debug.Log("Wrote to Debug File At: 'E:\\Users\\Aleksi Anderson\\Desktop\\Zelda Multiplayer\\Debug.txt'");
                string text = PersistentManager.Instance.inLevel.ToString() + ": ";
                foreach (Vector2 i in PersistentManager.Instance.dungeonDoorsExplored[PersistentManager.Instance.inLevel])
                    text += "new Vector2(" + i.x + ", " + i.y + "), ";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\Users\Aleksi Anderson\Desktop\Zelda Multiplayer\Debug.txt", true))
                    file.WriteLine(text);
            }

            if (Input.GetKeyDown("[1]"))
                freecam = !freecam;

            if (Input.GetKeyDown("[2]"))
            {
                if (!invincible)
                    foreach (GameObject player in players)
                        player.GetComponent<PlayerHealthManager>().invincibility = 99999999;

                if (invincible)
                    foreach (GameObject player in players)
                        player.GetComponent<PlayerHealthManager>().invincibility = 0;

                invincible = !invincible;
            }

            if (Input.GetKeyDown("[3]"))
            {
                collision = !collision;
                foreach (GameObject player in players)
                    player.GetComponent<BoxCollider2D>().enabled = !player.GetComponent<BoxCollider2D>().enabled;
            }

            debugText = "Debug:\n" + "Freecam [1]: " + freecam + "\nInvincibility [2]: " + invincible + "\nCollision [3]:" + collision + "\nZoom [7 9]:" + fov + "\nSpeed [+ -]: " + gameSpeed + "\nPosition: " + PersistentManager.Instance.playerLocation + "\nDump [0]";
            text.text = debugText;
        }
        else
        {
            text.text = "";
        }
    }
}