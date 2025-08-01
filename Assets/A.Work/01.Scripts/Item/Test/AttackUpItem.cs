using Code.Scripts.Entities;
using Code.Scripts.Players.States;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Item.Test
{
    public class AttackUpItem : LevelUpItem
    {
        private PlayerAttackCompo _attackCompo;

        public override void ApplyItem(Entity targetEntity)
        {
            _attackCompo = targetEntity.GetCompo<PlayerAttackCompo>();
            var statCompo = targetEntity.GetCompo<EntityStat>();
            if (statCompo == null)
            {
                Debug.LogError("No have attackCompo");
                return;
            }

            Debug.Log("AttackItemSelected!!");

            statCompo.IncreaseBaseValue(_attackCompo.damageStat, 10);
            _levelUpItemSO.selectCount++;
            
            SizeUp(targetEntity);
        }
        
    }
}