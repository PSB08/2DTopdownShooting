using System;
using UnityEngine;

namespace Code.Scripts.Enemies.Astar
{
    public class AStarNode : IComparable<AStarNode>
    {
        public float G;
        public float F; // H는 계산해서 사용

        public Vector3 worldPosition;
        public Vector3Int cellPosition;
        public NodeData nodeData;
        
        public AStarNode parentNode;
        
        public int CompareTo(AStarNode other)
        {
            if(Mathf.Approximately(other.F, this.F))
                return 0;
            return other.F < F ? -1 : 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is AStarNode node)
            {
                return Equals(node);
            }
            return false;
        }

        private bool Equals(AStarNode node)
        {
            if (node is null) return false;
            return cellPosition == node.cellPosition;
        }
        
        public override int GetHashCode() => cellPosition.GetHashCode();

        public static bool operator ==(AStarNode lhs, AStarNode rhs)
        {
            if (lhs is null)
            {
                if (rhs is null) return true;
                return false;
            }
            
            return lhs.Equals(rhs);
        }

        public static bool operator !=(AStarNode lhs, AStarNode rhs)
        {
            return !(lhs == rhs);
        }
        
    }
}