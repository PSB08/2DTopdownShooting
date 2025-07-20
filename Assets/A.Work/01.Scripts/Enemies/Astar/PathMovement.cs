using Code.Scripts.Enemies.BT;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Scripts.Enemies.Astar
{
    public class PathMovement : MonoBehaviour, IBtEntityComponent
    {
        [SerializeField] private PathAgent agent;
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
        
        public void Initialize(IComponentOwner owner)
        {
            _owner = owner;
            _pathArr = new Vector3[maxPathCount];
            _movement = owner.GetCompo<AgentMovement>();
            baseTilemap = FindAnyObjectByType<Tilemap>();
        }

        public void SetDestination(Vector3 destination)
        {
            _totalPathCount = 0;
            IsArrived = false;
            IsPathFailed = false;

            Vector3Int startCell = baseTilemap.WorldToCell(transform.position);
            Vector3Int endCell = baseTilemap.WorldToCell(destination);

            _totalPathCount = agent.GetPath(startCell, endCell, _pathArr);

            if (_totalPathCount < 2)
            {
                IsPathFailed = true;
                return;
            }
            
            _prevPosition = _owner.Transform.position;
            _currentPathIndex = 1; // 첫번째 점(내 위치)는 안가고 다음 위치를 감.
        }

        private void Update()
        {
            if (IsStop)
                return;

            if (_currentPathIndex >= _totalPathCount)
                return;

            if (!CheckArrive())
            {
                Vector2 direction = _pathArr[_currentPathIndex] - _owner.Transform.position;
                _movement.SetMovement(direction.normalized);
            }
            else
            {
                _movement.StopImmediately(); // 도착했다면 정지.
            }
        }

        private bool CheckArrive()
        {
            Vector2 nextGoal = _pathArr[_currentPathIndex];
            Vector2 currPos = _owner.Transform.position;
            Vector2 prevDir = (nextGoal - _prevPosition).normalized;
            Vector2 curDir = (nextGoal - currPos).normalized;
            _prevPosition = currPos; // 갱신

            if (Vector2.Dot(prevDir, curDir) <= 0 || Vector2.Distance(nextGoal, currPos) < 0.01f)
            {
                _currentPathIndex++;
                if (_currentPathIndex >= _totalPathCount) // 도착
                {
                    IsArrived = true;
                    return true;
                }
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_totalPathCount <= 0) return;

            for (int i = 0; i < _totalPathCount - 1; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_pathArr[i], 0.25f);
                Gizmos.DrawLine(_pathArr[i], _pathArr[i + 1]);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_pathArr[_totalPathCount - 1], 0.25f);
        }
        
    }
}