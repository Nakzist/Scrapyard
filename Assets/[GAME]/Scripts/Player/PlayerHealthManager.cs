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
        public bool canReceiveDamage = true;

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

        public void TakeDamage(float incomingDamage, DamageType damageType, DamageCauser damageCauser)
        {
            if (canReceiveDamage)
            {
                _health -= incomingDamage;
                Push(CustomEvents.OnHealthChanged);
                if (_health <= 0) CharacterDeath();
            }
        }

        public void HealPlayer(float amount)
        {
            _health += amount;
            
            if(_health > maxHealth) _health = maxHealth;
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
