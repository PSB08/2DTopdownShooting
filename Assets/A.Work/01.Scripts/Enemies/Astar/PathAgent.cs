using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    public class PathAgent : MonoBehaviour
    {
        [SerializeField] private BakedDataSO bakedData;

        public int GetPath(Vector3Int startPosition, Vector3Int destination, Vector3[] pointArr)
        {
            List<AStarNode> result = CalculatePath(startPosition, destination);
            // result를 기반으로 pointArr에 넣어준다.
            int cornerIndex = 0;
            if (result.Count > 0)
            {
                pointArr[cornerIndex] = result[0].worldPosition;
                cornerIndex++;

                for (int i = 1; i < result.Count - 1; i++)
                {
                    if(cornerIndex >= pointArr.Length) break;
                    
                    Vector3Int beforeDirection = result[i].cellPosition - result[ i - 1 ].cellPosition;
                    Vector3Int nextDirection = result[i + 1].cellPosition - result[i].cellPosition;

                    if (beforeDirection != nextDirection)
                    {
                        pointArr[cornerIndex] = result[i].worldPosition;
                        cornerIndex++;
                    }
                }
                pointArr[cornerIndex] = result[^1].worldPosition; // 최종 목적지는 수동으로 넣어준다.
                cornerIndex++;
            }
            // pointArr = result.Select(node => node.worldPosition).ToArray();
            return cornerIndex;
        }

        private List<AStarNode> CalculatePath(Vector3Int start, Vector3Int end)
        {
            PriorityQueue<AStarNode> openList = new PriorityQueue<AStarNode>();
            List<AStarNode> closedList = new List<AStarNode>();
            List<AStarNode> path = new List<AStarNode>();
            
            bool result = false;

            if (!bakedData.TryGetNode(start, out NodeData startNodeData))
                return path;

            if (!bakedData.TryGetNode(end, out NodeData endNodeData))
                return path;
            
            openList.Push(new AStarNode
            {
                nodeData = startNodeData,
                cellPosition = startNodeData.cellPosition,
                worldPosition = startNodeData.worldPosition,
                parentNode = null,
                G = 0, F = CalcH(startNodeData.cellPosition, endNodeData.cellPosition)
            });

            while (openList.Count > 0)
            {
                AStarNode currentNode = openList.Pop(); // 가장 F값이 작은 녀석이 나옴
                foreach (LinkData link in currentNode.nodeData.neighbours)
                {
                    // 해당 노드가 이미 방문한 노드인지 검사.
                    bool isVisited = closedList.Any(n => n.cellPosition == link.endCellPosition); // Any : Linq식, 조건을 만족하는 놈이 하나라도 있으면 true
                    if(isVisited) continue; // 이미 방문한 노드면 다음 노드로
                    
                    if(!bakedData.TryGetNode(link.endCellPosition, out NodeData nextNode))
                        continue;

                    float newG = link.cost + currentNode.G;
                    // 이동하려는 노드의 비용과 현재까지 이동한 G값을 더한다.

                    AStarNode nextAStarNode = new AStarNode
                    {
                        nodeData = nextNode,
                        cellPosition = nextNode.cellPosition,
                        worldPosition = nextNode.worldPosition,
                        parentNode = currentNode,
                        G = newG, F = newG + CalcH(nextNode.cellPosition, endNodeData.cellPosition)
                    };

                    AStarNode existNode = openList.Contains(nextAStarNode);
                    if (existNode == null)
                    {
                        openList.Push(nextAStarNode);
                    }
                } // end foreach
                
                closedList.Add(currentNode); // 계산이 끝난 노드는 closeNode로 들어감

                if (currentNode.nodeData == endNodeData)
                {
                    result = true; // 목적지 도착
                    break;
                }
            } // end of while

            if (result)
            {
                AStarNode last = closedList[^1]; // 마지막으로 방문한 곳을 시작점으로 잡고 ^1 : 끝에서 첫번째 원소
                while (last.parentNode != null)
                {
                    path.Add(last);
                    last = last.parentNode; // 쭉 따라서 올라간다.
                }
                path.Add(last);
                path.Reverse(); // 순서 역순으로 변경해야 정순이 된다.
            }

            return path;
        }

        private float CalcH(Vector3Int start, Vector3Int end) => Vector3Int.Distance(start, end);
        
    }
}