using System;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class AmmoInteractable : MonoBehaviour
    {
        public AmmoObjectSpawner spawner;

        private void Update()
        {
            transform.Rotate(Vector3.up, 10 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var weaponController = other.GetComponent<PlayerWeaponController>();
                weaponController.IncreaseBullet(20f);
                spawner.ObjectPickedUp();
                Destroy(gameObject);
            }
        }
    }
}