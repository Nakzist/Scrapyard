using _GAME_.Scripts.Enums;
using _GAME_.Scripts.GlobalVariables;
using _GAME_.Scripts.Managers;
using _GAME_.Scripts.Observer;
using _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons;
using _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Ranged_Weapons;
using TMPro;
using UnityEngine;
using static _GAME_.Scripts.Currency.Currency;

namespace _GAME_.Scripts.Currency
{
    public class CurrencyHudController : ObserverBase
    {
        #region Serialized Variables

        [SerializeField]private BaseCloseCombatWeaponScriptableObject cleaverScriptable;
        [SerializeField]private BaseCloseCombatWeaponScriptableObject kanaboScriptable;
        [SerializeField]private BaseCloseCombatWeaponScriptableObject katanaScriptable;
        [SerializeField]private BaseCloseCombatWeaponScriptableObject spearScriptable;
        
        [SerializeField]private BaseRangedWeaponScriptableObject doubleBarrelScriptable;
        [SerializeField]private BaseRangedWeaponScriptableObject grenadeLauncherScriptable;
        [SerializeField]private BaseRangedWeaponScriptableObject minigunScriptable;
        [SerializeField]private BaseRangedWeaponScriptableObject revolverScriptable;

        #endregion
        
        #region Private Variables

        private bool _cleaverUnlocked;
        private bool _kanaboUnlocked;
        private bool _katanaUnlocked = true;
        private bool _spearUnlocked;

        private CloseRangeWeaponSkill _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;

        private bool _doubleBarrelUnlocked;
        private bool _grenadeLauncherUnlocked;
        private bool _minigunUnlocked;
        private bool _revolverUnlocked = true;
        
        private RangedWeaponsEnum _selectedRangeWeapon = RangedWeaponsEnum.Revolver;
        
        private TextMeshProUGUI _currencyText;
        private TextMeshProUGUI _closeRangeWeaponText;
        private TextMeshProUGUI _rangedWeaponText;



        #endregion

        #region Monobehaviour Methods

        private void Start()
        {
            _currencyText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            _closeRangeWeaponText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
            _rangedWeaponText = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
            
            UpdateUI();
        }

        #endregion

        #region Public Methods

        #region CloseRange

        public void SelectCleaver()
        {
            if(!_cleaverUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _cleaverUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Cleaver;
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Cleaver;
            }

            UpdateUI();
        }

        public void SelectKanabo()
        {
            if (!_kanaboUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _kanaboUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Kanabo;
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Kanabo;
            }
            UpdateUI();
        }

        public void SelectKatana()
        {
            if (!_katanaUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _katanaUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;
            }
            UpdateUI();
        }
        
        public void SelectSpear()
        {
            if (!_spearUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _spearUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Spear;
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Spear;
            }
            UpdateUI();
        }

        #endregion

        #region Ranged

        public void SelectDoubleBarrel()
        {
            if (!_doubleBarrelUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _doubleBarrelUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.DoubleBarrel;
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.DoubleBarrel;
            }
            UpdateUI();
        }

        public void SelectGrenadeLauncher()
        {
            if (!_grenadeLauncherUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _grenadeLauncherUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.GrenadeLauncher;
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.GrenadeLauncher;
            }
            UpdateUI();
        }
        
        public void SelectMinigun()
        {
            if (!_minigunUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _minigunUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.Minigun;
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.Minigun;
            }
            UpdateUI();
        }
        
        public void SelectRevolver()
        {
            if (!_revolverUnlocked)
            {
                if (SpendCurrency(1))
                {
                    _revolverUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.Revolver;
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.Revolver;
            }
            UpdateUI();
        }

        #endregion

        public void AcceptButton()
        {
            var selectedCloseWeapon = _selectedCloseRangeWeapon switch
            {
                CloseRangeWeaponSkill.Cleaver => cleaverScriptable,
                CloseRangeWeaponSkill.Kanabo => kanaboScriptable,
                CloseRangeWeaponSkill.Katana => katanaScriptable,
                CloseRangeWeaponSkill.Spear => spearScriptable,
                _ => null
            };
            
            var selectedRangedWeapon = _selectedRangeWeapon switch
            {
                RangedWeaponsEnum.Minigun => minigunScriptable,
                RangedWeaponsEnum.Revolver => revolverScriptable,
                RangedWeaponsEnum.DoubleBarrel => doubleBarrelScriptable,
                RangedWeaponsEnum.GrenadeLauncher => grenadeLauncherScriptable,
                _ => null
            };
            
            GameManager.Instance.currentPlayer.PlayerWeaponController.AddWeapon(selectedRangedWeapon, selectedCloseWeapon);
            
            Push(CustomEvents.WeaponSelected);
        }

        #endregion

        #region Private Methods

        private void UpdateUI()
        {
            _currencyText.text = GetCurrency().ToString();
            _closeRangeWeaponText.text = "Close Range: " + _selectedCloseRangeWeapon;
            _rangedWeaponText.text = "Ranged: " + _selectedRangeWeapon;
        }

        #endregion
    }
}