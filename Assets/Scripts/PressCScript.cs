using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressCScript : MonoBehaviour
{
    [SerializeField] private GameObject mario;
    private Rigidbody2D marioRigid;

    //CONST
    private const int CHANGE = -2;

    private void Start()
    {
        marioRigid = mario.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (marioRigid.transform.position.x > CHANGE)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}