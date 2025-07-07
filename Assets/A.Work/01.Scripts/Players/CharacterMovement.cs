using System;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Players
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private Rigidbody2D rigid2D;

        public bool CanManualMovement { get; set; } = true;
        private Vector2 _autoMovement;

        private float _moveSpeed = 8f;
        private Vector2 _velocity;
        
        public Vector2 Velocity => _velocity;
        private float _verticalVelocity;
        private Vector3 _movementDirection;

        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void AfterInitialize()
        {
            
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            Move();
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection = new Vector3(movementInput.x, movementInput.y, 0).normalized;
        }
        
        private void CalculateMovement()
        {
            if (CanManualMovement)
                _velocity = _movementDirection * _moveSpeed;
            else
                _velocity = _autoMovement;
            
            if (_velocity.sqrMagnitude > 0.001f)
            {
                transform.up = _velocity;
            }
        }
        
        private void Move()
        {
            rigid2D.MovePosition(rigid2D.position + _velocity * Time.fixedDeltaTime);
        }

        public void StopImmediately()
        {
            _movementDirection = Vector2.zero;
        }
        
    }
}