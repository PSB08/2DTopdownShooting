using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class EnemyActionData : MonoBehaviour, IBtEntityComponent
    {
        public Vector2 HitPoint { get; set; }
        public Vector2 HitNormal { get; set; }

        private IComponentOwner _entity;
        
        public void Initialize(IComponentOwner owner)
        {
            _entity = owner;
        }
        
    }
}