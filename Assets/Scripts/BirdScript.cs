using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    private ManegeTitlesScript titles;
    public GameObject hundred;

    //CONST
    private const int HUNDRED = 100;
    private const float DELTA_TIME = 0.5f;

    private void Start()
    {
        titles = GameObject.FindGameObjectsWithTag("UpperTitles")[0].GetComponent<ManegeTitlesScript>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "SmallMario")
        {
            if (col.transform.position.y > transform.position.y)
            {
                titles.AddScore(HUNDRED);
                Destroy(Instantiate(hundred, transform.position, Quaternion.identity), DELTA_TIME);
                Destroy(gameObject);
            }
        }
    }
}