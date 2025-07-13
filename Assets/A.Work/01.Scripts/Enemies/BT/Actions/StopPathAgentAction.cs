using System;
using Code.Scripts.Enemies.Astar;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopPathAgent", story: "set [Agent] stop to [IsStop]", category: "Action", id: "a2fb85ecc2e8b6ed8a36214e8acc0199")]
    public partial class StopPathAgentAction : Action
    {
        [SerializeReference] public BlackboardVariable<PathMovement> Agent;
        [SerializeReference] public BlackboardVariable<bool> IsStop;

        protected override Status OnStart()
        {
            Agent.Value.IsStop = IsStop.Value;
            return Status.Success;
        }
        
    }
}

