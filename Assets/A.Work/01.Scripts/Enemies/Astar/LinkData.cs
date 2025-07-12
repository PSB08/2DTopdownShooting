using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    public class LinkData : MonoBehaviour
    {
        public Vector3 startPosition;
        public Vector3Int startCellPosition;
        public Vector3 endPosition;
        public Vector3Int endCellPosition;

        public float cost; // 이 경로의 비용
        
    }
}