using System;
using Code.Scripts.Enemies;
using Code.Scripts.Enemies.BT.Events;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Feedbacks
{
    public class ReactPowerHitFeedback : Feedback
    {
        [SerializeField] private EntityActionData actionData;
        [SerializeField] private CommonEnemy enemy;
        
        private StateChange _stateChannel;

        private void Start()
        {
            _stateChannel = enemy.GetBlackboardVariable<StateChange>("StateChange");
        }

        public override void CreateFeedback()
        {
            _stateChannel.SendEventMessage(EnemyState.HIT);
        }

        public override void StopFeedback()
        {
            
        }
        
    }
}