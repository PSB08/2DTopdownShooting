using System;
using System.Collections;
using Code.Scripts.Enemies;
using UnityEngine;

namespace Code.Scripts.Core
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private int maxRound = 15;
        [SerializeField] private int baseSpawnCount = 2;
        [SerializeField] private EnemySpawner enemySpawner;

        public event Action<int> OnCountdown;
        public event Action OnGameEnd;

        public int MaxRound => maxRound;
        public int CurrentRound { get; private set; } = 0;

        private void Start()
        {
            enemySpawner.OnAllEnemiesDead += OnRoundClear;
            StartNextRound();
        }

        private void OnRoundClear()
        {
            if (CurrentRound + 1 >= maxRound)
            {
                EndGame();
            }
            else
            {
                CurrentRound++;
                StartCoroutine(NextRoundDelay());
            }
        }

        private IEnumerator NextRoundDelay()
        {
            float countdown = 3f;
            
            OnCountdown?.Invoke((int)countdown);

            while (countdown > 0)
            {
                yield return new WaitForSeconds(1f);
                countdown--;
                OnCountdown?.Invoke((int)countdown);
            }
            
            StartNextRound();
        }
        
        private void StartNextRound()
        {
            int spawnCount = baseSpawnCount + CurrentRound;
            enemySpawner.SetSpawnCount(spawnCount);
            enemySpawner.SpawnEnemies();
        }
        
        private void EndGame()
        {
            OnGameEnd?.Invoke();
        }
        
    }
}