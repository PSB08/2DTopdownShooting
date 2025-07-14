using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerMoveState : PlayerCanAttackState
    {
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInput.MovementKey;
            
            _movement.SetMovementDirection(movementKey);
            
            if (movementKey.x != 0)
            {
                _unitRoot.localScale = new Vector3(movementKey.x > 0 ? -1 : 1, 1, 1);
            }
            
            if (movementKey.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }
        
    }
}