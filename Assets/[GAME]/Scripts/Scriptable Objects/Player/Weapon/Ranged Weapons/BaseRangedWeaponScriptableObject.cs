using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Ranged_Weapons
{
    public class BaseRangedWeaponScriptableObject : ScriptableObject
    {
        #region Serialied Fields

        [SerializeField] private AnimationClip shootAnimationClip;

        #endregion

        #region Public Fields

        public AnimationClip ShootAnimationClip => shootAnimationClip;

        #endregion
    }
}