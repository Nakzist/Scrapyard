using System.Collections.Generic;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Level;
using UnityEngine;
using static _GAME_.Scripts.Currency.Currency;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Controllers
{
    public class LevelController : ObserverBase
    {
        #region Private Variables

        private float _lastSpawnTime;
        // ReSharper disable once CollectionNeverQueried.Local
        private List<GameObject> _aliveEnemies;
        private List<Transform> _spawnPoints;
        private Level _currentLevel;

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {            
            GetLevelData();
            
            if(GetCurrency() == 0)
                AddCurrency(2);
            
            //create hud for crafting

            var parent = transform.GetChild(2);
            _spawnPoints = new List<Transform>();
            _aliveEnemies = new List<GameObject>();
            
            foreach (Transform child in parent)
            {
                _spawnPoints.Add(child);
            }
        }

        private void Update()
        {
            if (_currentLevel.enemiesToSpawn.Count >0)
            {
                SpawnEnemy();
            }
            if (_currentLevel.winCondition == WinConditions.Time)
            {
                _currentLevel.timeToSurvive -= Time.deltaTime;
                GameManager.Instance.currentPlayer.PlayerHudController.UpdateWinConditionText(Mathf.Ceil(_currentLevel.timeToSurvive) + " seconds left to win");
                //winConditionText.text = Mathf.Ceil(timeToSurvive) + " seconds left to win";
                if (_currentLevel.timeToSurvive <= 0)
                {
                    GameManager.Instance.Win();
                }
            }
        }

        private void OnEnable()
        {
            Register(CustomEvents.IncreaseSCore, IncreaseScore);
        }

        private void OnDisable()
        {
            Unregister(CustomEvents.IncreaseSCore, IncreaseScore);
        }

        #endregion

        #region Private Methods

        private void SpawnEnemy()
        {
            if (_lastSpawnTime + 1/_currentLevel.spawnRate <= Time.time)
            {
                _lastSpawnTime = Time.time;
                var spawnPointIndex = Random.Range(0, _spawnPoints.Count);
                var enemyIndex = Random.Range(0, _currentLevel.enemiesToSpawn.Count);
                var enemy = Instantiate(_currentLevel.enemiesToSpawn[enemyIndex], _spawnPoints[spawnPointIndex].position, Quaternion.identity);
                if (enemy != null) _aliveEnemies.Add(enemy);
            }
        }

        private void IncreaseScore()
        {
            if (_currentLevel.winCondition == WinConditions.Score)
            {
                GameManager.Instance.currentPlayer.PlayerHudController.UpdateWinConditionText($"{GameManager.Instance.score} / {_currentLevel.winScore}");
            }
            if (GameManager.Instance.score >= _currentLevel.winScore)
            {
                AddCurrency(2); //Add 2 currency for winning
                GameManager.Instance.Win();
            }
        }
        
        private void GetLevelData()
        {
            var levelDataScriptableObject = Resources.Load<LevelDataScriptableObject>(FolderPaths.LEVEL_DATA_PATH);
            if (GameManager.Instance == null)
            {
                var manager = new GameObject("GameManager");
                manager.AddComponent<GameManager>();
            }

            GameManager.Instance.currentLevel++;
            GameManager.Instance.score = 0;
            
            _currentLevel = levelDataScriptableObject.Levels[(GameManager.Instance.currentLevel - 1)];
        }

        #endregion
    }
}