using System;
using Code.Scripts.Combat;
using Code.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI.InGame
{
    public class HpUIBar : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Slider slider;

        private Entity _entity;
        private EntityHealth _health;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _health = entity.GetCompo<EntityHealth>();
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