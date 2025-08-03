using Code.Scripts.Combat;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Item.Test
{
    public class HealthUpItem : LevelUpItem
    {
        private EntityHealth _health;
        
        public override void ApplyItem(Entity targetEntity)
        {
            _health = targetEntity.GetCompo<EntityHealth>();
            var statCompo = targetEntity.GetCompo<EntityStat>();
            if (statCompo == null)
                Debug.LogError("No have attackCompo");

            if (_health.currentHealth >= 100)
            {
                statCompo.IncreaseBaseValue(_health.hpStat, changeValue);
                _health.OnMaxHealthChange?.Invoke();
            }
            else
            {
                _health.CurrentHpIncrease(changeValue);   
            }
            _levelUpItemSO.selectCount++;
            
            SizeUp(targetEntity);
        }
        
    }
}