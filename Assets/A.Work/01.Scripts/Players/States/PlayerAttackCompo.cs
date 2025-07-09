using System.Collections.Generic;
using Code.Scripts.Combat;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        #region Temp Attack
        
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private AttackDataSO defaultAttackData;

        [SerializeField] private StatSO damageStat;
        private EntityStat _statCompo;
        private float _damage = 5f;
        
        private HashSet<Collider2D> _hitEnemies = new HashSet<Collider2D>();
        private bool isAttacking = false;
        
        private Entity _entity;
        private EntityAnimatorTrigger _animatorTrigger;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
        }
        
        public void AfterInitialize()
        {
            _damage = _statCompo.SubscribeStat(damageStat, HandleDamageChange, 5f);   
            _animatorTrigger.OnDamageCastTrigger += AttackHit;
            _animatorTrigger.OnStartAttackCast += BeginAttack;
            _animatorTrigger.OnEndAttackCast += EndAttack;
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(damageStat, HandleDamageChange);
            _animatorTrigger.OnDamageCastTrigger -= AttackHit;
            _animatorTrigger.OnStartAttackCast -= BeginAttack;
            _animatorTrigger.OnEndAttackCast -= EndAttack;
        }

        private void HandleDamageChange(StatSO stat, float currentValue, float prevValue)
        {
            _damage = currentValue;
        }

        private void BeginAttack()
        {
            isAttacking = true;
            _hitEnemies.Clear();
        }

        private void AttackHit()
        {
            if (!isAttacking) return;

            DamageData damageData = new DamageData { damage = _damage };
            CastDamage(damageData, attackPoint.position, defaultAttackData);
        }
        
        public void ApplyDamageAndKnockback(Transform target, DamageData damageData, Vector3 position,
            Vector3 normal, AttackDataSO attackData, Vector3 direction)
        {
            if (target.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(damageData, position, normal, attackData, _entity);
            }

            if (target.TryGetComponent(out IKnockbackable knockbackable))
            {
                Vector2 force = new Vector2(direction.x, direction.y).normalized * attackData.knockBackForce;
                knockbackable.Knockback(force, attackData.knockBackDuration);
            }
        }
        
        public bool CastDamage(DamageData damageData, Vector3 position, AttackDataSO attackData)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, attackRange, enemyLayer);

            bool hitSuccess = false;
            foreach (var hit in hits)
            {
                if (_hitEnemies.Contains(hit)) continue;

                _hitEnemies.Add(hit);

                Vector3 hitPos = hit.transform.position;
                Vector3 direction = (hitPos - position).normalized;
                Vector3 normal = -direction;

                ApplyDamageAndKnockback(hit.transform, damageData, hitPos, normal, attackData, direction);
                hitSuccess = true;
            }
            return hitSuccess;
        }
        
        
        private void EndAttack()
        {
            isAttacking = false;
            _hitEnemies.Clear();
        }
        
        #endregion

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
    }
}