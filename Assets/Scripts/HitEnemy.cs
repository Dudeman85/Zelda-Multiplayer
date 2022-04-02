using UnityEngine;

public class HitEnemy : MonoBehaviour
{

    public int damage;
    public bool stun;
    private int direction; //Down = 0, Right = 1, Up = 2, Left = 3
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Get proper knockback angles
        if(name == "Sword")
            direction = GetComponentInParent<PlayerController>().facing;
        if (name.Contains("Sword Beam"))
        {
            direction = (int)transform.eulerAngles.z / 90 - 1;
            if (direction == 0)
                direction = 4;
            if (direction == -1)
                direction = 3;

        }
        //Apply knockback to enemy
        if (other.gameObject.tag == "Enemy")
        {
            if (!stun)
                other.GetComponent<EnemyHealthManager>().HitEnemy(damage, direction);
            else
                other.GetComponent<EnemyHealthManager>().HitEnemy(0, -1);
        }
    }
}