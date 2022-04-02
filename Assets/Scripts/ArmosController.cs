using System.Collections;
using UnityEngine;

public class ArmosController : MonoBehaviour
{
    
    private SpriteRenderer shader;
    public Sprite defaultSprite;

    // Use this for initialization
    void Start()
    {
        shader = GetComponent<SpriteRenderer>();
        shader.material.SetInt("_CurrentPallette", 0);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
            StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        shader.sprite = defaultSprite;
        for (int i = 0; i < 20; i++)
        {
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 4);
            yield return StartCoroutine(WaitFor.Frames(2));
            shader.material.SetInt("_CurrentPallette", 2);
        }
        gameObject.layer = 12;
        GetComponent<EnemyHealthManager>().enabled = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<EnemyController>().enabled = true;
        Destroy(this);
    }
}
