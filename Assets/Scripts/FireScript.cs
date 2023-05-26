using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    public SmallMarioScript mario;
    private float randomY;

    //CONST
    private const int FIRE_SPEED = -2;
    private const int RANGE = 2;

    private void Start()
    {
        randomY = Random.value;
        if (Random.Range(-RANGE, RANGE) < 0)
        {
            randomY = -randomY;
        }

        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.velocity = new Vector2(FIRE_SPEED, randomY);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "SmallMario")
        {
            mario = col.gameObject.GetComponent<SmallMarioScript>();
            if (Time.time - mario.LastFireHit() > 1)
            {
                if (mario.IsBig())
                {
                    mario.MakeSmallPublic();
                }
                else
                {
                    mario.MarioDie();
                }
            }
        }
    }
}