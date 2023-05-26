using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionBrickScript : MonoBehaviour
{
    private Animator questionMoveUp;
    private SpriteRenderer sprite;
    [SerializeField] private Sprite pileBrickSprite;
    [SerializeField] private GameObject[] prizes;
    private float xPos;
    private float yPos;
    private GameObject parent;
    private int isWithprize;
    private bool firstTime;
    private GameObject allBricks;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject mushroomUpgrade;

    //CONST
    private const int RANDOM_RANGE = 4;
    private const float COLLISION_DIFF = 0.12f;
    private const float DELTA_TIME = 7;
    private const int IS_0 = 0;
    private const int IS_1 = 1;
    private const int IS_2 = 2;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        questionMoveUp = gameObject.GetComponent<Animator>();
        isWithprize = Random.Range(0, RANDOM_RANGE);
        firstTime = true;

        allBricks = GameObject.FindGameObjectWithTag("AllBricks");
        xPos = transform.position.x;
        yPos = transform.position.y;
        parent = new GameObject("questionParent");
        parent.transform.position = new Vector3(xPos, yPos, 0);
        transform.SetParent(parent.transform);
        parent.transform.SetParent(allBricks.transform);
        transform.position = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (firstTime)
        {
            if (col.gameObject.tag == "SmallMario" && col.transform.position.y + COLLISION_DIFF < transform.position.y)
            {
                if (col.transform.GetComponent<SmallMarioScript>().IsLuigi() == false)
                {
                    questionMoveUp.Play("QuestionBrickSmallMarioHit");
                    firstTime = false;
                    if (isWithprize == IS_0 || isWithprize == IS_1)
                    {
                        Instantiate(coin, transform.position, Quaternion.identity);
                    }
                    else if (isWithprize == IS_2)
                    {
                        Vector3 newLoc = new Vector3(transform.position.x, transform.position.y + COLLISION_DIFF,
                            transform.position.z);
                        Destroy(Instantiate(clock, newLoc, Quaternion.identity), DELTA_TIME);
                    }
                    else
                    {
                        Vector3 newLoc = new Vector3(transform.position.x, transform.position.y + COLLISION_DIFF,
                            transform.position.z);
                        Destroy(Instantiate(mushroomUpgrade, newLoc, Quaternion.identity), DELTA_TIME);
                    }
                }
            }
        }
    }
}