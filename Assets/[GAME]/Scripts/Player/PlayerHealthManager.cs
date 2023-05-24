using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Player
{
    public class PlayerHealthManager : MonoBehaviour, IDamageable
    {
        #region Public Variables

        public static PlayerHealthManager Instance;

        #endregion

        #region Serialized Variables

        [FormerlySerializedAs("_maxHealth")] [SerializeField]
        private float maxHealth;

        #endregion

        #region Private Variables

        private float _health;

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
        }

        private void Start()
        {
            _health = maxHealth;
            GameManager.OnHealthChanged?.Invoke(_health);
        }

        #endregion

        #region Public Methods

        public void TakeDamage(float incomingDamage)
        {
            _health -= incomingDamage;
            GameManager.OnHealthChanged?.Invoke(_health);
            if (_health <= 0) CharacterDeath();
        }
        
        #endregion

        #region Private Methods

        private void CharacterDeath()
        {
            Time.timeScale = 0;
            GameManager.OnPlayerDied?.Invoke();
        }

        #endregion
    }
}
