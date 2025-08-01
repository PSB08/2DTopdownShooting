using System;
using Code.Scripts.Entities;
using Code.Scripts.Players.States;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI.InGame.PlayerUI
{
    public class AttackPowerUI : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private TextMeshProUGUI damageText;

        private Entity _entity;
        
        private PlayerAttackCompo _attackCompo;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        private void Update()
        {
            damageText.text = $"Atk : {_attackCompo.Damage.ToString()}";
        }
        
        
    }
}