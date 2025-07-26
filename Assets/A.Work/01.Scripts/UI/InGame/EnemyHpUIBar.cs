using System;
using Code.Scripts.Enemies;
using Code.Scripts.Enemies.BT;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI.InGame
{
    public class EnemyHpUIBar : MonoBehaviour, IBtEntityComponent
    {
        [SerializeField] private Slider slider;

        private IComponentOwner _owner;
        private EnemyHealth _health;

        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
            _health = owner.GetCompo<EnemyHealth>();
        }

        private void Awake()
        {
            slider.maxValue = _health.MaxHealth;
        }

        private void Update()
        {
            slider.value = _health.currentHealth;
        }
        
        
    }
}