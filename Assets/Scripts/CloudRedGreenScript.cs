using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudRedGreenScript : MonoBehaviour
{
    private bool redFlag = false;
    private SpriteRenderer render;
    private SmallMarioScript marioScript;
    private bool canDieFlag = true;

    //Const
    private const int DELTA = 2;
    private const float R_RED = 0.709f;
    private const float G_RED = 0.192f;
    private const float B_RED = 0.1215f;
    private const float R_GREEN = 0.2117f;
    private const float G_GREEN = 0.5294f;
    private const float B_GREEN = 0.007f;
    private const int RANGE_RAND = 2;
    private const int RED = 1;

    private void Start()
    {
        render = transform.GetComponent<SpriteRenderer>();
        if (Random.Range(0, RANGE_RAND) == RED)
        {
            render.color = new Color(R_RED, G_RED, B_RED);

            redFlag = true;
        }
        else
        {
            render.color = new Color(R_GREEN, G_GREEN, B_GREEN);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.transform.CompareTag("SmallMario"))
        {
            marioScript = col.transform.GetComponent<SmallMarioScript>();
            if (redFlag)
            {
                if (marioScript.IsLuigi() && !marioScript.IsBig() && canDieFlag)
                {
                    marioScript.MarioDie();
                }

                if (marioScript.IsLuigi() && marioScript.IsBig())
                {
                    canDieFlag = false;
                    StartCoroutine(FlagWait());
                    marioScript.MakeSmallPublic();
                }
            }
            else
            {
                if (marioScript.IsLuigi() == false && !marioScript.IsBig() && canDieFlag)
                {
                    marioScript.MarioDie();
                }

                if (marioScript.IsLuigi() == false && marioScript.IsBig())
                {
                    canDieFlag = false;
                    StartCoroutine(FlagWait());
                    marioScript.MakeSmallPublic();
                }
            }
        }
    }

    private IEnumerator FlagWait()
    {
        yield return new WaitForSeconds(DELTA);
        canDieFlag = true;
    }
}