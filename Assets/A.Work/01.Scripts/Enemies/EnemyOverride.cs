using System;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Nexuses;
using Code.Scripts.Players;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class EnemyOverride : MonoBehaviour, IBtEntityComponent
    {
        public Transform _player;
        public Transform _nexus;
     
        private IComponentOwner _owner;
        
        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
        }
        
        private void Awake()
        {
            _player = FindAnyObjectByType<Player>().transform;
            _nexus = FindAnyObjectByType<Nexus>().transform;
        }
        
    }
}