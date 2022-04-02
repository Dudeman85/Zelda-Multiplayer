using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementTestScript : MonoBehaviour {

    private Rigidbody2D move;
    public float moveSpeed;
    public float moveTiles;

	// Use this for initialization
	void Start () {
        move = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (moveTiles > 0)
        {
            moveTiles -= 0.03125f * moveSpeed;
            transform.Translate(0.005f * moveSpeed, 0f, 0f);
        }
        else
        {
            move.velocity = new Vector2(0f, 0f);
            moveTiles = 0;
        }
	}
}
