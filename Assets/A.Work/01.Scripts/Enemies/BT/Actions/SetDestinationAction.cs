using System;
using Code.Scripts.Enemies.Astar;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetDestination", story: "[Agent] navigate to [NextPos]", category: "Action", id: "78e638649ab34b1a10bb571258dd3fba")]
    public partial class SetDestinationAction : Action
    {
        [SerializeReference] public BlackboardVariable<PathMovement> Agent;
        [SerializeReference] public BlackboardVariable<Vector3> NextPos;

        protected override Status OnStart()
        {
            Agent.Value.SetDestination(NextPos.Value);
            return Status.Success;
        }
    
    }
}

