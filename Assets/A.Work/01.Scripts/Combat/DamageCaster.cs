using System.Collections.Generic;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Combat
{
    public abstract class DamageCaster : MonoBehaviour, IEntityComponent, IBtEntityComponent
    {
        [Header("Attack Range")]
        public Transform attackPoint;
        public AttackDataSO defaultAttackData;
        public float attackRange = 0.5f;
        public LayerMask targetLayer;

        protected Entity _entity;
        protected IComponentOwner _owner;
        
        [field:SerializeField] public HashSet<Collider2D> _hitTargets = new HashSet<Collider2D>();
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
        }

        public abstract bool CastDamage(DamageData damageData, Vector3 position, AttackDataSO attackData);

        public abstract void ApplyDamageAndKnockback(Transform target, DamageData damageData, Vector3 position,
            Vector3 normal, AttackDataSO attackData, Vector3 direction);
        
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
    }
}