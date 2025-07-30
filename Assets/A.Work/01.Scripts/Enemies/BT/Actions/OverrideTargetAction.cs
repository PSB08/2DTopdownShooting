using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "OverrideTarget", story: "[Target] and [Nexus] in [Override]", category: "Action", id: "4f314173d940c9b200c319f2b9a00d8e")]
    public partial class OverrideTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<Transform> Nexus;
        [SerializeReference] public BlackboardVariable<EnemyOverride> Override;

        protected override Status OnStart()
        {
            if (Override.Value == null)
            {
                Debug.LogWarning("Override is null in BT");
                return Status.Failure;
            }

            if (Override.Value._player == null || Override.Value._nexus == null)
            {
                Debug.LogWarning("Player or Nexus is null in EnemyOverride");
                return Status.Failure;
            }

            Target.Value = Override.Value._player;
            Nexus.Value = Override.Value._nexus;
            return Status.Success;
        }

    
    }
}

