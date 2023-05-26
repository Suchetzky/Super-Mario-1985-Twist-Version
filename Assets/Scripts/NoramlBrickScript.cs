using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoramlBrickScript : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    private GameObject allBricks;
    private Animator brickMoveUp;

    private float xPos;
    private float yPos;
    private bool isWithCoin;
    private GameObject parent;

    //CONST
    private const int RANDOM_RANGE = 4;
    private const int WITH_COIN = 1;
    private const float DIFF = 0.12f;
    private const float DELTA_TIME = 0.7f;

    private void Start()
    {
        //Random Coin per Brick
        if (Random.Range(0, RANDOM_RANGE) == WITH_COIN)
        {
            isWithCoin = true;
        }
        else
        {
            isWithCoin = false;
        }

        brickMoveUp = gameObject.GetComponent<Animator>();
        allBricks = GameObject.FindGameObjectWithTag("AllBricks");
        xPos = transform.position.x;
        yPos = transform.position.y;
        parent = new GameObject("brickParent");
        parent.transform.position = new Vector3(xPos, yPos, 0);
        transform.SetParent(parent.transform);
        parent.transform.SetParent(allBricks.transform);
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "SmallMario" && col.transform.position.y + DIFF < transform.position.y)
        {
            brickMoveUp.Play("NormalBrickSmallMarioHit");
            if (isWithCoin)
            {
                isWithCoin = false;
                Instantiate(coin, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "SmallMario")
        {
            Invoke("StopAnimation", DELTA_TIME);
        }
    }

    private void StopAnimation()
    {
        brickMoveUp.Play("NormalBrickStay");
    }
}