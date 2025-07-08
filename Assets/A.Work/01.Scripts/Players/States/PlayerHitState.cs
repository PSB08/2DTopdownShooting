using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerHitState : PlayerState
    {
        public PlayerHitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
        }
        
        public override void Update()
        {
            base.Update();
            Vector2 moveDir = _player.PlayerInput.MovementKey;
            
            if (moveDir.sqrMagnitude > 0.01f)
            {
                _animator.SetParam(MoveXHash, moveDir.x);
                _animator.SetParam(MoveYHash, moveDir.y);
            }
            
            if (moveDir.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            base.Exit();
            _movement.CanManualMovement = true;
        }
    }
}