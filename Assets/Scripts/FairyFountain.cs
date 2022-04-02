using UnityEngine;

public class FairyFountain : MonoBehaviour
{
    public GameObject hearts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponents<BoxCollider2D>()[1].enabled = true;
        StartCoroutine(collision.GetComponent<PlayerHealthManager>().FairyFountain(collision.gameObject, PersistentManager.Instance.maxHealth - collision.GetComponent<PlayerHealthManager>().health, gameObject));
        StartCoroutine(collision.GetComponent<PlayerHealthManager>().FairyFountainHearts(hearts, collision.gameObject, PersistentManager.Instance.maxHealth - collision.GetComponent<PlayerHealthManager>().health, gameObject));
    }
}
