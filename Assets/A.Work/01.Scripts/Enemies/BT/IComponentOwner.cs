using UnityEngine;

namespace Code.Scripts.Enemies.BT
{
    public interface IComponentOwner
    {
        Transform Transform { get; }

        public T GetCompo<T>(bool isDerived = false) where T : IBtEntityComponent;
    }
}