using Code.Scripts.Entities;
using Code.Scripts.FSM;
using UnityEngine;

namespace Code.Scripts.Players.States
{
    public class PlayerState : EntityState
    {
        protected Player _player;
        protected Transform _unitRoot;
        protected readonly float _inputThreshold = 0.1f;
        
        protected EntityAnimator _animator;
        protected CharacterMovement _movement;
        
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<CharacterMovement>();
            _animator = entity.GetCompo<EntityAnimator>();
            
            _unitRoot = _animator.transform;
        }
        
    }
}