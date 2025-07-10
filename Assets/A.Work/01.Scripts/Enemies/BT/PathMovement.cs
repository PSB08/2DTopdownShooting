using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Scripts.Enemies.BT
{
    public class PathMovement : MonoBehaviour
    {
        //Astar 먼저 만들기 ㄱㄱㄱ
        //[SerializeField] private PathAgent agent;
        [SerializeField] private int maxPathCount = 50;
        [SerializeField] private Tilemap baseTilemap;

        private Vector3[] _pathArr;
        private int _totalPathCount;
        
        public bool IsArrived { get; private set; }
        public bool IsPathFailed { get; private set; }
        public bool IsStop { get; set; }

        private IComponentOwner _owner;
        private AgentMovement _movement;
        private int _currentPathIndex = 0;
        private Vector2 _prevPosition;
        
    }
}