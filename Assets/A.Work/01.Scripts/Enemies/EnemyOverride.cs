using Code.Scripts.Entities;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class EnemyOverride : MonoBehaviour, IEntityComponent
    {
        public Transform _player;
        public Transform _nexus;

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void SetTargets(Transform player, Transform nexus)
        {
            _player = player;
            _nexus = nexus;
        }
        
    }
}