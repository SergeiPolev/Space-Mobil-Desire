using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int addHealthAtScorePoint = 1000;
    float difficultModiffier = 1f;
    int score = 0;
    int healthScoreConnection = 0;
    int health = 100;
    Player player;
    // Start is called before the first frame update

    private void Awake()
    {
        SetUpSingleton();
    }
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        health = player.GetHealth();
    }
    private void SetUpSingleton()
    {
        int numbersOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numbersOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHealth()
    {
        return health;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
        healthScoreConnection += scoreValue;
        if (healthScoreConnection >= addHealthAtScorePoint)
        {
            difficultModiffier += 0.4f;
            Debug.Log(difficultModiffier);
            player.AddHealth();
            healthScoreConnection -= addHealthAtScorePoint;
        }
    }

    public float GetDifficultModiffier()
    {
        return difficultModiffier;
    }
    
    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
