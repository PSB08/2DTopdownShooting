using System;
using Code.Scripts.Combat;
using Code.Scripts.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI.InGame
{
    public class HpUIBar : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI hpTxt;

        private Entity _entity;
        private EntityHealth _health;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _health = entity.GetCompo<EntityHealth>();
        }

        private void OnDestroy()
        {
            _health.OnMaxHealthChange -= HealthChange;
        }

        private void Start()
        {
            _health.OnMaxHealthChange += HealthChange;
            slider.maxValue = _health.MaxHealth;
            hpTxt.text = $"{_health.currentHealth}/{_health.MaxHealth}";
        }

        private void Update()
        {
            slider.value = _health.currentHealth;
            hpTxt.text = $"{_health.currentHealth}/{_health.MaxHealth}";
        }

        private void HealthChange()
        {
            slider.maxValue = _health.MaxHealth;
        }
        
    }
}