using UnityEngine;

namespace Code.Scripts.Enemies
{
    public class WayPoints : MonoBehaviour
    {
        [SerializeField] private WayPoint[] wayPoints;
        
        private int _currIndex = -1;

        public Vector3 GetNextWayPoint()
        {
            Debug.Assert(wayPoints.Length >= 1, "1개 이상의 waypoint 필요");
            _currIndex = (_currIndex + 1) % wayPoints.Length;
            return wayPoints[_currIndex].Position;
        }
        
    }
}