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
            if (_isTriggerCall)
            {
                _player.ChangeState("IDLE");
            }
        }

        public override void Exit()
        {
            _movement.CanManualMovement = true;
            base.Exit();
        }
        
        
    }
}