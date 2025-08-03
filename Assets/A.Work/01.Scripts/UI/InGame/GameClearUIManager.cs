using System.Collections;
using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.UI.InGame
{
    public class GameClearUIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject uiPanel;
        [SerializeField] private string titleScene;
        
        [SerializeField] private float tweenDuration = 0.3f;
        private bool _isTransitioning = false;
        
        [SerializeField] private SoundID gameClearSound;

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
        
        private IEnumerator OpenPanelCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            //사운드
            BroAudio.Stop(BroAudioType.Music);
            BroAudio.Play(gameClearSound);
            
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
        
        public void GoMainMenu()
        {
            SceneManager.LoadScene(titleScene);
        }
        
    }
}