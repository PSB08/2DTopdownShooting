using UnityEngine;

namespace Code.Scripts.Combat
{
    public interface IKnockbackable
    {
        public void Knockback(Vector2 force, float time);
    }
}