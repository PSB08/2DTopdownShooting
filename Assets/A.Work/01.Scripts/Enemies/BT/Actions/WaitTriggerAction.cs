using System;
using Code.Scripts.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitTrigger", story: "Wait for [Trigger] end", category: "Action", id: "0c7fb41f7117f606f2900e0819ca0db6")]
    public partial class WaitTriggerAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityRenderer> Trigger;

        private bool _isTriggered;
        
        protected override Status OnStart()
        {
            _isTriggered = false;
            Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _isTriggered ? Status.Success: Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
        }

        private void HandleAnimationEnd() => _isTriggered = true;
        
        
    }
}

