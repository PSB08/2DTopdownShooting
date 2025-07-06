using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerAttackState : PlayerCanAttackState
    {
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack Enter");
            _player.ChangeState("IDLE");
        }
        
        
        
    }
}