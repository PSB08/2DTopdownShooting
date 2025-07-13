using System;
using Code.Scripts.Enemies.Astar;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForArrive", story: "[Agent] wait for arrive", category: "Action", id: "d2cb96d9819c333fc1ca05eead98e962")]
    public partial class WaitForArriveAction : Action
    {
        [SerializeReference] public BlackboardVariable<PathMovement> Agent;

        protected override Status OnUpdate()
        {
            if(Agent.Value.IsArrived)
                return Status.Success;
            if(Agent.Value.IsPathFailed)
                return Status.Failure;
        
            return Status.Running;
        }
    
    
    }
}

