using Code.Scripts.Combat;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Enemies
{
    public class EnemyHealth : MonoBehaviour, IBtEntityComponent, IDamageable, IAfterInitialize
    {
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        private IComponentOwner _entity;
        private EnemyActionData _actionData;
        private EnemyStat _statCompo;
        
        [SerializeField] private StatSO hpStat;
        [SerializeField] private float maxHealth;
        public float currentHealth;
        
        public float MaxHealth => maxHealth;
        
        public void Initialize(IComponentOwner owner)
        {
            _entity = owner;
            _actionData = owner.GetCompo<EnemyActionData>();
            _statCompo = owner.GetCompo<EnemyStat>();
        }

        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNormal, AttackDataSO attackData, Entity dealer)
        {
            _actionData.HitPoint = hitPoint;
            _actionData.HitNormal = hitNormal;

            currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);
            if (currentHealth <= 0)
            {
                OnDeadEvent?.Invoke();
            }
            
            OnHitEvent?.Invoke();
        }

        public void AfterInitialize()
        {
            currentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleMaxHPChange, 10f);
        }
        
        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleMaxHPChange);
        }
        
        private void HandleMaxHPChange(StatSO stat, float currentValue, float prevValue)
        {
            float changed = currentValue - prevValue;
            maxHealth = currentValue;
            if (changed > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth + changed, 0, maxHealth);
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
        }
        
    }
}