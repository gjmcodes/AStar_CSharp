using System;

namespace AStar.Domain
{
    public struct Node
    {
        public Guid nodeId;
        public byte positionX;
        public byte positionY;
        public bool walkable;

        public Node(byte positionX, byte positionY, bool walkable)
        {
            this.nodeId = Guid.NewGuid();
            this.positionX = positionX;
            this.positionY = positionY;
            this.walkable = walkable;
        }
    }
}
