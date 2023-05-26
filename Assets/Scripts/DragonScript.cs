using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private float xStartPos;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject mario;

    //CONST
    private const int DIRECTION = -1;
    private const float SPEED = 0.7f;
    private const float LOCATION_DIFF = 0.2f;
    private const float FIRE_X_1 = 25.2f;
    private const float FIRE_Y_1 = 19.7f;
    private const float DELTA_TIME = 8;
    private const float DIFF = 0.05f;
    private const int MAX_3_RANGE = 3;
    private const int MAX_5_RANGE = 5;
    private const int MARIO_Y = 14;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        xStartPos = myRigidbody2D.position.x;
        myRigidbody2D.velocity = new Vector2(DIRECTION, 0);
    }

    private void Update()
    {
        if (mario.transform.position.y > MARIO_Y)
        {
            if (myRigidbody2D.transform.position.x < xStartPos - LOCATION_DIFF)
            {
                myRigidbody2D.velocity = new Vector2(SPEED, 0);
            }
            else if (myRigidbody2D.transform.position.x > xStartPos + LOCATION_DIFF)
            {
                myRigidbody2D.velocity = new Vector2(-SPEED, 0);
            }

            if (myRigidbody2D.velocity.magnitude < SPEED)
            {
                myRigidbody2D.AddForce(new Vector2(1, 0));
            }

            if (myRigidbody2D.position.x > xStartPos + LOCATION_DIFF)
            {
                Vector2 firstLoc = new Vector2(myRigidbody2D.position.x - DIFF, myRigidbody2D.position.y - DIFF);
                Vector2 secondLoc = new Vector2(FIRE_X_1, FIRE_Y_1);
                if (Random.Range(0, MAX_5_RANGE) == 1)
                {
                    Destroy(Instantiate(fire, firstLoc, Quaternion.identity), DELTA_TIME);
                }

                if (Random.Range(0, MAX_3_RANGE) == 1)
                {
                    Destroy(Instantiate(fire, secondLoc, Quaternion.identity), DELTA_TIME);
                }
            }
        }
    }
}