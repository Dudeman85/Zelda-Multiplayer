using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausingController : MonoBehaviour
{

    private bool paused;
    private bool selecting;
    private int pauseCooldown;
    private int selectedOption;
    public GameObject[] players;
    public List<string> UITexts;
    public GameObject pauseMenu;
    public GameObject selector;
    public GameObject text;
    public GameObject cont;
    public GameObject save;
    public GameObject quit;

    // Use this for initialization
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseCooldown > 0)
            pauseCooldown--;
        else if (Input.GetAxisRaw("Select") == 1 || Input.GetAxisRaw("Select2") == 1 || Input.GetAxisRaw("Select3") == 1 || Input.GetAxisRaw("Select4") == 1)
        {
            if (!paused && !PersistentManager.Instance.disablePausing)
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                //Disable Everything
                paused = true;
                pauseCooldown = 30;
                selecting = true;
                pauseMenu.SetActive(true);
                PersistentManager.Instance.disableUI = true;
                GameObject.Find("UI").GetComponent<UIController>().enabled = false;
                for (var i = 0; i < players.Length; i++)
                {
                    players[i].SetActive(false);
                }
                FindObjectOfType<MusicManager>().GetComponent<AudioSource>().Pause();
                //Set UI Items
                selectedOption = 1;
                selector.transform.localPosition = new Vector2(-0.4f, 0.471f);
                quit.GetComponent<Text>().color = Color.white;
                //Random UI Text
                text.GetComponent<Text>().text = UITexts[Random.Range(0, UITexts.Capacity)];
                //Pause Enemies
                foreach(GameObject enemy in FindObjectOfType<EnemySpawnManager>().enemies)
                {
                    if(enemy)
                        enemy.SetActive(false);
                }
            }
        }
        if (paused)
        {
            //No Vertical Input
            if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Vertical2") == 0 && Input.GetAxisRaw("Vertical3") == 0 && Input.GetAxisRaw("Vertical4") == 0 && Input.GetAxisRaw("Select") == 0 && Input.GetAxisRaw("Select2") == 0 && Input.GetAxisRaw("Select3") == 0 && Input.GetAxisRaw("Select4") == 0)
                selecting = false;
            if (pauseCooldown == 0)
            {
                //Select Up/Down
                if (!selecting)
                {
                    if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical2") > 0 || Input.GetAxisRaw("Vertical3") > 0 || Input.GetAxisRaw("Vertical4") > 0)
                        SelectUp();
                    if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Vertical2") < 0 || Input.GetAxisRaw("Vertical3") < 0 || Input.GetAxisRaw("Vertical4") < 0)
                        SelectDown();
                    if (Input.GetAxisRaw("Select") == 1 || Input.GetAxisRaw("Select2") == 1 || Input.GetAxisRaw("Select3") == 1 || Input.GetAxisRaw("Select4") == 1)
                        SelectDown();
                }
                //Confirm Option With A
                if (Input.GetAxisRaw("Attack") == 1 || Input.GetAxisRaw("Attack2") == 1 || Input.GetAxisRaw("Attack3") == 1 || Input.GetAxisRaw("Attack4") == 1 || Input.GetAxisRaw("Start") == 1 || Input.GetAxisRaw("Start2") == 1 || Input.GetAxisRaw("Start3") == 1 || Input.GetAxisRaw("Start4") == 1)
                {
                    //Continue
                    if (selectedOption == 1)
                    {
                        pauseCooldown = 300;
                        StartCoroutine(FlashText(cont.GetComponent<Text>()));
                    }
                    //Save
                    if (selectedOption == 2)
                    {
                        pauseCooldown = 300;
                        if(PersistentManager.Instance.saveSlot == 1)
                            SaveSystem.SaveGame("/save1.despacito");
                        if (PersistentManager.Instance.saveSlot == 2)
                            SaveSystem.SaveGame("/save2.despacito");
                        if (PersistentManager.Instance.saveSlot == 3)
                            SaveSystem.SaveGame("/save3.despacito");
                        StartCoroutine(FlashText(save.GetComponent<Text>()));
                    }
                    //Quit
                    if (selectedOption == 3)
                    {
                        pauseCooldown = 300;
                        if (PersistentManager.Instance.saveSlot == 1)
                            SaveSystem.SaveGame("/save1.despacito");
                        if (PersistentManager.Instance.saveSlot == 2)
                            SaveSystem.SaveGame("/save2.despacito");
                        if (PersistentManager.Instance.saveSlot == 3)
                            SaveSystem.SaveGame("/save3.despacito");
                        StartCoroutine(Quit());
                        StartCoroutine(FlashText(quit.GetComponent<Text>()));
                    }
                }
            }
        }
    }

    void SelectUp()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedOption == 1)
        {
            selectedOption = 3;
            selector.transform.Translate(new Vector2(0, -0.32f));
        }
        else
        {
            selectedOption--;
            selector.transform.Translate(new Vector2(0, 0.16f));
        }
    }
    void SelectDown()
    {
        FindObjectOfType<SFXManager>().PlaySound("Rupee");
        selecting = true;
        if (selectedOption == 3)
        {
            selectedOption = 1;
            selector.transform.Translate(new Vector2(0, 0.32f));
        }
        else
        {
            selectedOption++;
            selector.transform.Translate(new Vector2(0, -0.16f));
        }
    }
    public void Continue()
    {
        //Enable Everything
        quit.GetComponent<Text>().color = Color.white;
        paused = false;
        pauseCooldown = 30;
        pauseMenu.SetActive(false);
        PersistentManager.Instance.disableUI = false;
        GetComponent<UIController>().enabled = true;
        for (var i = 0; i < players.Length; i++)
            players[i].SetActive(true);
        FindObjectOfType<MusicManager>().GetComponent<AudioSource>().UnPause();
    }
    IEnumerator Quit()
    {
        Destroy(GameObject.Find("Game Manager"));
        yield return StartCoroutine(WaitFor.Frames(50));
        SceneManager.LoadSceneAsync("Title");
        for (var i = 0; i < players.Length; i++)
        {
            players[i].SetActive(true);
            players[i].transform.position = new Vector2(-10, 0);
        }
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector2(-10, 0);
        GameObject.Find("Black Screen").GetComponent<SpriteRenderer>().enabled = true;
        FindObjectOfType<MusicManager>().ChangeSong("Intro");
        FindObjectOfType<MusicManager>().GetComponent<AudioSource>().UnPause();
        Continue();
    }
    IEnumerator FlashText(Text flashText)
    {
        foreach (GameObject enemy in FindObjectOfType<EnemySpawnManager>().enemies)
            if(enemy)
                enemy.SetActive(true);
        for (var i = 0; i < 10; i++)
        {
            yield return StartCoroutine(WaitFor.Frames(3));
            flashText.color = Color.red;
            yield return StartCoroutine(WaitFor.Frames(3));
            flashText.color = Color.white;
        }
        Continue();
    }
}
