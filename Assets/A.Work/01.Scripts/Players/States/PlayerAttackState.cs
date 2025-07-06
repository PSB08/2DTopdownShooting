using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackState : PlayerCanAttackState
    {
        private PlayerAttackCompo attackCompo;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
        }
        
        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
            {
                _player.ChangeState("IDLE");
            }
        }
        
        public override void Exit()
        {
            //_attackCompo.EndAttack();
            _movement.CanManualMovement = true;
            _movement.StopImmediately();
            base.Exit();
        }
        
        
    }
}