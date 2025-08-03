using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Code.Scripts.Enemies.BT.Events
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChange", message: "state change to [NewValue]", category: "Events",
        id: "7d1e06d8f56130f4abbef0f493f8235b")]
    public partial class StateChange : EventChannelBase
    {
        public delegate void StateChangeEventHandler(EnemyState newValue);
        public event StateChangeEventHandler Event; 

        public void SendEventMessage(EnemyState newValue)
        {
            Event?.Invoke(newValue);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<EnemyState> newValueBlackboardVariable = messageData[0] as BlackboardVariable<EnemyState>;
            var newValue = newValueBlackboardVariable != null ? newValueBlackboardVariable.Value : default(EnemyState);

            Event?.Invoke(newValue);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            StateChangeEventHandler del = (newValue) =>
            {
                BlackboardVariable<EnemyState> var0 = vars[0] as BlackboardVariable<EnemyState>;
                if(var0 != null)
                    var0.Value = newValue;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as StateChangeEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as StateChangeEventHandler;
        }
    }
}

