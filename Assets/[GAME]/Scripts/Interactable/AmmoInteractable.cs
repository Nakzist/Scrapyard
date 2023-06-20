using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class AmmoInteractable : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var weaponController = other.GetComponent<PlayerWeaponController>();
                weaponController.IncreaseBullet(20f);
            }
        }
    }
}