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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnScoreUp(float points)
    {
        score.value += points;
        scoreText.text = "Score: " + score.value.ToString();
    }
}
