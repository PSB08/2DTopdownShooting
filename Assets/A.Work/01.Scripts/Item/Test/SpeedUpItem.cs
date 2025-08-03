using Code.Scripts.Entities;
using Code.Scripts.Players;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Item.Test
{
    public class SpeedUpItem : LevelUpItem
    {
        private CharacterMovement _characterMovement;
        
        public override void ApplyItem(Entity targetEntity)
        {
            _characterMovement = targetEntity.GetCompo<CharacterMovement>();
            var statCompo = targetEntity.GetCompo<EntityStat>();
            if (statCompo == null)
                Debug.LogError("No have attackCompo");

            statCompo.IncreaseBaseValue(_characterMovement.moveSpeedStat, changeValue);
            _levelUpItemSO.selectCount++;
            
            SizeUp(targetEntity);
        }
        
    }
}