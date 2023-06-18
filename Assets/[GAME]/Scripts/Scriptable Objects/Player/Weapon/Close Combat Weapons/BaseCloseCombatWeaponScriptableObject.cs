using _GAME_.Scripts.Models;
using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons
{
    [CreateAssetMenu(fileName = "New Close Combat Weapon", menuName = "Scrapyard/Data/Player/Weapon/Close Combat Weapon")]
    public class BaseCloseCombatWeaponScriptableObject : ScriptableObject
    {
        #region Serialied Fields

        [SerializeField] private CloseRangeWeaponData weaponData;

        #endregion

        #region Public Fields
        
        public CloseRangeWeaponData WeaponData => weaponData;

        #endregion
    }
}