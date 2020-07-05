using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Configuration")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBeetwenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;
    GameSession gameSession;

    private void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }
    public GameObject GetEnemyPrefab()
    { return enemyPrefab; }
    public List<Transform> GetWaypoints()
    {
        var waveWaypoints = new List<Transform>();
        foreach (Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; 
    }
    public float GetTimeBeetwenSpawns()
    { return timeBeetwenSpawns; }
    public float GetSpawnRandomFactor()
    { return spawnRandomFactor; }
    public int GetNumberOfEnemies()
    { return numberOfEnemies; }
    public float GetMoveSpeed()
    { if (gameSession) { return moveSpeed * gameSession.GetDifficultModiffier(); }
        else
        { return moveSpeed; }
    }
}
