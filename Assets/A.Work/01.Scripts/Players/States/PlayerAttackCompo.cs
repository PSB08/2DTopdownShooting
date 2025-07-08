using System;
using System.Collections.Generic;
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
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hits)
            {
                if (_hitEnemies.Contains(enemy)) continue;
                
                _hitEnemies.Add(enemy);
                Debug.Log("피격 처리" + " " + enemy.name + " " + _damage);
            }
            
        }
        
        private void EndAttack()
        {
            isAttacking = false;
            _hitEnemies.Clear();
        }
        
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
    }
}