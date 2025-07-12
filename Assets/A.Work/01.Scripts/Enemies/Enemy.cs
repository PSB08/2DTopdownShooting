using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Enemies.BT;
using Code.Scripts.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Enemies
{
    public abstract class Enemy : MonoBehaviour, IComponentOwner
    {
        protected Dictionary<Type, IBtEntityComponent> _compoDict;

        private void Awake()
        {
            _compoDict = GetComponentsInChildren<IBtEntityComponent>(true).ToDictionary(compo => compo.GetType());

            InitializeCompos();
            AfterInitializeCompos();
        }
        
        protected virtual void InitializeCompos()
        {
            _compoDict.Values.ToList().ForEach(compo => compo.Initialize(this));
        }

        protected virtual void AfterInitializeCompos()
        {
            _compoDict.Values.OfType<IAfterInitialize>().ToList().ForEach(compo => compo.AfterInitialize());
        }

        public Transform Transform => transform;

        public T GetCompo<T>(bool isDerived = false) where T : IBtEntityComponent
        {
            if (_compoDict.TryGetValue(typeof(T), out IBtEntityComponent compo))
                return (T)compo;
            
            if(isDerived == false) return default;
            
            Type findType = _compoDict.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if(findType != null)
                return (T) _compoDict[findType];
            
            return default;
        }

        public IBtEntityComponent GetCompo(Type type)
        {
            return _compoDict.GetValueOrDefault(type);
        }
        
        
    }
}