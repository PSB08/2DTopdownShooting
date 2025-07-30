using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.UI.InGame
{
    public class GameOverUIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject uiPanel;   
        
        [SerializeField] private float tweenDuration = 0.3f;
        private bool _isTransitioning = false;

        public void Awake()
        {
            Time.timeScale = 1f;
            uiPanel.SetActive(false);
        }

        public void OpenUIPanel()
        {
            if (uiPanel.activeSelf || _isTransitioning) return;
            
            StartCoroutine(OpenPanelCoroutine());
        }

        public void CloseUIPanel()
        {
            if (!uiPanel.activeSelf || _isTransitioning) return;

            _isTransitioning = true;
            uiPanel.transform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    uiPanel.SetActive(false);
                    Time.timeScale = 1f;
                    _isTransitioning = false;
                });
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
        }

        private IEnumerator OpenPanelCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            Time.timeScale = 0f;
            _isTransitioning = true;
            uiPanel.SetActive(true);
            uiPanel.transform.localScale = Vector3.zero;
            uiPanel.transform.DOScale(Vector3.one, tweenDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _isTransitioning = false;
                });
        }

    }
}