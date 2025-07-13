using System;
using Code.Scripts.Enemies;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Code.Scripts.Enemies.BT.Events
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChannel", message: "change to [NewState]", category: "Events", id: "2533c9bb6b786273a202001722bf5d25")]
    public sealed partial class StateChannel : EventChannel<EnemyState> { }
}

