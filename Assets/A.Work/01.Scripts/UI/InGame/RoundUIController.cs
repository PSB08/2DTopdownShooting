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

        [Header("References")]
        [SerializeField] private RoundManager roundManager;
        [SerializeField] private EnemySpawner enemySpawner;

        private void Start()
        {
            if (enemySpawner != null)
            {
                enemySpawner.onAllEnemiesDead += () =>
                {
                    UpdateUI(); // 라운드 넘어갈 때도 업데이트
                };
            }

            UpdateUI(); // 초기 표시
        }

        private void Update()
        {
            UpdateUI(); // 실시간 적 수 갱신
        }

        private void UpdateUI()
        {
            if (roundManager != null)
            {
                roundText.text = $"ROUND: {roundManager.currentRound + 1}";
            }

            if (enemySpawner != null)
            {
                enemyCountText.text = $"ENEMIES: {enemySpawner.RemainingEnemyCount}";
            }
        }
        
    }
}