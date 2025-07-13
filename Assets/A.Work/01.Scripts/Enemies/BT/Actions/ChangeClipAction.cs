using System;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeClip", story: "change to new [Clip] in [Renderer]", category: "Action", id: "d713cbef4907fe594c16a23c457221d8")]
    public partial class ChangeClipAction : Action
    {
        [SerializeReference] public BlackboardVariable<AnimParamSO> Clip;
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;

        protected override Status OnStart()
        {
            Renderer.Value.ChangeClip(Clip.Value);
            return Status.Success;
        }
        
    }
}

