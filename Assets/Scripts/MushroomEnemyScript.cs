using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemyScript : MonoBehaviour
{
    // Constants
    private const float HALF = 0.5f;
    private const int FIRST_SCORE = 100;
    private const int SECOND_SCORE = 100;
    private const float TIME_DELEY = 0.07f;

    private ManegeTitlesScript titles;
    private Animator mushroomAnimator;
    [SerializeField] private GameObject hundred;
    [SerializeField] private GameObject twoHundred;

    private void Start()
    {
        titles = GameObject.FindGameObjectsWithTag("UpperTitles")[0].GetComponent<ManegeTitlesScript>();
        mushroomAnimator = gameObject.GetComponent<Animator>();
    }

    public void MushroomKilledByMario()
    {
        if (GameObject.FindGameObjectsWithTag("100").Length > 0)
        {
            Destroy(Instantiate(twoHundred, transform.position, Quaternion.identity), HALF);
            titles.AddScore(SECOND_SCORE);
        }
        else
        {
            Destroy(Instantiate(hundred, transform.position, Quaternion.identity), HALF);
            titles.AddScore(FIRST_SCORE);
        }

        StartCoroutine(WaitDie());
    }

    private IEnumerator WaitDie()
    {
        mushroomAnimator.SetBool("Die", true);
        yield return new WaitForSeconds(TIME_DELEY);
        Destroy(gameObject);
    }
}