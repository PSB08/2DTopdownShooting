using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.UI.InGame
{
    public class GameOverUIManager : UIManager
    {
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        }
        
    }
}