using System.Collections.Generic;
using UnityEngine;

namespace _GAME_.Scripts.Player
{
    public class InAttackRange : MonoBehaviour
    {
        public static List<GameObject> EnemiesInAttackRange;

        private void Start()
        {
            EnemiesInAttackRange = new List<GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log(other.gameObject.name + " is entered attack range");
                EnemiesInAttackRange.Add(other.gameObject);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log(other.gameObject.name + " is exited attack range");
                EnemiesInAttackRange.Remove(other.gameObject);
            }
        }
    }
}
