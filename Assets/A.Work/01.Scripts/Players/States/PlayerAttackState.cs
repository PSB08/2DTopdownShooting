using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackState : PlayerCanAttackState
    {
        private PlayerAttackCompo _attackCompo;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            
            Vector2 moveDir = _player.PlayerInput.MovementKey;
            
            if (moveDir.sqrMagnitude > 0.01f)
            {
                _animator.SetParam(MoveXHash, moveDir.x);
                _animator.SetParam(MoveYHash, moveDir.y);
            }
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