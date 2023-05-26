using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRightLeftMove : MonoBehaviour
{
    //Const
    private const int FLIP = 180;
    private const float ENEMY_SPEED = 0.3f;
    private const int DIRECTION = 1;
    private const int FLIP_DIRECTION = -1;

    private Rigidbody2D myRigidbody2D;
    private int direction = DIRECTION;
    public float enemySpeed = ENEMY_SPEED;
    
    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.velocity = new Vector2(1, 0);
    }

    private void Update()
    {
        myRigidbody2D.velocity = new Vector2(direction * enemySpeed, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (CompareTag("Bird"))
        {
            direction *= FLIP_DIRECTION;
            transform.Rotate(0, FLIP, 0);
        }
        else if (col.transform.tag == "ShortPipe" || col.transform.tag == "LongPipe" ||
                 col.transform.tag == "VeryLongPipe" ||
                 col.transform.tag == "MushroomEnemy" || col.transform.tag == "SideBrick" ||
                 col.transform.tag == "SmallMario")
        {
            direction *= FLIP_DIRECTION;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Map")
        {
            direction = 0;
        }
    }
}