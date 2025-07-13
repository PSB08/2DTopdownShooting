using System;
using Code.Scripts.Enemies.BT;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMovement", story: "stop [Movement]", category: "Action", id: "394bb7e24d1027b45844705951651b0d")]
    public partial class StopMovementAction : Action
    {
        [SerializeReference] public BlackboardVariable<AgentMovement> Movement;

        protected override Status OnStart()
        {
            Movement.Value.StopImmediately();
            return Status.Success;
        }
    
    }
}

