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

        private Entity _entity;
        
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayer;

        [SerializeField] private StatSO damageStat;
        private EntityStat _statCompo;
        private float _damage = 5f;
        
        private HashSet<Collider2D> _hitEnemies = new HashSet<Collider2D>();
        private bool isAttacking = false;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            _damage = _statCompo.SubscribeStat(damageStat, HandleDamageChange, 5f);   
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(damageStat, HandleDamageChange);
        }

        private void HandleDamageChange(StatSO stat, float currentValue, float prevValue)
        {
            _damage = currentValue;
        }

        public void BeginAttack()
        {
            isAttacking = true;
            _hitEnemies.Clear();
        }

        public void AttackHit()
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
        
        public void EndAttack()
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