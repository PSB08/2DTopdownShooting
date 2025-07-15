using Code.Scripts.Combat;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IBtEntityComponent, IAfterInitialize
    {
        [Header("Attack Damage")]
        [SerializeField] private StatSO damageStat;
        private float _damage = 5f;
        
        private IComponentOwner _entity;
        private EnemyStat _statCompo;
        private EnemyDamageCaster _damageCaster;
        private EntityRenderer _animatorRenderer;
        
        private bool isAttacking = false;
        
        public void Initialize(IComponentOwner owner)
        {
            _entity = owner;
            _statCompo = owner.GetCompo<EnemyStat>();
            _damageCaster = owner.GetCompo<EnemyDamageCaster>();
            _animatorRenderer = owner.GetCompo<EntityRenderer>();
        }

        public void AfterInitialize()
        {
            _damage = _statCompo.SubscribeStat(damageStat, HandleDamageChange, 5f);   
            _animatorRenderer.OnDamageCastTrigger += AttackHit;
            _animatorRenderer.OnStartAttackCast += BeginAttack;
            _animatorRenderer.OnEndAttackCast += EndAttack;
        }
        
        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(damageStat, HandleDamageChange);
            _animatorRenderer.OnDamageCastTrigger -= AttackHit;
            _animatorRenderer.OnStartAttackCast -= BeginAttack;
            _animatorRenderer.OnEndAttackCast -= EndAttack;
        }

        private void HandleDamageChange(StatSO stat, float currentValue, float prevValue)
        {
            _damage = currentValue;
        }

        private void BeginAttack()
        {
            isAttacking = true;
            _damageCaster._hitTargets.Clear();
        }

        private void AttackHit()
        {
            if (!isAttacking) return;

            DamageData damageData = new DamageData { damage = _damage };
            _damageCaster.CastDamage(damageData, _damageCaster.attackPoint.position, _damageCaster.defaultAttackData);
        }
        
        private void EndAttack()
        {
            isAttacking = false;
            _damageCaster._hitTargets.Clear();
        }
        
        
    }
}