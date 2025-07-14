using UnityEngine;

namespace Code.Scripts.Combat
{
    public class PlayerDamageCaster : DamageCaster
    {
        public override bool CastDamage(DamageData damageData, Vector3 position, AttackDataSO attackData)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(position, attackRange, targetLayer);

            bool hitSuccess = false;
            foreach (var hit in hits)
            {
                if (_hitTargets.Contains(hit)) continue;

                _hitTargets.Add(hit);

                Vector3 hitPos = hit.transform.position;
                Vector3 direction = (hitPos - position).normalized;
                Vector3 normal = -direction;

                ApplyDamageAndKnockback(hit.transform, damageData, hitPos, normal, attackData, direction);
                hitSuccess = true;
            }
            return hitSuccess;
        }
        
        public override void ApplyDamageAndKnockback(Transform target, DamageData damageData, Vector3 position,
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
        
    }
}