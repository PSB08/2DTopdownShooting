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
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
        }

        private void OnDestroy()
        {
            
        }

        protected override void Start()
        {
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

        
    }
}
