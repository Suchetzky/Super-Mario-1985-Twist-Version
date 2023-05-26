using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private ManegeTitlesScript timeTitle;

    //CONST
    private const int X_SPEED = 6;
    private const int Y_SPEED = 3;
    private const int EXTRA_TIME = 20;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.AddForce(new Vector2(X_SPEED, Y_SPEED));
        timeTitle = GameObject.FindGameObjectsWithTag("UpperTitles")[0].GetComponent<ManegeTitlesScript>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "SmallMario")
        {
            timeTitle.AddTime(EXTRA_TIME);
            Destroy(gameObject);
        }
    }
}