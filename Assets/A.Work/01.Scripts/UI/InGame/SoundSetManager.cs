using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class SoundSetManager : MonoBehaviour
    {
        [SerializeField] private GameObject uiPanel;
        [SerializeField] private Vector2 hiddenPosition = new Vector2(0, 1000); 
        [SerializeField] private Vector2 shownPosition = Vector2.zero; 
        [SerializeField] private float tweenDuration = 0.3f;
        private bool _isTransitioning = false;

        public void Awake()
        {
            uiPanel.SetActive(false);
        }

        public void OpenSoundUIPanel()
        {
            Time.timeScale = 0f;
            if (_isTransitioning) return;

            _isTransitioning = true;
            var rect = uiPanel.transform as RectTransform;
            rect.anchoredPosition = hiddenPosition;
            uiPanel.SetActive(true);
            rect.DOAnchorPos(shownPosition, tweenDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnComplete(() => _isTransitioning = false);
        }

        public void CloseSoundUIPanel()
        {
            if (!uiPanel.activeSelf || _isTransitioning) return;

            _isTransitioning = true;
            var rect = uiPanel.transform as RectTransform;
            rect.DOAnchorPos(hiddenPosition, tweenDuration)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    Time.timeScale = 1f;
                    uiPanel.SetActive(false);
                    _isTransitioning = false;
                });
        }

        public bool IsOpen()
        {
            return uiPanel.activeSelf && !_isTransitioning;
        }

        
    }
}