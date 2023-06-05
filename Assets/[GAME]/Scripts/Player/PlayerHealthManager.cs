using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Interfaces;
using _GAME_.Scripts.Observer;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    public class PlayerHealthManager : ObserverBase, IDamageable
    {
        #region Public Variables

        public float Health => _health;

        #endregion
        
        #region Serialized Variables

        [SerializeField] private float maxHealth;

        #endregion

        #region Private Variables

        private float _health;

        #endregion
        
        #region Monobehavious Methods

        private void Start()
        {
            _health = maxHealth;
            Push(CustomEvents.OnHealthChanged);
        }

        #endregion

        #region Public Methods

        public void TakeDamage(float incomingDamage, DamageType damageType)
        {
            _health -= incomingDamage;
            Push(CustomEvents.OnHealthChanged);
            if (_health <= 0) CharacterDeath();
        }
        
        #endregion

        #region Private Methods

        private void CharacterDeath()
        {
            Time.timeScale = 0;
            Push(CustomEvents.OnPlayerDeath);
        }

        #endregion
    }
}
