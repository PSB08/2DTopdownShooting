using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    [CreateAssetMenu(fileName = "BakedData", menuName = "SO/Path/BakedData", order = 0)]
    public class BakedDataSO : ScriptableObject
    {
        public List<NodeData> points = new  List<NodeData>();
        
        private Dictionary<Vector3Int, NodeData> _pointDict; // 이건 처음에 사용할 때 초기화 하면서 만들 딕셔너리
        
        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_pointDict == null || _pointDict.Count != points.Count)
                _pointDict = points.ToDictionary(node => node.cellPosition);
        }

        public void ClearPoints()
        {
            points?.Clear();
        }

        public void AddPoint(Vector3 worldPosition, Vector3Int cellPosition)
        {
            points.Add(new NodeData(worldPosition, cellPosition));
        }

        public bool HashNode(Vector3Int cellPosition) => _pointDict != null && _pointDict.ContainsKey(cellPosition);

        public bool TryGetNode(Vector3Int cellPosition, out NodeData nodeData)
        {
            if (HashNode(cellPosition))
            {
                nodeData = _pointDict[cellPosition];
                return true;
            }
            
            nodeData = default;
            return false;
        }
        
        
    }
}