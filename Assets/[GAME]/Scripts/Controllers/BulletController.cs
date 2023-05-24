using UnityEngine;

namespace _GAME_.Scripts.Controllers
{
    public class BulletController : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, .5f);
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
