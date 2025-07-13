using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Scripts.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckPlayer", story: "[Self] check [Target] in detectRange", category: "Conditions", id: "e7a6af4d32e79fa7f9cb6c720287a15a")]
    public partial class CheckPlayerCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            return Vector2.Distance(Self.Value.transform.position, Target.Value.position) < Self.Value.DetectRadius;
        }
    
    }
}
