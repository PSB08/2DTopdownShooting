using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerIdleState : PlayerCanAttackState
    {
        public PlayerIdleState(Entity entity, int animationHash) : base(entity, animationHash)
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
            if (movementKey.magnitude > 0.01f)
            {
                _animator.SetParam(MoveXHash, movementKey.x);
                _animator.SetParam(MoveYHash, movementKey.y);
                _player.ChangeState("MOVE");
            }
        }
        
    }
}