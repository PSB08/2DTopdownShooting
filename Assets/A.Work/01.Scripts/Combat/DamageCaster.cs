using System.Collections.Generic;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Combat
{
    public abstract class DamageCaster : MonoBehaviour
    {
        [SerializeField] protected LayerMask whatIsTarget;

        protected Entity _owner;

        public virtual void InitOwner(Entity entity)
        {
            _owner = entity;
        }

        public virtual void ApplyDamageAndKnockback(Transform target, DamageData damageData, Vector3 position,
            Vector3 normal, AttackDataSO attackData, Vector3 direction)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData, position, normal, attackData, _owner);
            }

            if (target.TryGetComponent(out IKnockbackable knockbackable))
            {
                Vector2 force = new Vector2(direction.x, direction.y).normalized * attackData.knockBackForce;
                knockbackable.Knockback(force, attackData.knockBackDuration);
            }
        }
        
        public abstract bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData);
        
    }
}