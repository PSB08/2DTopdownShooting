using System;
using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Entities;
using PSB_Lib.ObjectPool.RunTime;
using Unity.Behavior;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public abstract class Enemy : Entity, IPoolable
    {
        public BehaviorGraphAgent BtAgent { get; private set; }
        protected EntityRenderer EntityRenderer;

        protected override void AddComponents()
        {
            base.AddComponents();
            
            EntityRenderer = GetCompo<EntityRenderer>();
            BtAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(BtAgent != null, $"{gameObject.name} don't have BehaviorGraphAgent");
        }
        
        
        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (BtAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            return default;
        }


        public PoolItemSO PoolItem { get; }
        public void SetUpPool(Pool pool)
        {
            
        }

        public void ResetItem()
        {
            
        }
        
    }
}