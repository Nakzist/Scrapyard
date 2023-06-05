using System;
using _GAME_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Variables

        #endregion

        #region Public Variables

        public PlayerHealthManager PlayerHealthManager { get; private set; }

        public PlayerInputHandler PlayerInputHandler { get; private set; }

        public PlayerMovementController PlayerMovementController { get; private set; }

        public PlayerWeaponController PlayerWeaponController { get; private set; }

        public PlayerHudController PlayerHudController { get; private set; }

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
            PlayerHealthManager = GetComponent<PlayerHealthManager>();
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            PlayerMovementController = GetComponent<PlayerMovementController>();
            PlayerWeaponController = GetComponent<PlayerWeaponController>();
            PlayerHudController = GetComponent<PlayerHudController>();

            GameManager.Instance.currentPlayer = this;
        }

        #endregion
    }
}