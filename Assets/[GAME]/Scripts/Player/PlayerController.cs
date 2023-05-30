using System;
using _GAME_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables

        private PlayerHealthManager _playerHealthManager;
        private PlayerInputHandler _playerInputHandler;
        private PlayerMovementController _playerMovementController;
        private PlayerWeaponController _playerWeaponController;
        private PlayerHudController _playerHudController;

        #endregion

        #region Public Variables

        public PlayerHealthManager PlayerHealthManager => _playerHealthManager;
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
        public PlayerMovementController PlayerMovementController => _playerMovementController;
        public PlayerWeaponController PlayerWeaponController => _playerWeaponController;
        public PlayerHudController PlayerHudController => _playerHudController;

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            GetReferences();
        }

        #endregion

        #region Private Methods

        private void GetReferences()
        {
            _playerHealthManager = GetComponent<PlayerHealthManager>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerWeaponController = GetComponent<PlayerWeaponController>();
            _playerHudController = GetComponent<PlayerHudController>();

            GameManager.Instance.currentPlayer = this;
        }

        #endregion
    }
}