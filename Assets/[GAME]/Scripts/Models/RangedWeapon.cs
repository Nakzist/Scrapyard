using System;
using _GAME_.Scripts.Enums;
using UnityEngine;

namespace _GAME_.Scripts.Models
{
    [Serializable]
    public class RangedWeapon
    {
        [Header("Weapon Data")]
        public GameObject weaponPrefab;
        public RangedWeaponEffects weaponEffect;
        public float delayBetweenShoots = 0.25f;
        public float weaponDamage = 5f;
        public float weaponRange = 80f;
        public float bulletSpeed = 25f;
        public GameObject bulletPrefab;
        public float reloadTime = 1f;
        public float magSize = 10f;
        public float bulletCount = 100f;
        public LayerMask hittableLayerMask;
        public Vector3 weaponPositionOffset;
        public Vector3 weaponRotationOffset;
        public Vector3 weaponScale;
        
        [Header("Animation Data")]
        public AnimationClip reloadAnimationClip;
        public AnimationClip shootAnimationClip;
    }
}