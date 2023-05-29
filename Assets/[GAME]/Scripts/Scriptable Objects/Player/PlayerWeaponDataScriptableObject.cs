using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player
{
    [CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "Scrapyard/Data/Player Weapon Data")]
    public class PlayerWeaponDataScriptableObject : ScriptableObject
    {
        #region Serialized Variables
        
        [Header("Weapon Data")]
        [SerializeField] private float delayBetweenShoots = 0.25f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float meleeDamage = 25f;
        [SerializeField] private float weaponRange = 80f;
        [SerializeField] private float bulletSpeed = 25f;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float maxAmmo = 10f;
        [SerializeField] private LayerMask hittableLayerMask;

        #endregion

        #region Public Variables
        
        public float DelayBetweenShoots => delayBetweenShoots;
        public float WeaponDamage => weaponDamage;
        public float MeleeDamage => meleeDamage;
        public float WeaponRange => weaponRange;
        public float BulletSpeed => bulletSpeed;
        public GameObject BulletPrefab => bulletPrefab;
        public float MaxAmmo => maxAmmo;
        public LayerMask HittableLayerMask => hittableLayerMask;

        #endregion
    }
}