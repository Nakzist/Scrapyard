using System.Collections.Generic;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME_.Scripts.Player
{
    public class PlayerHudController : ObserverBase
    {
        #region Private Variables

        private GameObject _hudInstance;
        private TextMeshProUGUI _winConditionText;
        private TextMeshProUGUI _hpText;
        private GameObject _losePanel;
        private List<Sprite> _waveSprites;
        private TextMeshProUGUI _ammoText;

        private PlayerHudDataScriptableObject _playerHudDataScriptableObject;

        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            //GetReferences();
        }

        #region Observer 

        private void OnEnable()
        {
            Register(CustomEvents.OnPlayerDeath, ShowLosePanel);
            Register(CustomEvents.OnHealthChanged, ShowHp);
            Register(CustomEvents.OnWeaponChanged, WeaponChange);
            Register(CustomEvents.OnBulletChange, UpdateWeaponAmmo);
            Register(CustomEvents.WeaponSelected, GetReferences);
        }

        private void OnDisable()
        {
            Unregister(CustomEvents.OnPlayerDeath, ShowLosePanel);
            Unregister(CustomEvents.OnHealthChanged, ShowHp);
            Unregister(CustomEvents.OnWeaponChanged, WeaponChange);
            Unregister(CustomEvents.OnBulletChange, UpdateWeaponAmmo);
            Unregister(CustomEvents.WeaponSelected, GetReferences);
        }

        #endregion

        #endregion
        
        #region Public Methods
        
        public void UpdateWinConditionText(string text)
        {
            _winConditionText.text = text;
        }
        
        #endregion

        #region Private Methods

        private void GetReferences()
        {
            _playerHudDataScriptableObject = Resources.Load<PlayerHudDataScriptableObject>(FolderPaths.HUD_DATA_PATH);
            _waveSprites = _playerHudDataScriptableObject.WaveSprites;
            _hudInstance = Instantiate(_playerHudDataScriptableObject.HudPrefab);
            
            _winConditionText = _hudInstance.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            _hpText = _hudInstance.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            
            _losePanel = _hudInstance.transform.GetChild(0).GetChild(4).gameObject;
            
            _ammoText = _hudInstance.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>();

            var image = _hudInstance.transform.GetChild(0).GetChild(5).GetComponent<Image>();
            if(_waveSprites.Count > GameManager.Instance.currentLevel)
                image.sprite = _waveSprites[GameManager.Instance.currentLevel - 1];
            image.gameObject.SetActive(true);
            
            WeaponChange();
            UpdateWeaponAmmo();
        }

        private void ShowLosePanel()
        {
            _losePanel.SetActive(true);
        }

        private void ShowHp()
        {
            if(GameManager.Instance == null)
                return;
            
            if (GameManager.Instance.currentPlayer == null)
            {
                if(_hpText == null)
                    return;
                
                _hpText.text = "100";
            }
            else
            {
                var currentHealth = GameManager.Instance.currentPlayer.PlayerHealthManager.Health;
                _hpText.text = currentHealth.ToString("F0");
            }
        }

        private void WeaponChange()
        {
            if (_ammoText == null) return;
            _ammoText.text = GameManager.Instance.currentPlayer.PlayerWeaponController.GetCurrentAmmoText();
        }
        
        private void UpdateWeaponAmmo()
        {
            if (_ammoText == null) return;
            _ammoText.text = GameManager.Instance.currentPlayer.PlayerWeaponController.GetCurrentAmmoText();
        }

        #endregion
        
    }
}