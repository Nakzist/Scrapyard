using System.Collections.Generic;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME_.Scripts.Managers
{
    public class GameManager : ObserverBase
    {
        #region Public Static Variables

        public static GameManager Instance{get; private set;}

        [HideInInspector] public float score;

        #endregion

        #region Public Variables
        
        [HideInInspector] public List<GameObject> aliveEnemies;
        [HideInInspector] public PlayerController currentPlayer;
        [HideInInspector] public int currentLevel;

        #endregion

        #region Private Variables

        private float _lastSpawnTime;
        private int _currentLevel;

        #endregion
        
        #region Monobehavious Methods

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            
            aliveEnemies = new List<GameObject>();
            score = 0;
            Time.timeScale = 1;
        }

        private void OnEnable()
        {
            Register(CustomEvents.OnEnemyDeath, IncreaseScore);
            Register(CustomEvents.OnQueenDeath, Win);
        }
        
        private void OnDisable()
        {
            Unregister(CustomEvents.OnEnemyDeath, IncreaseScore);
            Unregister(CustomEvents.OnQueenDeath, Win);
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

        private void IncreaseScore()
        {
            score++;
            Push(CustomEvents.IncreaseSCore);
        }

        public void Win()
        {
            _currentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        #endregion
    }
}