using System.Collections.Generic;
using UnityEngine;

namespace PSB_Lib.ObjectPool.RunTime
{
    [CreateAssetMenu(fileName = "PoolManager", menuName = "SO/Pool/Manager", order = 0)]
    public class PoolManagerSO : ScriptableObject
    {
        public List<PoolItemSO> itemList = new List<PoolItemSO>();
        
        private Dictionary<PoolItemSO, Pool> _pools;
        private Transform _rootTrm;

        public void Initialize(Transform rootTrm)
        {
            _rootTrm = rootTrm;
            _pools = new Dictionary<PoolItemSO, Pool>();

            foreach (var item in itemList)
            {
                IPoolable poolable = item.prefab.GetComponent<IPoolable>();
                Debug.Assert(poolable != null, $"Pooling item does not have IPoolable component {item.prefab.name}");
                
                //하이어라키에서 예쁘게ㅔ 한다
                Transform poolParent = new GameObject(item.prefab.name).transform;
                poolParent.SetParent(_rootTrm);
                //여기까지 세팅
                
                Pool pool = new Pool(poolable, poolParent, item.initCount);
                _pools.Add(item, pool);
            }
        }

        public IPoolable Pop(PoolItemSO item)
        {
            if (_pools.TryGetValue(item, out Pool pool))
                return pool.Pop();
            return null;
        }

        public void Push(IPoolable item)
        {
            if (_pools.TryGetValue(item.PoolItem, out Pool pool))
                pool.Push(item);
        }

    }
}