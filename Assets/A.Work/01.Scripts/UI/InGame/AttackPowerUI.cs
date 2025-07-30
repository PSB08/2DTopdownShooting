using System;
using Code.Scripts.Players.States;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI.InGame
{
    public class AttackPowerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;

        private PlayerAttackCompo _attackCompo;
        
        private void Awake()
        {
            _attackCompo = GetComponent<PlayerAttackCompo>();
        }

        private void Update()
        {
            damageText.text = $"Atk : {_attackCompo.Damage.ToString()}";
        }
        
        
    }
}