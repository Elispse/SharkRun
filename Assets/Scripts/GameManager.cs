using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Canvas titleUI;
    [SerializeField] Canvas gameOverUI;
    [SerializeField] Canvas pauseUI;
    [SerializeField] Canvas pauseButtonUI;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text highScoreAnnounceText;
    [SerializeField] TMP_Text diedByText;

    [SerializeField] FloatVariable score;
    [SerializeField] FloatVariable highScore;
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;

    private State state = State.TITLE;
    private bool pause = false;

    public enum State
    {
        TITLE,
        SET_GAME,
        PLAY_GAME,
    }

    // Start is called before the first frame update
    void Start()
    {
        speed.value = speed.initialValue;
        scoreMult.value = scoreMult.initialValue;
        Time.timeScale = 0;
        titleUI.enabled = true;
        pauseUI.enabled = false;
        pauseButtonUI.enabled = false;
        gameOverUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.TITLE:
                if (Input.anyKey)
                {
                    titleUI.enabled = false;
                    Time.timeScale = 1;
                    state = State.SET_GAME;
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
                scoreMult.value += speed.value * (Time.deltaTime / 16);
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
        }
    }

    public void OnScoreUp(float points)
    {
        score.value = (score.value + points) < 0 ? 0 : score.value;
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void GameOver(string deathMessage)
    {
        gameOverUI.enabled = true;
        pauseButtonUI.enabled = false;
        highScoreAnnounceText.enabled = false;
        if (score.value > highScore)
        {
            highScore.value = score.value;
            highScoreText.text = "HighScore: " + highScore.value.ToString("0000");
            highScoreAnnounceText.enabled = true;
        }
        finalScoreText.text = "Final Score: " + score.value.ToString("0000");
        diedByText.text = deathMessage;
        Time.timeScale = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
