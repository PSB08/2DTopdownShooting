using UnityEngine;

namespace Code.Scripts.Entities
{
    public class EntityActionData : MonoBehaviour, IEntityComponent
    {
        public Vector2 HitPoint { get; set; }
        public Vector2 HitNormal { get; set; }

        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
    }
}