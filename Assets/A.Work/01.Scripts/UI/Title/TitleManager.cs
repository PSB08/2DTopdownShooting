using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.UI.Title
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button settingBtn;
        [SerializeField] private Button exitBtn;

        [SerializeField] private string gameScene;
        
        [SerializeField] private GameObject soundUIPanel;
       
        [Header("DoTween Settings")]
        [SerializeField] private Vector2 hiddenPosition = new Vector2(0, 1000); 
        [SerializeField] private Vector2 shownPosition = Vector2.zero; 
        [SerializeField] private float tweenDuration = 0.3f;
        private bool _isTransitioning = false;


        private void Awake()
        {
            Time.timeScale = 1f;
            soundUIPanel.SetActive(false);
        }

        public void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine()
        {
            Debug.Log("Start Game");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(gameScene);
        }
        
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void SettingGame()
        {
            if (_isTransitioning) return;

            _isTransitioning = true;
            var rect = soundUIPanel.transform as RectTransform;
            rect.anchoredPosition = hiddenPosition;
            soundUIPanel.SetActive(true);
            rect.DOAnchorPos(shownPosition, tweenDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true)
                .OnComplete(() => _isTransitioning = false);
        }
        
        public void CloseSettingUIPanel()
        {
            if (!soundUIPanel.activeSelf || _isTransitioning) return;

            _isTransitioning = true;
            var rect = soundUIPanel.transform as RectTransform; 
            rect.DOAnchorPos(hiddenPosition, tweenDuration)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    soundUIPanel.SetActive(false);
                    _isTransitioning = false;
                });
        }

    }
}