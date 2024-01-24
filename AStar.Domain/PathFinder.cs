using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStar.Domain
{
    public class PathFinder
    {
        private PathGrid pathGrid;

        public PathFinder(PathGrid grid)
        {
            pathGrid = grid;
        }

        public Task<Node[]> FindPath(Node startNode, Node endNode)
        {
            var _task = Task.Run(() =>
            {

                var path = new Stack<Node>();
                var initialNodePath = new NodePathData(startNode);

                var openNodes = new Dictionary<Guid, NodePathData>
            {
                { initialNodePath.NodeId, initialNodePath }
            };

                var closedNodes = new Dictionary<Guid, NodePathData>();


                while (openNodes.Any())
                {
                    var currentNode = openNodes.First();

                    foreach (var openNode in openNodes)
                    {
                        if (openNode.Value.CostBetterThanCurrent(currentNode.Value))
                            currentNode = openNode;
                    }

                    closedNodes.Add(currentNode.Key, currentNode.Value);
                    openNodes.Remove(currentNode.Key);

                    if (currentNode.Value.NodeId == endNode.nodeId)
                    {
                        var currentPathNode = currentNode.Value;

                        while (currentPathNode.NodeId != startNode.nodeId)
                        {
                            path.Push(currentPathNode.Node);

                            currentPathNode = currentPathNode.NodeLink;
                        }

                        break;
                    }

                    var neighbourNodes = pathGrid.GetHexagonalNeighbours(currentNode.Value.Node);

                    foreach (var neighbourNode in neighbourNodes)
                    {
                        NodePathData currentNodePathData = currentNode.Value;
                        NodePathData neighbourNodePath = null;
                        openNodes.TryGetValue(neighbourNode.nodeId, out neighbourNodePath);
                        bool nodeInOpenList = neighbourNodePath != null;

                        if (neighbourNodePath == null)
                            neighbourNodePath = new NodePathData(neighbourNode);

                        var costToNeighbour = currentNodePathData.GCost + currentNodePathData.GetDistanceFromNode(neighbourNode);

                        //we didn't check in open list or the cost from current node is better than before:
                        if (!nodeInOpenList || costToNeighbour < neighbourNodePath.GCost)
                        {
                            neighbourNodePath.UpdateGCost(costToNeighbour);
                            neighbourNodePath.NodeLink = currentNodePathData;

                            if (!nodeInOpenList)
                            {
                                neighbourNodePath.UpdateHCost(endNode, diagonal: true);
                                openNodes.Add(neighbourNodePath.NodeId, neighbourNodePath);

                                if (closedNodes.ContainsKey(neighbourNodePath.NodeId))
                                    closedNodes.Remove(neighbourNodePath.NodeId);
                            }

                        }

                    }

                }
                return path.ToArray();
            });

            return _task;
        }

    }
}
