using System;
using System.Collections;
using Code.Scripts.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Core
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private int maxRound = 15;
        [SerializeField] private int baseSpawnCount = 2;
        [SerializeField] private EnemySpawner enemySpawner;

        public event Action<int> OnCountdown;
        public UnityEvent OnGameClear;

        private float _countdown = 3f;
        
        public int Countdown => (int)_countdown;
        
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
                ClearGame();
            }
            else
            {
                CurrentRound++;
                StartCoroutine(NextRoundDelay());
            }
        }

        private IEnumerator NextRoundDelay()
        {
            OnCountdown?.Invoke((int)_countdown);

            while (_countdown > 0)
            {
                yield return new WaitForSeconds(1f);
                _countdown--;
                OnCountdown?.Invoke((int)_countdown);
            }
            
            StartNextRound();
        }
        
        private void StartNextRound()
        {
            int spawnCount = baseSpawnCount + CurrentRound;
            enemySpawner.SetSpawnCount(spawnCount);
            enemySpawner.SpawnEnemies();
        }
        
        private void ClearGame()
        {
            OnGameClear?.Invoke();
        }
        
    }
}