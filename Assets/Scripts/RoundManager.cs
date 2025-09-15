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
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
    }
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
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
    }
}
