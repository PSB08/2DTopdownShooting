using Code.Scripts.Enemies.BT;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Scripts.Enemies.Astar
{
    public class PathMovement : MonoBehaviour, IBtEntityComponent
    {
        [SerializeField] private PathAgent agent;
        // 저장 가능한 최대 경로 포인트 수
        [SerializeField] private int maxPathCount = 50;
        // 타일 좌표 계산용 타일맵
        [SerializeField] private Tilemap baseTilemap;

        // 경로 포인트 저장 배열
        private Vector3[] _pathArr;
        // 전체 경로 포인트 개수
        private int _totalPathCount;
        
        public bool IsArrived { get; private set; }  // 현재 경로 도착 여부
        public bool IsPathFailed { get; private set; }  // 경로 탐색 실패 여부
        public bool IsStop { get; set; }  // 강제 이동 정지 여부

        private IComponentOwner _owner;
        private AgentMovement _movement;
        private int _currentPathIndex = 0;
        private Vector2 _prevPosition;
        
        public void Initialize(IComponentOwner owner)
        {
            // 초기화: 소유자와 이동 컴포넌트, 경로 배열을 준비
            _owner = owner;
            _pathArr = new Vector3[maxPathCount];
            _movement = owner.GetCompo<AgentMovement>();
            baseTilemap = FindAnyObjectByType<Tilemap>();
        }

        public void SetDestination(Vector3 destination)
        {
            // 목적지를 설정하고 경로 계산
            _totalPathCount = 0;
            IsArrived = false;
            IsPathFailed = false;

            // 시작/목표 지점을 타일 좌표로 변환
            Vector3Int startCell = baseTilemap.WorldToCell(transform.position);
            Vector3Int endCell = baseTilemap.WorldToCell(destination);

            // 경로 계산
            _totalPathCount = agent.GetPath(startCell, endCell, _pathArr);

            if (_totalPathCount < 2)
            {
                IsPathFailed = true;
                return;
            }
            
            _prevPosition = _owner.Transform.position;
            _currentPathIndex = 1; // 0은 시작점이므로 다음 지점부터 이동
        }

        private void Update()
        {
            if (IsStop)
                return;

            if (_currentPathIndex >= _totalPathCount)
                return;

            if (!CheckArrive())
            {
                // 현재 경로 포인트 방향으로 이동
                Vector2 direction = _pathArr[_currentPathIndex] - _owner.Transform.position;
                _movement.SetMovement(direction.normalized);
            }
            else
            {
                // 목표 지점에 도착 시 즉시 멈춤
                _movement.StopImmediately();
            }
        }

        private bool CheckArrive()
        {
            Vector2 nextGoal = _pathArr[_currentPathIndex];  //다음
            Vector2 currPos = _owner.Transform.position;  //현재
            Vector2 prevDir = (nextGoal - _prevPosition).normalized;  //이전 목표 방향
            Vector2 curDir = (nextGoal - currPos).normalized;  //현재 목표 방향
            _prevPosition = currPos;

            // 방향이 반대가 되었거나, 목표와의 거리가 매우 짧으면 도착 처리
            if (Vector2.Dot(prevDir, curDir) <= 0 || Vector2.Distance(nextGoal, currPos) < 0.01f)
            {
                _currentPathIndex++;
                if (_currentPathIndex >= _totalPathCount)
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