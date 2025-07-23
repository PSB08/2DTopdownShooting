using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.UI.InGame
{
    public class GameClearUIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject uiPanel;
        [SerializeField] private string titleScene;

        public void Awake()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
        }

        public void OpenUIPanel()
        {
            Time.timeScale = 0f;
            uiPanel.SetActive(true);
        }

        public void CloseUIPanel()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
        }
        
        public void GoMainMenu()
        {
            SceneManager.LoadScene(titleScene);
        }
        
    }
}