using System;
using Code.Scripts.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FlipTowardsTarget", story: "[Renderer] flip towards [Target]", category: "Action", id: "dd017d98dfd2f3a75c96363727b9a213")]
    public partial class FlipTowardsTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            Renderer.Value.FlipTowardsTarget(Target.Value);
            return Status.Success;
        }

        
    }
}

