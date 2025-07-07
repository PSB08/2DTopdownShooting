using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 hitNormal, Entity dealer);
    }
}