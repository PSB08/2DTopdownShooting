using System;
using Code.Scripts.Entities;
using PSB_Lib.StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Enemies.BT
{
    public class AgentMovement : MonoBehaviour, IBtEntityComponent, IAfterInitialize
    {
        [field: SerializeField] public Rigidbody2D RigidCompo { get; private set; }
        public Vector2 Velocity => RigidCompo.linearVelocity;
        public bool CanManualMove { get; set; } = true;
        
        [field: SerializeField] public AnimParamSO VelocityParam { get; private set; }
        public UnityEvent<int, float> OnSpeedParamChange;
        public UnityEvent<float> OnXMoveChange;

        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private float moveSpeed = 5f;
        
        private EnemyActionData _actionData;
        private EnemyStat _statCompo;
        
        private IComponentOwner _owner;
        private Vector2 _moveInput;
        
        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
            _actionData = owner.GetCompo<EnemyActionData>();
            _statCompo = owner.GetCompo<EnemyStat>();
        }
        
        public void AfterInitialize()
        {
            moveSpeed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 10f);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentValue, float prevValue)
        {
            moveSpeed = currentValue;
        }
        
        public void StopImmediately()
        {
            _moveInput = Vector2.zero;
            RigidCompo.linearVelocity = Vector2.zero;
        }

        public void SetMovement(Vector2 input)
        {
            _moveInput = input;
        }

        private void FixedUpdate()
        {
            if (CanManualMove)
            {
                float xMove = Mathf.Approximately(RigidCompo.linearVelocityX,0) ? 0 : Mathf.Sign(RigidCompo.linearVelocityX);
                RigidCompo.linearVelocity = _moveInput * moveSpeed;
                OnXMoveChange?.Invoke(xMove);
            }
            if (VelocityParam != null)
            {
                float velocity = RigidCompo.linearVelocity.magnitude;
                OnSpeedParamChange?.Invoke(VelocityParam.paramHash, velocity);
            }
        }

        public void AddForceToEntity(Vector2 force)
        {
            RigidCompo.AddForce(force, ForceMode2D.Impulse);
        }
        
    }
}