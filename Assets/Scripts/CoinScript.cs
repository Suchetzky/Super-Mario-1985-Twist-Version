using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    //CONST
    private const int UP_FORCE = 40;
    private const int COIN_SCORE = 200;

    private ManegeTitlesScript titles;
    private Rigidbody2D myRigidbody2D;
    private float saveY;
    private GameObject coinsTitle;
    private string numOfCoinsString;
    private int curNumOfCoins;
    private GameObject scoreTitle;
    private string scoreString;
    private int curScore;

    private void Start()
    {
        saveY = transform.position.y;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.AddForce(new Vector2(0, UP_FORCE));
        titles = GameObject.FindGameObjectsWithTag("UpperTitles")[0].GetComponent<ManegeTitlesScript>();

        // Add coin to count.
        AddCoin();

        // Update Score
        titles.AddScore(COIN_SCORE);
    }

    private void Update()
    {
        if (transform.position.y < saveY)
        {
            Destroy(gameObject);
        }
    }

    private void AddCoin()
    {
        coinsTitle = GameObject.FindGameObjectsWithTag("CoinsTitle")[0];
        numOfCoinsString = coinsTitle.GetComponent<TextMeshProUGUI>().text.Split(" ")[1];
        curNumOfCoins = int.Parse(numOfCoinsString);

        curNumOfCoins++;
        numOfCoinsString = curNumOfCoins.ToString();
        coinsTitle.GetComponent<TextMeshProUGUI>().text = "COINS \n" + numOfCoinsString;
    }
}