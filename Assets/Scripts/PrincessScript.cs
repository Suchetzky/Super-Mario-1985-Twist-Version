using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PrincessScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SmallMario"))
        {
            SceneManager.LoadSceneAsync("EndGame");
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}