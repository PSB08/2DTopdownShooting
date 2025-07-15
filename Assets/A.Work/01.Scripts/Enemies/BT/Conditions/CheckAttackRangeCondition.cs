using System;
using Code.Scripts.Enemies;
using Unity.Behavior;
using UnityEngine;

namespace Code.Scripts.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "CheckAttackRange", story: "[Self] check [Target] in attackRange", category: "Conditions", id: "46b0ad734ecc5bd1c7e9fdacb7d8c818")]
    public partial class CheckAttackRangeCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            return Vector2.Distance(Self.Value.transform.position, Target.Value.position) < Self.Value.AttackRadius;
        }
    
    }
}
