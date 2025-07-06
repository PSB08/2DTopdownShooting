using Code.Scripts.Entities;
using Code.Scripts.FSM;

namespace Code.Scripts.Players.States
{
    public class PlayerState : EntityState
    {
        protected Player _player;
        protected readonly float _inputThreshold = 0.1f;

        protected CharacterMovement _movement;
        public PlayerState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _player = entity as Player;
            _movement = entity.GetCompo<CharacterMovement>();
        }
        
    }
}