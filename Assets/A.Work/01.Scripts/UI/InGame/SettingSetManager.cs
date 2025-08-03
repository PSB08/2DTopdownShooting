using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class SettingSetManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private SoundSetManager soundSetManager;

        [SerializeField] private float tweenDuration = 0.3f;
        private bool _isOpen;
        private bool _isTransitioning;
        
        [SerializeField] private SoundID openSound;

        public void Awake()
        {
            Time.timeScale = 1f;
            settingPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (soundSetManager.IsOpen())
                {
                    soundSetManager.CloseSoundUIPanel();
                }
                else
                {
                    ToggleSettingPanel();
                }
            }
        }

        private void ToggleSettingPanel()
        {
            if (_isOpen) CloseUIPanel();
            else OpenUIPanel();
        }
        
        public void OpenUIPanel()
        {
            if (_isTransitioning) return;
            //사ㅏ운드
            BroAudio.Play(openSound);
            
            Time.timeScale = 0f;
            _isTransitioning = true;
            settingPanel.SetActive(true);
            settingPanel.transform.localScale = Vector3.zero;
            settingPanel.transform.DOScale(Vector3.one, tweenDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    _isTransitioning = false;
                    _isOpen = true;
                });
        }

        public void CloseUIPanel()
        {
            if (!settingPanel.activeSelf || _isTransitioning) return;

            _isTransitioning = true;
            settingPanel.transform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    settingPanel.SetActive(false);
                    Time.timeScale = 1f;
                    _isTransitioning = false;
                    _isOpen = false;
                });
        }

        
    }
}