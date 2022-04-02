using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingController : MonoBehaviour
{

    public GameObject part1;
    public GameObject part2;
    public int position;
    private Collider2D player;
    public SpriteRenderer[] signs;
    public SpriteRenderer[] digits;
    public Sprite[] numbers;

    // Use this for initialization
    void Start()
    {
        numbers = Resources.LoadAll<Sprite>("Numbers");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision;
            if (collision.GetComponent<PlayerController>().rupees >= 10)
            {
                int successPosition = Random.Range(1, 4);
                int failPosition = Random.Range(1, 4);
                while(successPosition == failPosition)
                    failPosition = Random.Range(1, 4);
                int successAmount = Random.Range(1, 3);
                int failAmount = Random.Range(1, 4);
                signs[successPosition - 1].sprite = numbers[11];
                if (failAmount == 1)
                    digits[failPosition - 1].sprite = numbers[1];
                if (failAmount == 2)
                    digits[failPosition - 1].sprite = numbers[2];
                if (failAmount == 3)
                    digits[failPosition - 1].sprite = numbers[3];
                if(successAmount == 1)
                    digits[successPosition - 1].sprite = numbers[2];
                if(successAmount == 2)
                    digits[successPosition - 1].sprite = numbers[4];
                if (successPosition == position)
                {
                    if (successAmount == 1)
                        Rupees(20);
                    else
                        Rupees(40);
                }
                else if(failPosition == position)
                {
                    if (failAmount == 1)
                        Rupees(-10);
                    if (failAmount == 2)
                        Rupees(-20);
                    if (failAmount == 3)
                        Rupees(-30);
                }
                else
                {
                    Rupees(-10);
                }
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                part1.GetComponent<BoxCollider2D>().enabled = false;
                part2.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(Reset());
            }
        }
    }
    IEnumerator Reset()
    {
        yield return StartCoroutine(WaitFor.Frames(300));
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        part1.GetComponent<BoxCollider2D>().enabled = true;
        part2.GetComponent<BoxCollider2D>().enabled = true;
        signs[0].sprite = numbers[10];
        signs[1].sprite = numbers[10];
        signs[2].sprite = numbers[10];
        digits[0].sprite = numbers[1];
        digits[1].sprite = numbers[1];
        digits[2].sprite = numbers[1];
    }
    void Rupees(int amount)
    {
        Debug.Log(amount);
        if (player.GetComponent<PlayerController>().rupees + amount >= 0 && player.GetComponent<PlayerController>().rupees + amount <= 999)
            player.GetComponent<PlayerController>().AddRupees(amount);
        else
        {
            if (amount > 0)
                player.GetComponent<PlayerController>().AddRupees(999 - player.GetComponent<PlayerController>().rupees);
            else
                player.GetComponent<PlayerController>().AddRupees(-player.GetComponent<PlayerController>().rupees);
        }
    }
}