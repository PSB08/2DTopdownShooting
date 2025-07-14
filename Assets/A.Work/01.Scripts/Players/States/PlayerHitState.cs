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
            
            if (moveDir.x != 0)
            {
                _unitRoot.localScale = new Vector3(moveDir.x > 0 ? -1 : 1, 1, 1);
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