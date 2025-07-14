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
            
            if (moveDir.x != 0)
            {
                _unitRoot.localScale = new Vector3(moveDir.x > 0 ? -1 : 1, 1, 1);
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