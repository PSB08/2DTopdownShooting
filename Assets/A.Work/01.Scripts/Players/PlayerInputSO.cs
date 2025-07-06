using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Players
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Player", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private LayerMask whatIsGround;
        public event Action OnAttackPressed;

        public Vector2 MovementKey { get; private set; }

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();   
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAttackPressed?.Invoke();
        }
        
        
        
    }
}