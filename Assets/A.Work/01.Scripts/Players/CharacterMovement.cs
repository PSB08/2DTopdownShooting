using System;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Players
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private Rigidbody2D rigid2D;

        private EntityStat _statCompo;
        
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
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            _moveSpeed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 8f);
        }
        
        private void OnDestroy()
        { 
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            Move();
        }
        
        #region Temp

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                _statCompo.SetBaseValue(moveSpeedStat, 4f);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                _statCompo.SetBaseValue(moveSpeedStat, 8f);
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                _statCompo.SetBaseValue(moveSpeedStat, 12f);
            }
        }

        #endregion
        
        private void HandleMoveSpeedChange(StatSO stat, float currentValue, float prevValue)
        {
            _moveSpeed = currentValue;
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
            Vector2 move = _velocity.normalized * (_moveSpeed * Time.fixedDeltaTime);
            rigid2D.MovePosition(rigid2D.position + move);
        }

        public void StopImmediately()
        {
            _movementDirection = Vector2.zero;
        }
        
    }
}