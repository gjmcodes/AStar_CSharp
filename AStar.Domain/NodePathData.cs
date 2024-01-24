using System;
using System.Runtime.CompilerServices;

namespace AStar.Domain
{
    public class NodePathData
    {
        const float DiagonalCost = 1.414f;
        const float DistanceCost = 1f;

        public Guid NodeId => Node.nodeId;
        public Node Node { get; private set; }
        public NodePathData? NodeLink { get; set; } //For backtracking
        public float GCost { get; private set; } = 0; //distance from this node to the initial node
        public float HCost { get; private set; } //heuristics: distance from this node to the target node
        public float FCost => GCost + HCost; //the lowest the FCost the better for A* pathfinding

        public NodePathData(Node node)
        {
            this.Node = node;
        }

        public float GetDiagonalDistanceFromNode(Node nodeFrom)
        {
            var distanceX = Math.Abs(this.Node.positionX - nodeFrom.positionX);
            var distanceY = Math.Abs(this.Node.positionY - nodeFrom.positionY);
            float distanceCost = Math.Min(distanceX, distanceY) + (Math.Max(distanceX, distanceY) - Math.Min(distanceX, distanceY)) * DiagonalCost;

            return distanceCost;
        }

        public float GetDistanceFromNode(Node nodeFrom)
        {
            var distanceX = Math.Abs(this.Node.positionX - nodeFrom.positionX);
            var distanceY = Math.Abs(this.Node.positionY - nodeFrom.positionY);
            float distanceCost = distanceX + distanceY;

            return distanceCost;
        }

        public float UpdateHCost(Node nodeFrom, bool diagonal = false)
        {
            if (diagonal)
                HCost = GetDiagonalDistanceFromNode(nodeFrom);
            else
                HCost = GetDistanceFromNode(nodeFrom);

            return HCost;
        }



        public float UpdateGCost(float movementCost)
        {
            GCost = movementCost;

            return GCost;
        }

        public bool CostBetterThanCurrent(NodePathData currentNode)
        {
            return this.FCost < currentNode.FCost || this.FCost == currentNode.FCost && this.HCost < currentNode.HCost;
        }
    }
}
