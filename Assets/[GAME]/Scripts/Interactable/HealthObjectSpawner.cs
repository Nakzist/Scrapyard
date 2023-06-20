using System.Collections;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class HealthObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject healthObjectPrefab;

        private void Start()
        {
            ObjectPickedUp();
        }

        public void ObjectPickedUp()
        {
            StartCoroutine(SpawnWithDelay());
        }
        
        private IEnumerator SpawnWithDelay()
        {
            yield return new WaitForSeconds(5f);
            var health = Instantiate(healthObjectPrefab, transform.position, Quaternion.identity);
            health.GetComponent<HealthInteractable>().spawner = this;
        }
    }
}