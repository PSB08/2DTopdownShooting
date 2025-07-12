using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code.Scripts.Enemies.Astar
{
    public class PathBaker : MonoBehaviour
    {
        [SerializeField] private Tilemap groundMap;
        [SerializeField] private Tilemap obstacleMap;
        [SerializeField] private BakedDataSO bakedData;
        [SerializeField] private bool isCornerCheck = true;
        [SerializeField] private bool isDrawGizmos = true;
        [SerializeField] private Color nodeColor, edgeColor;

        [ContextMenu("Bake Map Data")]
        private void BakeMapData()
        {
            Debug.Assert(groundMap != null && obstacleMap != null, "Target tilemap must be attached");

            WritePointData();
            RecordNeighbors();
            SaveIfInUnityEditor();
        }

        private void SaveIfInUnityEditor()
        {
#if UNITY_EDITOR
            
            EditorUtility.SetDirty(bakedData);
            AssetDatabase.SaveAssets();
            
#endif
        }

        private void WritePointData()
        {
            bakedData.ClearPoints(); // 모든 데이터 지우고 다시 적는다.
            groundMap.CompressBounds();
            
            BoundsInt mapBound = groundMap.cellBounds;

            for (int x = mapBound.xMin; x < mapBound.xMax; x++)
            {
                for (int y = mapBound.yMin; y < mapBound.yMax; y++)
                {
                    Vector3Int searchPoint = new Vector3Int(x, y);
                    if (CanMovePosition(searchPoint))
                    {
                        AddPoint(searchPoint);
                    }
                }
            }
        }
        
        private void RecordNeighbors()
        {
            foreach (NodeData nodeData in bakedData.points)
            {
                nodeData.neighbours.Clear();
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if(x == 0 && y == 0) continue; // 자기 자신
                        
                        Vector3Int nextPoint = new Vector3Int(x, y) + nodeData.cellPosition;

                        if (bakedData.TryGetNode(nextPoint, out NodeData adjacentNode))
                        {
                            if(CheckCorner(nextPoint, nodeData.cellPosition))
                                nodeData.AddNeighbour(adjacentNode);
                        }
                    }
                }
            }
        }

        private bool CheckCorner(Vector3Int nextPoint, Vector3Int currentPoint)
        {
            if (!isCornerCheck) return true;

            return CanMovePosition(new Vector3Int(nextPoint.x, currentPoint.y)) &&
                CanMovePosition(new Vector3Int(currentPoint.x, nextPoint.y));
        }
        
        private void AddPoint(Vector3Int searchPoint)
        {
            Vector3 worldPoint = groundMap.GetCellCenterWorld(searchPoint);
            bakedData.AddPoint(worldPoint, searchPoint);
        }

        private bool CanMovePosition(Vector3Int searchPoint)
        {
            bool hashObstacle = obstacleMap.HasTile(searchPoint);
            bool hasFloor = groundMap.HasTile(searchPoint);
            
            return !hashObstacle && hasFloor;
        }
        
        #if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (!isDrawGizmos) return;

            foreach (NodeData nodeData in bakedData.points)
            {
                Gizmos.color = nodeColor;
                Gizmos.DrawWireSphere(nodeData.worldPosition, 0.15f);

                foreach (LinkData linkData in nodeData.neighbours)
                {
                    Gizmos.color = edgeColor;
                    DrawArrowGizmos(linkData.startPosition, linkData.endPosition);
                }
            }
        }

        private void DrawArrowGizmos(Vector3 from, Vector3 to)
        {
            Vector3 direction = (from - to).normalized;
            
            Vector3 arrowStart = to - direction * 0.2f;
            Vector3 arrowEnd = to - direction * 0.15f;
            const float arrowSize = 0.05f;
            
            Vector3 triangleA = arrowStart + (Quaternion.Euler(0,0,-90f) * direction) * arrowSize;
            Vector3 triangleB = arrowStart + (Quaternion.Euler(0,0,90f) * direction) * arrowSize;
            
            Gizmos.DrawLine(from, arrowStart);
            Gizmos.DrawLine(triangleA, arrowEnd);
            Gizmos.DrawLine(triangleB, arrowEnd);
            Gizmos.DrawLine(triangleA, triangleB);
        }
        
        #endif
        
    }
}