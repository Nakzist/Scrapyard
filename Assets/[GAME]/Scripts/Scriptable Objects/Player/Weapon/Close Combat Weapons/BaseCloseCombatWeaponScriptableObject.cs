using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons
{
    public abstract class BaseCloseCombatWeaponScriptableObject : ScriptableObject
    {
        #region Serialied Fields

        [SerializeField] private AnimationClip shootAnimationClip;

        #endregion

        #region Public Fields

        public AnimationClip ShootAnimationClip => shootAnimationClip;

        public abstract void WeaponSkill();

        #endregion
    }
}