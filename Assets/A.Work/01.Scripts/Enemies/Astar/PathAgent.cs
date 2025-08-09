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
                pointArr[cornerIndex] = result[^1].worldPosition; 
                cornerIndex++;
            }
            
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
                AStarNode currentNode = openList.Pop(); 
                foreach (LinkData link in currentNode.nodeData.neighbours)
                {
                    bool isVisited = closedList.Any(n => n.cellPosition == link.endCellPosition);
                    if(isVisited) continue;
                    
                    if(!bakedData.TryGetNode(link.endCellPosition, out NodeData nextNode))
                        continue;

                    float newG = link.cost + currentNode.G;
                    
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
                
                closedList.Add(currentNode);

                if (currentNode.nodeData == endNodeData)
                {
                    result = true; 
                    break;
                }
            } // end of while

            if (result)
            {
                AStarNode last = closedList[^1]; 
                while (last.parentNode != null)
                {
                    path.Add(last);
                    last = last.parentNode; 
                }
                path.Add(last);
                path.Reverse(); 
            }

            return path;
        }

        private float CalcH(Vector3Int start, Vector3Int end) => Vector3Int.Distance(start, end);
        
    }
}