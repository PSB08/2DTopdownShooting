using System;
using System.Collections.Generic;
using Code.Scripts.Combat;
using PSB_Lib.Dependencies;
using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Enemies
{
    [Provide]
    public class EnemySpawner : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private List<PoolItemSO> enemies;
        [SerializeField] private Transform[] spawnPoints;
        
        [SerializeField] private Transform player;
        [SerializeField] private Transform nexus;
        
        private List<Enemy> _spawnedEnemies = new();
        private int _spawnCount = 1;

        public Action OnAllEnemiesDead;
        
        [Inject] private PoolManagerMono _poolManager;
        
        public int RemainingEnemyCount => _spawnedEnemies.Count;

        public void SetSpawnCount(int count)
        {
            _spawnCount = count;
        }

        public void SpawnEnemies()
        {
            _spawnedEnemies.Clear();

            for (int i = 0; i < _spawnCount; i++)
            {
                var randomEnemySO = enemies[UnityEngine.Random.Range(0, enemies.Count)];
                var spawnEnemy = _poolManager.Pop<Enemy>(randomEnemySO);

                var over = spawnEnemy.GetCompo<EnemyOverride>();
                if (over == null)
                {
                    Debug.LogError($"{spawnEnemy.name}에 EnemyOverride가 없습니다!");
                    continue; // 다음 루프
                }
                over.SetTargets(player, nexus);

                Vector3 spawnPos = GetRandomSpawnPosition(1f);
                spawnEnemy.transform.position = spawnPos;

                _spawnedEnemies.Add(spawnEnemy);

                AttachDeathListener(spawnEnemy);
            }
        }

        private Vector3 GetRandomSpawnPosition(float radius)
        {
            if (spawnPoints.Length == 0)
                return Vector3.zero;

            Vector3 basePos = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;

            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere;
            randomOffset.y = 0f;
            randomOffset = randomOffset.normalized * UnityEngine.Random.Range(0f, radius);

            return basePos + randomOffset;
        }

        private void AttachDeathListener(Enemy enemy)
        {
            if (enemy == null)
            {
                Debug.LogWarning($"Enemy가 없습니다.");
                return;
            }

            UnityAction onDead = null;
            onDead = () =>
            {
                enemy.OnDeadEvent.RemoveListener(onDead);
                _spawnedEnemies.Remove(enemy);

                if (_spawnedEnemies.Count == 0)
                    OnAllEnemiesDead?.Invoke();
            };
            enemy.OnDeadEvent.AddListener(onDead);
        }

        
        
    }
}