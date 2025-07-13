using System;
using Code.Scripts.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetNextPosition", story: "[Self] set [NextPosition] from [Target]", category: "Action", id: "0eedd04199f152ede3a41e36295e8047")]
    public partial class SetNextPositionAction : Action
    {
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;
        [SerializeReference] public BlackboardVariable<Vector3> NextPosition;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            NextPosition.Value = GetAttackPosition();
            return Status.Running;
        }

        private Vector3 GetAttackPosition()
        {
            Vector3 targetPos = Target.Value.position;
            Vector3 myPos = Self.Value.transform.position;
            
            float xDirection = Mathf.Sign(targetPos.x - myPos.x);
            targetPos.x -= xDirection;
            
            // 여기서 해당위치가 이동가능한 위치인지 파악하는 로직이 필요
            
            return targetPos;
        }
    
    }
}

