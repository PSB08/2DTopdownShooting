using System;
using Code.Scripts.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetNextWayPoint", story: "set [NextPoint] from [WayPoints]", category: "Action", id: "2f5270efc3eb506536b84ad0c728bae6")]
    public partial class SetNextWayPointAction : Action
    {
        [SerializeReference] public BlackboardVariable<Vector3> NextPoint;
        [SerializeReference] public BlackboardVariable<WayPoints> WayPoints;

        protected override Status OnStart()
        {
            NextPoint.Value = WayPoints.Value.GetNextWayPoint();
            return Status.Success;
        }
    
    }
}

