using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject pauseButtonUI;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text highScoreAnnounceText;
    [SerializeField] TMP_Text MobyDickQuote;
    [SerializeField] TMP_Text diedByText;

    [SerializeField] FloatVariable score;
    [SerializeField] FloatVariable speed;
    [SerializeField] FloatVariable scoreMult;
    [SerializeField] AudioManager audioManager;

    private State state = State.TITLE;
    private bool pause = false;
    string QuotePath = "Assets/Externals/Quotes.txt";
    string HSPath = "Assets/Externals/Highscore.txt";

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
        titleUI.active = true;
        pauseUI.active = false;
        pauseButtonUI.active = false;
        gameOverUI.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.TITLE:
                if (Input.anyKey)
                {
                    titleUI.active = false;
                    Time.timeScale = 1;
                    state = State.SET_GAME;
                }
                break;
            case State.SET_GAME:
                pauseButtonUI.active = true;
                score.value = 0;
                speed.value = speed.initialValue;
                scoreMult.value = scoreMult.initialValue;
                audioManager.Play("Start");
                audioManager.Play("Background");
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

    private string[] GetQuotes()
    {
        string[] quotes = File.ReadAllLines(QuotePath);
        return quotes;
    }
    private void DisplayRandomQuote()
    {
        string[] quotes = GetQuotes();
        int randIndex = UnityEngine.Random.Range(0, quotes.Length);
        MobyDickQuote.text = quotes[randIndex];
    }

    private float GetHighScore()
    {
        string highscore = File.ReadAllText(HSPath);
        float score = 0f;
        try
        {
            score = float.Parse(highscore);
            return score;
        } catch {}
        
        return score;
    }

    private void SetHighscore(float Highscore)
    {
        File.WriteAllText(HSPath, Highscore.ToString());
    }

    public void OnScoreUp(float points)
    {
        if (points < 0)
            audioManager.Play("Hit");
        if (score.value + points < 0)
        {
            score.value = 0;
        }
        else
        {
            score.value += points;
            audioManager.Play("Chomp");
        }
        scoreText.text = "Score: " + score.value.ToString("0000");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseUI.active = true;
        pauseButtonUI.active = false;
        pause = true;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        pauseUI.active = false;
        pauseButtonUI.active = true;
        pause = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void GameOver(string deathMessage)
    {
        audioManager.Play("GameOver");
        gameOverUI.active = true;
        pauseButtonUI.active = false;
        highScoreAnnounceText.enabled = false;
        float highScore = GetHighScore();
        if (score.value > highScore)
        {
            highScore = score.value;
            SetHighscore(highScore);
            highScoreAnnounceText.enabled = true;
        }
        highScoreText.text = "HighScore: " + highScore.ToString("0000");
        finalScoreText.text = "Final Score: " + score.value.ToString("0000");
        if (deathMessage == "You died by the KILLER Orca")
        {
            audioManager.Play("Orca");
        }
        diedByText.text = deathMessage;
        DisplayRandomQuote();
        Time.timeScale = 0;
    }


    public void Quit()
    {
        Application.Quit();
    }
}
