using System;
using System.Collections.Generic;
using System.Linq;

namespace AStar.Domain
{
    public class PathGrid
    {
        public Dictionary<Tuple<byte, byte>, Node> Grid { get; private set; }

        private byte gridMax_X;
        private byte gridMax_Y;
        public PathGrid(Node[] nodes)
        {
            Grid = new Dictionary<Tuple<byte, byte>, Node>();
            for (int i = 0; i < nodes.Length; i++)
            {
                var gridKey = new Tuple<byte, byte>(nodes[i].positionX, nodes[i].positionY);
                Grid.Add(gridKey, nodes[i]);
            }

            gridMax_X = nodes.Max(x => x.positionX);
            gridMax_Y = nodes.Max(y => y.positionY);
        }

        public Node? FindNode(Tuple<byte, byte> position)
        {
            if (!Grid.ContainsKey(position))
                return null;

            var node = Grid[position];

            return node;
        }

        public Node? FindWalkableNode(Tuple<byte, byte> position)
        {
            var node = FindNode(position);
            if (node == null || node.Value.walkable == false) return null;

            return node;
        }
        public Node[] GetHexagonalNeighbours(Node node)
        {
            var neighbourNodes = new List<Node>();

            //#1 ↖
            if (node.positionX > 0)
            {
                var _nTopLeftIdx = new Tuple<byte, byte>(Convert.ToByte(node.positionX - 1), node.positionY);
                var _nTopLeft = FindWalkableNode(_nTopLeftIdx);
                if (_nTopLeft.HasValue) neighbourNodes.Add(_nTopLeft.Value);
            }

            //#2 ↑
            if (node.positionY > 0)
            {
                var _nTopIdx = new Tuple<byte, byte>(node.positionX, Convert.ToByte(node.positionY - 1));
                var _nTop = FindWalkableNode(_nTopIdx);
                if (_nTop.HasValue) neighbourNodes.Add(_nTop.Value);
            }

            //#3 ↗
            if (node.positionX + 1 <= gridMax_X)
            {
                var _nTopRightIdx = new Tuple<byte, byte>(Convert.ToByte(node.positionX + 1), node.positionY);
                var _nTopRight = FindWalkableNode(_nTopRightIdx);
                if (_nTopRight.HasValue) neighbourNodes.Add(_nTopRight.Value);
            }

            //#4 ↘
            if (node.positionX + 1 <= gridMax_X && node.positionY + 1 <= gridMax_Y)
            {
                var _nBottomRightIdx = new Tuple<byte, byte>(Convert.ToByte(node.positionX + 1), Convert.ToByte(node.positionY + 1));
                var _nBottomRight = FindWalkableNode(_nBottomRightIdx);
                if (_nBottomRight.HasValue) neighbourNodes.Add(_nBottomRight.Value);
            }


            //#5 ↓
            if (node.positionY + 1 <= gridMax_Y)
            {
                var _nBottomIdx = new Tuple<byte, byte>(node.positionX, Convert.ToByte(node.positionY + 1));
                var _nBottom = FindWalkableNode(_nBottomIdx);
                if (_nBottom.HasValue) neighbourNodes.Add(_nBottom.Value);
            }

            //#6 ↙
            if (node.positionX > 0 && node.positionY <= gridMax_Y)
            {
                var _nBottomLeftIdx = new Tuple<byte, byte>(Convert.ToByte(node.positionX - 1), Convert.ToByte(node.positionY + 1));
                var _nBottomLeft = FindWalkableNode(_nBottomLeftIdx);
                if (_nBottomLeft.HasValue) neighbourNodes.Add(_nBottomLeft.Value);
            }

            return neighbourNodes.ToArray();
        }
    }
}
