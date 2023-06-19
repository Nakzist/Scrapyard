using _GAME_.Scripts.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons
{
    [CreateAssetMenu(fileName = "New Close Combat Weapon", menuName = "Scrapyard/Data/Player/Weapon/Close Combat Weapon")]
    public class BaseCloseCombatWeaponScriptableObject : ScriptableObject
    {
        #region Serialied Fields

        [FormerlySerializedAs("weaponData")] [SerializeField] private CloseRangeWeaponData weapon;

        #endregion

        #region Public Fields
        
        public CloseRangeWeaponData Weapon => weapon;

        #endregion
    }
}