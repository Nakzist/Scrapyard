using System.Collections;
using _GAME_.Scripts.Enemy;
using _GAME_.Scripts.Managers;
using UnityEngine;

namespace _GAME_.Scripts.Scriptable_Objects.Player.Weapon.Close_Combat_Weapons
{
    public static class CloseRangeWeaponSkills
    {
        #region Katana

        private static bool _katanaSkillOnCooldown;
        private static bool _skillUsed;

        public static void KatanaSkillHandler()
        {
            if (_katanaSkillOnCooldown || _skillUsed) return;

            GameManager.Instance.currentPlayer.StartCoroutine(KatanaSkill(3));
        }

        private static IEnumerator KatanaSkill(float skillUseTime)
        {
            GameManager.Instance.currentPlayer.PlayerHealthManager.canReceiveDamage = false;
            _skillUsed = true;
            yield return new WaitForSeconds(skillUseTime);
            GameManager.Instance.currentPlayer.PlayerHealthManager.canReceiveDamage = true;
            _skillUsed = false;
            GameManager.Instance.currentPlayer.StartCoroutine(KatanaSkillCooldown(12));
        }

        private static IEnumerator KatanaSkillCooldown(float cooldown)
        {
            _katanaSkillOnCooldown = true;
            yield return new WaitForSeconds(cooldown);
            _katanaSkillOnCooldown = false;
        }

        #endregion

        #region Spear

        private static float _spearUseTime;
        
        public static void SpearSkillHandler()
        {
            if(GameManager.Instance.currentPlayer.PlayerWeaponController.GetLastMeleeAttackTime() < 3)
                if(!GameManager.Instance.currentPlayer.PlayerWeaponController.IsMeleeAttackBoosted())
                    GameManager.Instance.currentPlayer.PlayerWeaponController.GiveMeleeDamageBoost();
        }

        #endregion

        #region Kanabo

        public static void KanaboSkillHandler()
        {
            if(GameManager.Instance.currentPlayer.PlayerWeaponController.GetCloseRangeKillCount() > 4)
                KanaboSkill();
        }
        
        private static void KanaboSkill()
        {
            GameManager.Instance.currentPlayer.PlayerWeaponController.ResetCloseRangeKillCount();

            // ReSharper disable once Unity.PreferNonAllocApi
            var hitColliders =Physics.OverlapSphere(GameManager.Instance.currentPlayer.transform.position, 10f);

            foreach (var col in hitColliders)
            {
                if (col.TryGetComponent<BaseEnemy>(out var enemy))
                    enemy.StartCoroutine(enemy.StunEnemy(2f));
            }
        }

        #endregion

        #region Cleaver

        public static void CleaverSkillHandler()
        {
            if(GameManager.Instance.currentPlayer.PlayerWeaponController.GetCloseRangeKillCount() > 3)
                CleaverSkill();
        }

        private static void CleaverSkill()
        {
            GameManager.Instance.currentPlayer.PlayerWeaponController.ResetCloseRangeKillCount();
            
            GameManager.Instance.currentPlayer.PlayerHealthManager.HealPlayer(12f);
        }

        #endregion
    }
}