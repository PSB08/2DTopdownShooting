using Code.Scripts.Entities;
using Code.Scripts.Players;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI.InGame.PlayerUI
{
    public class PlayerSpeedUI : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private TextMeshProUGUI speedText;

        private Entity _entity;
        
        private CharacterMovement _movement;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _movement = entity.GetCompo<CharacterMovement>();
        }

        private void Update()
        {
            speedText.text = $"Spd : {_movement.MoveSpeed.ToString("F1")}";
        }
        
    }
}