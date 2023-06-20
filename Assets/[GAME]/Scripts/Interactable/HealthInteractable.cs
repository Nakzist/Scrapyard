using System;
using _GAME_.Scripts.Player;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class HealthInteractable : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var healthController = other.GetComponent<PlayerHealthManager>();
                healthController.HealPlayer(20f);
            }
        }
    }
}