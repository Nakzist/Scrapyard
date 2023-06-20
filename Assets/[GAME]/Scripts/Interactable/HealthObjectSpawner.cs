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
            var health = Instantiate(healthObjectPrefab, transform.position, Quaternion.identity);
            health.GetComponent<HealthInteractable>().spawner = this;
        }
    }
}