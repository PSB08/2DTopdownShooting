using System;
using Code.Scripts.Enemies;
using UnityEngine;

namespace Code.Scripts.Core
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private int baseSpawnCount = 2;
        [SerializeField] private EnemySpawner enemySpawner;

        public int currentRound { get; private set; } = 0;

        private void Start()
        {
            enemySpawner.onAllEnemiesDead += OnRoundClear;
            StartNextRound();
        }

        private void OnRoundClear()
        {
            currentRound++;
            StartNextRound();
        }

        private void StartNextRound()
        {
            int spawnCount = baseSpawnCount + currentRound * 2;
            enemySpawner.SetSpawnCount(spawnCount);
            enemySpawner.SpawnEnemies();
        }
        
    }
}