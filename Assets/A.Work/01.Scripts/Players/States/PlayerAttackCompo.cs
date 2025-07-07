using System.Collections.Generic;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
    {
        #region Temp Attack
        
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private LayerMask enemyLayer;
        
        private HashSet<Collider2D> _hitEnemies = new HashSet<Collider2D>();
        private bool isAttacking = false;
        
        public void Initialize(Entity entity)
        {
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
                Debug.Log("맞다" + enemy.name);
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