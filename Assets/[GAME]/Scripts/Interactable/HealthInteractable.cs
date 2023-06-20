using System;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class HealthInteractable : MonoBehaviour
    {
        public HealthObjectSpawner spawner;
        
        private void Update()
        {
            transform.Rotate(Vector3.up, 10 * Time.deltaTime);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var healthController = other.GetComponent<PlayerHealthManager>();
                healthController.HealPlayer(20f);
                spawner.ObjectPickedUp();
                Destroy(gameObject);
            }
        }
    }
}