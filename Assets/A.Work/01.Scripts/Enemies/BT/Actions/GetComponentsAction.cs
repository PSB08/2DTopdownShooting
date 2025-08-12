using System;
using System.Collections.Generic;
using Code.Scripts.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Code.Scripts.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponents", story: "get components from [Self]", category: "Action", id: "f5e2d924061465655576d03f69df147b")]
    public partial class GetComponentsAction : Action
    {
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;

        protected override Status OnStart()
        {
            List<BlackboardVariable> variableList = Self.Value.BtAgent.BlackboardReference.Blackboard.Variables;

            foreach (BlackboardVariable variable in variableList)
            {
                if (typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue;

                IEntityComponent targetComponent = Self.Value.GetCompo(variable.Type);
                Debug.Assert(targetComponent != null, $"{variable.Name} is not exist on {Self.Value.gameObject.name}");
                variable.ObjectValue = targetComponent;
            }
            return Status.Success;
        }
        
    }
}

