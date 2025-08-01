using System;
using System.Collections;
using Code.Scripts.Enemies;
using PSB_Lib.Dependencies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Core
{
    [Provide]
    public class RoundManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private int maxRound = 15;
        [SerializeField] private int baseSpawnCount = 2;
        [SerializeField] private float countdownSeconds = 3f;
        
        
        [Inject] private EnemySpawner enemySpawner;

        public event Action<int> OnCountdown;
        public UnityEvent OnGameClear;
        public UnityEvent OnRoundClearEvent;

        public int MaxRound => maxRound;
        public int CurrentRound { get; private set; } = 0;
        
        private bool _waitEvent = false;

        private void Start()
        {
            enemySpawner.OnAllEnemiesDead += OnRoundClear;
            StartCoroutine(NextRoundDelay()); 
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
                _waitEvent = true;
                OnRoundClearEvent?.Invoke();
            }
        }
        
        public void NotifyRoundEventFinished()
        {
            if (!_waitEvent) return;
            _waitEvent = false;
            StartCoroutine(NextRoundDelay());
        }

        private IEnumerator NextRoundDelay()
        {
            float countdown = countdownSeconds;

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

        private void ClearGame()
        {
            OnGameClear?.Invoke();
        }
        
        
    }
}