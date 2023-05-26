using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManegeTitlesScript : MonoBehaviour
{
    [SerializeField] private GameObject score;
    [SerializeField] private GameObject time;
    [SerializeField] private GameObject world;
    [SerializeField] private GameObject coins;
    [SerializeField] private GameObject lives;
    [SerializeField] private SmallMarioScript mario;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI worldText;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI livesText;

    //Time
    private float startTime = 0;
    private int timedivide;
    private int pauseTime;
    private bool pauseFleg = false;
    public int totalTimeInput = TOTAL_TIME_INPUT;
    private int totalTime;

    //Score
    private GameObject scoreTitle;
    private string scoreString;
    private int curScore;

    //Const
    private const float DIVIDE_TIME = 0.4f;
    public const int TOTAL_TIME_INPUT = 400;

    private void Start()
    {
        scoreText = score.GetComponent<TextMeshProUGUI>();
        timeText = time.GetComponent<TextMeshProUGUI>();
        worldText = world.GetComponent<TextMeshProUGUI>();
        coinsText = coins.GetComponent<TextMeshProUGUI>();
        livesText = lives.GetComponent<TextMeshProUGUI>();
        totalTime = totalTimeInput;
        startTime = Time.time;
    }

    private void Update()
    {
        UpdateTime(Time.time);
    }

    private void UpdateTime(float number)
    {
        // In death start time will update to cur time.

        timedivide = (int)((number - startTime) / DIVIDE_TIME);
        if (pauseFleg)
        {
            timedivide = pauseTime;
        }

        timeText.text = $"TIME\n{totalTime - timedivide}";

        if (totalTime - timedivide == 0 && !pauseFleg)
        {
            mario.MarioDie();
        }
    }

    public void AddTime(int num)
    {
        totalTime += num;
    }

    public void PauseTime()
    {
        pauseFleg = true;
        pauseTime = (int)((Time.time - startTime) / DIVIDE_TIME);
    }

    public void ResetTime()
    {
        startTime = Time.time;
        totalTime = totalTimeInput;
        pauseFleg = false;
    }

    public void AddScore(int num)
    {
        scoreTitle = GameObject.FindGameObjectsWithTag("ScoreTitle")[0];
        scoreString = scoreTitle.GetComponent<TextMeshProUGUI>().text.Split(" ")[1];
        curScore = int.Parse(scoreString);

        curScore += num;
        scoreString = curScore.ToString();
        scoreTitle.GetComponent<TextMeshProUGUI>().text = "SCORE \n" + scoreString;
    }
}