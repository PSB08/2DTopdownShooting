using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    [Serializable]
    public struct NodeData
    {
        public Vector3 worldPosition; // 월드 좌표
        public Vector3Int cellPosition; // 타일맵 좌표
        public List<LinkData> neighbours;

        public NodeData(Vector3 worldPosition, Vector3Int cellPosition)
        {
            this.worldPosition = worldPosition;
            this.cellPosition = cellPosition;
            neighbours = new List<LinkData>();
        }

        public void AddNeighbour(NodeData neighbour)
        {
            neighbours.Add(new LinkData
            {
                startPosition = worldPosition,
                startCellPosition = cellPosition,
                endPosition = neighbour.worldPosition,
                endCellPosition = neighbour.cellPosition,
                cost = Vector3Int.Distance(cellPosition, neighbour.cellPosition)
            });
        }

        public override int GetHashCode() => cellPosition.GetHashCode();
        public override bool Equals(object obj)
        {
            if (obj is NodeData data)
            {
                return data.cellPosition == cellPosition;
            }
            return false;
        }

        public static bool operator ==(NodeData lhs, NodeData rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(NodeData lhs, NodeData rhs)
        {
            return !(lhs == rhs);
        }
        
        
    }
}