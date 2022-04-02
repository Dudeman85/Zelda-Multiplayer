using UnityEngine;

public class FairyFountainHearts : MonoBehaviour
{
    private float angle = 90;
    
    void Update()
    {
        transform.localPosition = new Vector2(Mathf.Cos(angle) / 2, Mathf.Sin(angle) / 2 - 0.20f);
        angle += 0.05f;
    }
}
