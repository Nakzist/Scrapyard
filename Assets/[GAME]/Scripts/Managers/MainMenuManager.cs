using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GAME_.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Monobehaviour Methods

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
        public void RestartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        #endregion
    }
}
