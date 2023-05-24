using System;
using System.Collections.Generic;
using _GAME_.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Public Variables

        public static List<GameObject> AliveEnemies;
        public static Action OnEnemyDied;
        public static Action OnPlayerDied;
        public static Action OnQueenDied;
        public static Action<float> OnHealthChanged;
        public static float Score;

        #endregion
        
        #region Serialized Variables

        [FormerlySerializedAs("_spawnPoints")] [SerializeField] private List<Transform> spawnPoints;
        [FormerlySerializedAs("_enemiesToSpawn")] [SerializeField] private List<GameObject> enemiesToSpawn;
        [FormerlySerializedAs("_spawnRate")] [SerializeField] private float spawnRate;
        [FormerlySerializedAs("_timeToSurvive")] [SerializeField] private float timeToSurvive;
        [SerializeField] private float winScore;
        [FormerlySerializedAs("_winCondition")] [SerializeField] private WinConditions winCondition;
        [SerializeField] private TextMeshProUGUI winConditionText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private GameObject losePanel;

        #endregion

        #region Private Variables

        private float _lastSpawnTime;

        #endregion
        
        #region Monobehavious Methods

        private void Awake()
        {
            AliveEnemies = new List<GameObject>();
            Score = 0;
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (enemiesToSpawn.Count >0)
            {
                SpawnEnemy();
            }
            if (winCondition == WinConditions.Time)
            {
                timeToSurvive -= Time.deltaTime;
                winConditionText.text = Mathf.Ceil(timeToSurvive) + " seconds left to win";
                if (timeToSurvive <= 0)
                {
                    Win();
                }
            }
        }
        
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

        #endregion

        #region Public Methods

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods

        private void SpawnEnemy()
        {
            if (_lastSpawnTime + 1/spawnRate <= Time.time)
            {
                _lastSpawnTime = Time.time;
                var spawnPointIndex = Random.Range(0, spawnPoints.Count);
                var enemyIndex = Random.Range(0, enemiesToSpawn.Count);
                var enemy = Instantiate(enemiesToSpawn[enemyIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
                if (enemy != null) AliveEnemies.Add(enemy);
            }
        }

        private void IncreaseScore()
        {
            Score++;
            if (winCondition == WinConditions.Score)
            {
                winConditionText.text = $"{Score} / {winScore}";
            }
            if (Score >= winScore)
            {
                Win();
            }
        }

        private void Win()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void ShowLosePanel()
        {
            losePanel.SetActive(true);
        }
        
        private void ShowHP(float health)
        {
            hpText.text = health.ToString();
        }

        #endregion
    }
}