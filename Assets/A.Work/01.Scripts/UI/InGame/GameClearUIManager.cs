using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.UI.InGame
{
    public class GameClearUIManager : UIManager
    {
        [SerializeField] private string titleScene;

        public void GoMainMenu()
        {
            SceneManager.LoadScene(titleScene);
        }
        
    }
}