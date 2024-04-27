using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject gameOverUI;

    [SerializeField] FloatVariable score;

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
        print(score.value);
    }
}
