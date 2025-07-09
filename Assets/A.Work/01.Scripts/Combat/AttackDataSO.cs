using UnityEngine;

namespace Code.Scripts.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public float damageMultiplier = 1f;
        public float damageIncrease = 0;
        
        public float knockBackForce;
        public float knockBackDuration;
    }
}