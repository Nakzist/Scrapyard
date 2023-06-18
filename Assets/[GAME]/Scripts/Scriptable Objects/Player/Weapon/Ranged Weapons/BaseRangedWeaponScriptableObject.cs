using _GAME_.Scripts.Models;
using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Ranged_Weapons
{
    [CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Scrapyard/Data/Player/Weapon/Ranged Weapon")]
    public class BaseRangedWeaponScriptableObject : ScriptableObject
    {
        #region Serialied Fields

        [SerializeField] private RangedWeapon weapon;
        
        #endregion

        #region Public Fields
        
        public RangedWeapon Weapon => weapon;
        

        #endregion
        
    }
}