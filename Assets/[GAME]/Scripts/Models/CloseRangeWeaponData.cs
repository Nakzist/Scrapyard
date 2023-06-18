using System;
using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Models
{
    [Serializable]
    public class CloseRangeWeaponData
    {
        [Header("Weapon Data")]
        public GameObject weaponPrefab;
        public float weaponDamage = 5f;
        public float weaponRange = 20f;
        public float attackSpeed = 1f;
        public CloseRangeWeaponSkill weaponSkill;
        public Vector3 weaponPositionOffset;
        public Vector3 weaponRotationOffset;
        public Vector3 weaponScale;
        
        [Header("Animation Data")]
        public AnimationClip attackAnimationClip;
    }
}