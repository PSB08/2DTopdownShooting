using UnityEngine;

namespace Code.Scripts.Combat
{
    public class PlayerDamageCaster : DamageCaster
    {
        [SerializeField] private Vector2 castSize = new Vector2(1f, 0.5f);
        [SerializeField] private Vector2 verticalCastSize = new Vector2(0.5f, 1f);
        [SerializeField] private float castOffset = 0.5f;
        [SerializeField] private float castDistance = 1f;
        
        private Vector2 _lastCastDirection = Vector2.right;

        public override bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData)
        {
            Vector2 dir = direction.normalized;

            Vector2 size = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? castSize : verticalCastSize;

            Vector2 center = (Vector2)position + dir * (castDistance * 0.5f + castOffset);

            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, whatIsTarget);

            bool hitSuccess = false;
            foreach (var hit in hits)
            {
                if (hit != null)
                {
                    ApplyDamageAndKnockback(hit.transform, damageData, hit.transform.position, -dir, attackData, dir);
                    hitSuccess = true;
                }
            }

            _lastCastDirection = dir;
            return hitSuccess;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Vector2 dir = (Application.isPlaying && _lastCastDirection != Vector2.zero)
                ? _lastCastDirection
                : Vector2.right;

            Vector2 size = Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? castSize : verticalCastSize;
            Vector2 center = (Vector2)transform.position + dir * (castDistance * 0.5f + castOffset);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(center, size);
        }
        
#endif
        
    }
}