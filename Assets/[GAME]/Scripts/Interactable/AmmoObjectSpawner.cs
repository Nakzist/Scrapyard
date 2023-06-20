using System.Collections;
using UnityEngine;

namespace _GAME_.Scripts.Interactable
{
    public class AmmoObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject ammoObjectPrefab;

        private void Start()
        {
            ObjectPickedUp();
        }

        public void ObjectPickedUp()
        {
            var ammo = Instantiate(ammoObjectPrefab, transform.position, Quaternion.identity);
            ammo.GetComponent<AmmoInteractable>().spawner = this;
        }

        private IEnumerator SpawnWithDelay()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}