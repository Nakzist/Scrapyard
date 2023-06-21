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

        private CloseRangeWeaponSkill _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;

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
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            
        }

        #endregion

        #region Public Methods

        #region CloseRange

        public void SelectCleaver(GameObject obj)
        {
            if(!CleaverUnlocked)
            {
                if (SpendCurrency(1))
                {
                    CleaverUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Cleaver;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Cleaver;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }

            UpdateUI();
        }

        public void SelectKanabo(GameObject obj)
        {
            if (!KanaboUnlocked)
            {
                if (SpendCurrency(1))
                {
                    KanaboUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Kanabo;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Kanabo;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }

        public void SelectKatana(GameObject obj)
        {
            if (!KatanaUnlocked)
            {
                if (SpendCurrency(1))
                {
                    KatanaUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Katana;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }
        
        public void SelectSpear(GameObject obj)
        {
            if (!SpearUnlocked)
            {
                if (SpendCurrency(1))
                {
                    SpearUnlocked = true;
                    _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Spear;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedCloseRangeWeapon = CloseRangeWeaponSkill.Spear;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }

        #endregion

        #region Ranged

        public void SelectDoubleBarrel(GameObject obj)
        {
            if (!DoubleBarrelUnlocked)
            {
                if (SpendCurrency(1))
                {
                    DoubleBarrelUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.DoubleBarrel;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.DoubleBarrel;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }

        public void SelectGrenadeLauncher(GameObject obj)
        {
            if (!GrenadeLauncherUnlocked)
            {
                if (SpendCurrency(1))
                {
                    GrenadeLauncherUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.GrenadeLauncher;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.GrenadeLauncher;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }
        
        public void SelectMinigun(GameObject obj)
        {
            if (!MinigunUnlocked)
            {
                if (SpendCurrency(1))
                {
                    MinigunUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.Minigun;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.Minigun;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }
        
        public void SelectRevolver(GameObject obj)
        {
            if (!RevolverUnlocked)
            {
                if (SpendCurrency(1))
                {
                    RevolverUnlocked = true;
                    _selectedRangeWeapon = RangedWeaponsEnum.Revolver;
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                _selectedRangeWeapon = RangedWeaponsEnum.Revolver;
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            UpdateUI();
        }

        #endregion

        public void AcceptButton(GameObject obj)
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
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Destroy(gameObject);
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