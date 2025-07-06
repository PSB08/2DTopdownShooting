using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerCanAttackState : PlayerState
    {
        protected EntityAnimator _animator;
        
        protected static readonly int MoveXHash = Animator.StringToHash("X_MOVE");
        protected static readonly int MoveYHash = Animator.StringToHash("Y_MOVE");
        
        public PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _animator = entity.GetCompo<EntityAnimator>();
        }
        
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnAttackPressed += HandleAttackPressed;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnAttackPressed -= HandleAttackPressed;
            base.Exit();
        }

        private void HandleAttackPressed()
        {
            _player.ChangeState("ATTACK");
        }
        
        
    }
}