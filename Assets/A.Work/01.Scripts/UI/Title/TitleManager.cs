using System;
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

        private void Awake()
        {
            Time.timeScale = 1f;
        }

        public void StartGame()
        {
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
            Debug.Log("Setting");
        }

    }
}