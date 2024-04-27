using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas titleUI;
    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas pauseUI;
    [SerializeField] Canvas pauseButtonUI;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] FloatVariable score;
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;

    private State state = State.TITLE;
    private bool pause = false;

    public enum State
    {
        TITLE,
        SET_GAME,
        PLAY_GAME,
        GAME_OVER
    }

    // Start is called before the first frame update
    void Start()
    {
        speed.value = speed.initialValue;
        scoreMult.value = scoreMult.initialValue;
        Time.timeScale = 0;
        pauseUI.enabled = false;
        pauseButtonUI.enabled = false;
        gameOverUI.enabled = false;
        titleUI.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.TITLE:
                if (Input.anyKey)
                {
                    state = State.SET_GAME;
                    titleUI.enabled = false;
                    Time.timeScale = 1;
                }
                break;
            case State.SET_GAME:
                pauseButtonUI.enabled = true;
                score.value = 0;
                speed.value = speed.initialValue;
                scoreMult.value = scoreMult.initialValue;
                state = State.PLAY_GAME;
                break;
            case State.PLAY_GAME:
                score.value += scoreMult.value * Time.deltaTime;
                scoreText.text = "Score: " + score.value.ToString("0000");
                if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
                {
                    Pause();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && pause == true)
                {
                    UnPause();
                }
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

    public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.enabled = true;
        pauseButtonUI.enabled = false;
        pause = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseUI.enabled = false;
        pauseButtonUI.enabled = true;
        pause = false;
    }

    public void GameOver()
    {
        gameOverUI.enabled = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
