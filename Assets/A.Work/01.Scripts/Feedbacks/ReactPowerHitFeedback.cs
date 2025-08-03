using System;
using Code.Scripts.Enemies;
using Code.Scripts.Enemies.BT.Events;
using UnityEngine;

namespace Code.Scripts.Feedbacks
{
    public class ReactPowerHitFeedback : Feedback
    {
        [SerializeField] private EnemyActionData actionData;
        [SerializeField] private CommonEnemy enemy;
        
        private StateChange _stateChannel;

        private void Start()
        {
            _stateChannel = enemy.GetBlackboardVariable<StateChange>("StateChange");
        }

        public override void CreateFeedback()
        {
            Debug.Log("HitFeedback");
            _stateChannel.SendEventMessage(EnemyState.HIT);
        }

        public override void StopFeedback()
        {
            throw new System.NotImplementedException();
        }
        
    }
}