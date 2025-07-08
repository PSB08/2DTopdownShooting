using Code.Scripts.Entities;
using Code.Scripts.FSM;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;
        
        protected EntityAnimator _animator;
        
        protected static readonly int MoveXHash = Animator.StringToHash("X_MOVE");
        protected static readonly int MoveYHash = Animator.StringToHash("Y_MOVE");

        protected CharacterMovement _movement;
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<CharacterMovement>();
            _animator = entity.GetCompo<EntityAnimator>();
        }
        
    }
}