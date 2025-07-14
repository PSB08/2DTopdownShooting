using Code.Scripts.Combat;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        #region Temp Attack
        
        [Header("Attack Damage")]
        [SerializeField] private StatSO damageStat;
        private float _damage = 5f;
        
        private Entity _entity;
        private EntityStat _statCompo;
        private PlayerDamageCaster _damageCaster;
        private EntityAnimatorTrigger _animatorTrigger;
        
        private bool isAttacking = false;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            _damageCaster = entity.GetCompo<PlayerDamageCaster>();
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
        
        #endregion
        
    }
}