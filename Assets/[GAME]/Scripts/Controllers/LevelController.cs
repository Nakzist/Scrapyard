using System.Collections.Generic;
using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Level;
using UnityEngine;
using UnityEngine.Serialization;
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

        private WinConditions _currentWinCondition;
        private float _currentWinConditionTargetValue;
        private int _currentWinConditionIndex;

        private bool _levelFinished;
        private bool _queenSpawned;

        #endregion

        #region Serialized Variables

        [FormerlySerializedAs("_craftHud")] [SerializeField] private GameObject craftHud;
        [FormerlySerializedAs("_levelIndex")] [SerializeField] private int levelIndex;

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {            
            GetLevelData();
            
            if(GetCurrency() == 0)
                AddCurrency(2);
            
            //create hud for crafting
            Instantiate(craftHud);
        }

        private void Update()
        {
            if (!_levelFinished) return;

            if (GameManager.Instance.currentLevel != 5)
            {
                if (_currentLevel.enemiesToSpawn.Count > 0)
                    SpawnEnemy();
            
                if (_currentWinCondition == WinConditions.Time)
                {
                    _currentWinConditionTargetValue -= Time.deltaTime;
                    GameManager.Instance.currentPlayer.PlayerHudController.UpdateWinConditionText(Mathf.Ceil(_currentWinConditionTargetValue) + " seconds left to win");
                    //winConditionText.text = Mathf.Ceil(timeToSurvive) + " seconds left to win";
                    if (_currentWinConditionTargetValue <= 0)
                    {
                        IncreaseCurrentWinCondition();
                    }
                }
            }
            else
            {
                if (!_queenSpawned)
                {
                    var spawnPointIndex = Random.Range(0, _spawnPoints.Count);
                    var enemyIndex = Random.Range(0, _currentLevel.enemiesToSpawn.Count);
                    var enemy = Instantiate(_currentLevel.enemiesToSpawn[0], _spawnPoints[spawnPointIndex].position,
                        Quaternion.identity);
                    if(enemy != null) _aliveEnemies.Add(enemy);
                    _queenSpawned = true;
                }
            }
        }

        private void OnEnable()
        {
            Register(CustomEvents.IncreaseSCore, IncreaseScore);
            Register(CustomEvents.WeaponSelected, AfterWeaponSelect);
        }

        private void OnDisable()
        {
            Unregister(CustomEvents.IncreaseSCore, IncreaseScore);
            Unregister(CustomEvents.WeaponSelected, AfterWeaponSelect);
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
        
        private void IncreaseCurrentWinCondition()
        {
            _currentWinConditionIndex++;
            GameManager.Instance.score = 0;

            switch (_currentWinConditionIndex)
            {
                case 1:
                    _currentWinCondition = _currentLevel.winCondition1;
                    _currentWinConditionTargetValue = _currentLevel.winCondition1Value;
                    break;
                case 2:
                    _currentWinCondition = _currentLevel.winCondition2;
                    _currentWinConditionTargetValue = _currentLevel.winCondition2Value;
                    break;
                case 3:
                    _currentWinCondition = _currentLevel.winCondition3;
                    _currentWinConditionTargetValue = _currentLevel.winCondition3Value;
                    break;
                case 4:
                    AddCurrency(2); //Add 2 currency for winning
                    GameManager.Instance.Win();
                    break;
            }
        }

        private void AfterWeaponSelect()
        {
            IncreaseCurrentWinCondition();

            var parent = transform.GetChild(2);
            _spawnPoints = new List<Transform>();
            _aliveEnemies = new List<GameObject>();
            
            foreach (Transform child in parent)
            {
                _spawnPoints.Add(child);
            }

            _levelFinished = true;

            if (GameManager.Instance.currentLevel == 5)
                return;
            var audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }

        private void IncreaseScore()
        {
            if (_currentWinCondition == WinConditions.Score)
            {
                GameManager.Instance.currentPlayer.PlayerHudController.UpdateWinConditionText($"{GameManager.Instance.score} / {_currentWinConditionTargetValue}");
            }
            if (GameManager.Instance.score >= _currentWinConditionTargetValue)
            {
                IncreaseCurrentWinCondition();
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

            GameManager.Instance.currentLevel = levelIndex;
            GameManager.Instance.score = 0;
            
            _currentLevel = levelDataScriptableObject.Levels[(GameManager.Instance.currentLevel - 1)];
        }

        #endregion
    }
}