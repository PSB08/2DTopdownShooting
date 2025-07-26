using Code.Scripts.Core;
using Code.Scripts.Enemies;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class RoundUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI enemyCountText;
        [SerializeField] private TextMeshProUGUI countdownText;

        [Header("References")]
        [SerializeField] private RoundManager roundManager;
        [SerializeField] private EnemySpawner enemySpawner;

        private void Start()
        {
            if (enemySpawner != null)
            {
                enemySpawner.OnAllEnemiesDead += UpdateUI;
            }

            if (roundManager != null)
            {
                roundManager.OnCountdown += ShowCountdown;
            }

            UpdateUI();
        }


        private void Update()
        {
            UpdateEnemyCount();
        }

        private void UpdateUI()
        {
            UpdateRoundText();
            UpdateEnemyCount();
        }

        private void UpdateRoundText()
        {
            if (roundManager == null || roundText == null)
                return;

            int displayRound = roundManager.CurrentRound + 1;
            
            if (displayRound >= roundManager.MaxRound)
            {
                roundText.text = "FINAL ROUND";
            }
            else
            {
                roundText.text = $"ROUND: {displayRound}";
            }
        }

        private void UpdateEnemyCount()
        {
            if (enemySpawner == null || enemyCountText == null)
                return;

            enemyCountText.text = $"ENEMIES: {enemySpawner.RemainingEnemyCount}";
        }
        
        private void ShowCountdown(int secondsLeft)
        {
            if (countdownText == null) return;

            if (secondsLeft > 0)
            {
                roundText.gameObject.SetActive(false);
                enemyCountText.gameObject.SetActive(false);
                countdownText.gameObject.SetActive(true);
                countdownText.text = $"{secondsLeft}";
            }
            else
            {
                roundText.gameObject.SetActive(false);
                enemyCountText.gameObject.SetActive(false);
                countdownText.gameObject.SetActive(true);
                countdownText.text = "라운드 시작";

                Invoke(nameof(HideCountdown), 1f);
            }
        }
        
        private void HideCountdown()
        {
            countdownText.gameObject.SetActive(false);
            roundText.gameObject.SetActive(true);
            enemyCountText.gameObject.SetActive(true);
        }
        
        
    }
}