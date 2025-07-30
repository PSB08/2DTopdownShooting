using Code.Scripts.Entities;
using Code.Scripts.FSM;
using UnityEngine;

namespace Code.Scripts.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        
        [SerializeField] private StateDataSO[] states;

        private EntityStateMachine _stateMachine;
        private EntityAnimatorTrigger _trigger;
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            _trigger = GetCompo<EntityAnimatorTrigger>();
        }

        private void OnDestroy()
        {
            _trigger.OnDeadEndTrigger -= DestroyEntity;
        }
        
        protected override void Start()
        {
            _trigger.OnDeadEndTrigger += DestroyEntity;
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();

            if (Input.GetKeyDown(KeyCode.H))
            {
                _stateMachine.ChangeState("HIT");
            }
        }
        
        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);

        public void ChangeDead()
        {
            ChangeState("DEAD");
        }
        
    }
}
