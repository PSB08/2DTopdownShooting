using System;
using UnityEngine;

namespace Code.Scripts.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action OnStartAttackCast;
        public Action OnEndAttackCast;
        public Action OnDamageCastTrigger;
        
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        private void AnimationEnd()
        {
            OnAnimationEndTrigger?.Invoke();
        }
        private void DamageCast() => OnDamageCastTrigger?.Invoke();
        private void StartCast() => OnStartAttackCast?.Invoke();
        private void EndCast() => OnEndAttackCast?.Invoke();
        
    }
}