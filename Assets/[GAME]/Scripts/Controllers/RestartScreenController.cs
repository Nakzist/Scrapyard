using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME_.Scripts.Controllers
{
    public class RestartScreenController: MonoBehaviour
    {
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
        
    }
}