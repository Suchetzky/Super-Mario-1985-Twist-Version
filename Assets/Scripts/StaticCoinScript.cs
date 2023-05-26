using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCoinScript : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    private GameObject myCoin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        myCoin = Instantiate(coin);
        myCoin.GetComponent<SpriteRenderer>().color = Color.clear;
        Destroy(gameObject);
    }
}