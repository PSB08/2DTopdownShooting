﻿using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Enemies.BT;
using PSB_Lib.StatSystem;
using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class EnemyStat : MonoBehaviour, IBtEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private Dictionary<string, StatSO> _stats = new Dictionary<string, StatSO>();
        
        public IComponentOwner Owner { get; private set; }
        
        public void Initialize(IComponentOwner owner)
        {
            Owner = owner;
            _stats = statOverrides.ToDictionary(so => so.StatName, so => so.CreateStat());
        }
        
        public StatSO GetStat(StatSO stat)
        {
            Debug.Assert(stat != null, "Finding stat cannot be null");
            return _stats.GetValueOrDefault(stat.statName);
        }

        public bool TryGetStat(StatSO stat, out StatSO outStat)
        {
            Debug.Assert(stat != null, "Finding stat cannot be null");
            
            outStat = _stats.GetValueOrDefault(stat.statName);
            return outStat != null;
        }
        
        public void SetBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue = value;
        
        public float GetBaseValue(StatSO stat)
            => GetStat(stat).BaseValue;
        
        public void IncreaseBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue += value;
        
        public void AddModifier(StatSO stat, object key, float value)
            => GetStat(stat).AddModifier(key, value);
        
        public void RemoveModifier(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void ClearAllStatModifier()
            => _stats.Values.ToList().ForEach(s => s.ClearModifier());

        public float SubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if (target == null) return defaultValue;
            target.OnValueChanged += handler;
            return target.Value;
        }

        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangeHandler handler)
        {
            StatSO target = GetStat(stat);
            if (target == null) return;
            target.OnValueChanged -= handler;
        }
        
    }
}