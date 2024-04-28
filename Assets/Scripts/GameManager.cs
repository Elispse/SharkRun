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
    string QuotePath;
    string HSPath;

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
        string QuotePath = Application.persistentDataPath + "/Quotes.txt";
        string HSPath = Application.persistentDataPath + "/Highscore.txt";
    }

        private string quotes = "“I know not all that may be coming, but be it what it will, I'll go to it laughing.”? Herman Melville, Moby-Dick or, The Whale\r\n“Better to sleep with a sober cannibal than a drunk Christian.”? Ismael,  Moby-Dick or, The Whale\r\n“As for me, I am tormented with an everlasting itch for things remote. I love to sail forbidden seas, and land on barbarous coasts.”? Herman Melville, Moby-Dick or, The Whale\r\n\"All my means are sane, my motive and my object mad.\"- Herman Melville, Moby-Dick or, The Whale\r\n\"All mortal greatness is but disease.\"- Ishmael,  Moby-Dick or, The Whale\r\n\"I try all things, I achieve what I can.\"- Ishmael, Moby-Dick or, The Whale\r\n\"Talk not to me of blasphemy, man; I’d strike the sun if it insulted me.\"- Captain Ahab, Moby-Dick or, The Whale\r\n\"Heaven have mercy on us all - Presbyterians and Pagans alike - for we are all somehow dreadfully cracked about the head, and sadly need mending.\"- Ismael, Moby-Dick or, The Whale\r\n\"Call me Ishmael.\"- Ishmael, Moby-Dick or, The Whale\r\n\"See how elastic our prejudices grow when once love comes to bend them.\"- Ishmael, Moby-Dick or, The Whale\r\n\"To produce a mighty book, you must choose a mighty theme. No great and enduring volume can ever be written on the flea, though many there be who have tried it.\"- Ishmael, Moby-Dick or, The Whale\r\n\"Book! You lie there; the fact is, you books must know your places. You’ll do to give us the bare words and facts, but we come in to supply the thoughts.\"- Stubb, Moby-Dick or, The Whale";
        

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
        //string[] quotes = File.ReadAllLines(QuotePath);
        string[] newQuotes = quotes.Split("\n");
        return newQuotes;
    }
    private void DisplayRandomQuote()
    {
        string[] quotes = GetQuotes();
        int randIndex = UnityEngine.Random.Range(0, quotes.Length);
        MobyDickQuote.text = quotes[randIndex];
    }

    private float GetHighScore()
    {
        StreamReader reader = new StreamReader(HSPath);
        string highscore = reader.ReadToEnd();
        float score = 0f;
        try
        {
            score = float.Parse(highscore);
            return score;
        } catch {}
        reader.Close();
        return score;
    }

    private void SetHighscore(float Highscore)
    {
        StreamWriter writer = new StreamWriter(HSPath, true);
        writer.WriteLine(Highscore.ToString());
        writer.Close();
        StreamReader reader = new StreamReader(HSPath);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
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
        float highScore = 0;
        try
        {
            highScore = GetHighScore();
        }
        catch { }
        if (score.value > highScore)
        {
            try
            {
                highScore = score.value;
                SetHighscore(highScore);
                highScoreAnnounceText.enabled = true;
            }
            catch { }
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
