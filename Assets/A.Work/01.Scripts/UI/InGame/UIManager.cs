using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public abstract class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject uiPanel;

        public virtual void Awake()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
        }

        public virtual void OpenUIPanel()
        {
            Time.timeScale = 0f;
            uiPanel.SetActive(true);
        }
        
        public virtual void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        
    }
}