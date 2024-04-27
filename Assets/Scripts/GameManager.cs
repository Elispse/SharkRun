using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] FloatVariable score;
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;

    private State state = State.PLAY_GAME;

    public enum State
    {
        TITLE,
        SET_GAME,
        START_GAME,
        PLAY_GAME,
        GAME_OVER
    }

    // Start is called before the first frame update
    void Start()
    {
        speed.value = speed.initialValue;
        scoreMult.value = scoreMult.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.TITLE:

                break;
            case State.SET_GAME:

                break;
            case State.START_GAME:

                break;
            case State.PLAY_GAME:
                score.value += scoreMult.value * Time.deltaTime;
                scoreText.text = "Score: " + score.value.ToString("0000");
                break;
            case State.GAME_OVER:

                break;
        }
    }

    public void OnScoreUp(float points)
    {
        score.value += points;
        scoreText.text = "Score: " + score.value.ToString("0000");
    }

    public void GameOver()
    {

    }
}
