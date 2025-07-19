using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable, IAfterInitialize
    {
        private Entity _entity;
        private EntityActionData _actionData;
        private EntityStat _statCompo;
        
        [SerializeField] private StatSO hpStat;
        [SerializeField] private float maxHealth; 
        public float currentHealth;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<EntityActionData>();
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNormal, AttackDataSO attackData, Entity dealer)
        {
            _actionData.HitPoint = hitPoint;
            _actionData.HitNormal = hitNormal;

            currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);
            if (currentHealth <= 0)
            {
                _entity.OnDeadEvent?.Invoke();
            }
            
            _entity.OnHitEvent?.Invoke();
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