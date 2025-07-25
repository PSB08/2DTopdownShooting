﻿using System;
using System.Collections.Generic;
using Code.Scripts.UI.InGame;
using PSB_Lib.Dependencies;
using PSB_Lib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private PoolItemSO enemy;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private EnemyArrow arrowManager;
        
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
                var spawnEnemy = _poolManager.Pop<Enemy>(enemy);
                var randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                spawnEnemy.transform.position = randomPoint.position;
                
                _spawnedEnemies.Add(spawnEnemy);
                
                arrowManager?.AddEnemy(spawnEnemy);
                
                var health = spawnEnemy.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    UnityAction onDead = null;
                    onDead = () =>
                    {
                        health.OnDeadEvent.RemoveListener(onDead);
                        _spawnedEnemies.Remove(spawnEnemy);

                        arrowManager?.RemoveEnemy(spawnEnemy);
                        
                        if (_spawnedEnemies.Count == 0)
                            OnAllEnemiesDead?.Invoke();
                    };

                    health.OnDeadEvent.AddListener(onDead);
                }
                else
                {
                    Debug.LogWarning($"Enemy {spawnEnemy.name}에 EnemyHealth가 없습니다.");
                }
            }
        }

        
        
    }
}