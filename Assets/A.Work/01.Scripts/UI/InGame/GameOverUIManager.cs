using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class GameOverUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI;

        private void Awake()
        {
            Time.timeScale = 1;
            gameOverUI.SetActive(false);
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
        }
        
    }
}