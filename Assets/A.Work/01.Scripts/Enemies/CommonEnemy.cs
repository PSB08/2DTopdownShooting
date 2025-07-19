using Unity.Behavior;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class CommonEnemy : Enemy
    {
        [field: SerializeField] public float DetectRadius { get; private set; } = 8f;
        [field: SerializeField] public float AttackRadius { get; private set; } = 1.5f;
        public BehaviorGraphAgent BtAgent { get; private set; }

        protected override void InitializeCompos()
        {
            base.InitializeCompos();
            BtAgent = GetComponent<BehaviorGraphAgent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, DetectRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
        
    }
}