using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<GameObject> _enemiesToSpawn;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _timeToSurvive;
    [SerializeField] private float winScore;
    [SerializeField] private WinConditions _winCondition;
    [SerializeField] private TextMeshProUGUI winConditionText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject losePanel;
    private float lastSpawnTime;
    public static List<GameObject> aliveEnemies;
    public static Action OnEnemyDied;
    public static Action OnPlayerDied;
    public static Action OnQueenDied;
    public static Action<float> OnHealthChanged;
    public static float _score;


    private void OnEnable()
    {
        OnEnemyDied += IncreaseScore;
        OnPlayerDied += ShowLosePanel;
        OnHealthChanged += ShowHP;
        OnQueenDied += Win;
    }
    private void OnDisable()
    {
        OnEnemyDied -= IncreaseScore;
        OnPlayerDied -= ShowLosePanel;
        OnHealthChanged -= ShowHP;
        OnQueenDied -= Win;
    }

    private void Awake()
    {
        aliveEnemies = new List<GameObject>();
        _score = 0;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (_enemiesToSpawn.Count >0)
        {
            SpawnEnemy();
        }
        if (_winCondition == WinConditions.Time)
        {
            _timeToSurvive -= Time.deltaTime;
            winConditionText.text = Mathf.Ceil(_timeToSurvive).ToString() + " seconds left to win";
            if (_timeToSurvive <= 0)
            {
                Win();
            }
        }
    }

    void SpawnEnemy()
    {
        if (lastSpawnTime + 1/_spawnRate <= Time.time)
        {
            lastSpawnTime = Time.time;
            int spawnPointIndex = Random.Range(0, _spawnPoints.Count);
            int EnemyIndex = Random.Range(0, _enemiesToSpawn.Count);
            GameObject enemy = Instantiate(_enemiesToSpawn[EnemyIndex], _spawnPoints[spawnPointIndex].position, Quaternion.identity);
                if (enemy != null) aliveEnemies.Add(enemy);
        }
    }

    void IncreaseScore()
    {
        _score++;
        if (_winCondition == WinConditions.Score)
        {
            winConditionText.text = $"{_score} / {winScore}";
        }
        if (_score >= winScore)
        {
            Win();
        }
    }

    void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void ShowHP(float health)
    {
        hpText.text = health.ToString();
    }
}

enum WinConditions
{
    Score,
    Time,
    KillingQueen
}
