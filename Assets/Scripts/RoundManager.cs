using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;

    private bool endingRound = false;

    private Board board;

    public int currentScore = 0;
    public float displayScore = 0;
    public float scoreSpeed = 5f;

    public int scoreTarget1;
    public int scoreTarget2;
    public int scoreTarget3;
    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        board = FindAnyObjectByType<Board>();
    }
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;
        }
        else
        {
            roundTime = 0;

            endingRound = true;
        }

        if (endingRound && board.currentState == Board.BoardState.move)
        {
            endingRound = false;
            WinCheck();
        }

        uiManager.timeText.text = roundTime.ToString("F1") + "s";

        displayScore = Mathf.Lerp(displayScore, currentScore, scoreSpeed * Time.deltaTime);
        uiManager.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);

        uiManager.winScore.text = currentScore.ToString();

        if (currentScore >= scoreTarget3)
        {
            uiManager.winStars3.SetActive(true);
            uiManager.winText.text = "Amazing! You earn 3 stars";
        }
        else if (currentScore >= scoreTarget2)
        {
            uiManager.winStars2.SetActive(true);
            uiManager.winText.text = "Great job! You earn 2 stars";
        }
        else if (currentScore >= scoreTarget1)
        {
            uiManager.winStars1.SetActive(true);
            uiManager.winText.text = "Good job! You earn 1 star";
        }
        else
        {
            uiManager.winText.text = "You failed!";
        }
    }
}
