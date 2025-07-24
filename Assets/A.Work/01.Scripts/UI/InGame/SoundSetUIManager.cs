using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class SoundSetUIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject uiPanel;
        private bool _isOpen;

        public void Awake()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ToggleUIPanel();
        }
        
        private void ToggleUIPanel()
        {
            if (_isOpen) CloseUIPanel();
            else OpenUIPanel();
        }

        public void OpenUIPanel()
        {
            Time.timeScale = 0f;
            uiPanel.SetActive(true);
            _isOpen = true;
        }

        public void CloseUIPanel()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
            _isOpen = false;
        }
        
        
    }
}