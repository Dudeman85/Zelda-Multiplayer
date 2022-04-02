using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage;
    public int deflectable = 0; //0 = no, 1 = big shield, 2 = small shield

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerHealthManager>().HitPlayer(damage, deflectable, GetComponent<BoxCollider2D>());
        }
    }
}
