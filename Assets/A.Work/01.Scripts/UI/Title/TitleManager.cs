using System;
using System.Collections;
using System.Collections.Generic;
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
            soundUIPanel.SetActive(true);
        }
        
        public void CloseSettingUIPanel()
        {
            soundUIPanel.SetActive(false);
        }

    }
}