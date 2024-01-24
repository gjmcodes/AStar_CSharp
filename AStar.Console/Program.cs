// See https://aka.ms/new-console-template for more information
using AStar.Domain;

Console.WriteLine("Starting A* Pathfinding");

const byte gridSize = 3;

var nodes = new Node[gridSize * gridSize];


int itx = 0;
for (byte y = 0; y < gridSize; y++)
{
    for (byte x = 0; x < gridSize; x++)
    {
        bool walkable = !(x == 1 && y == 1);
        if (walkable)
            walkable = !(x == 0 && y == 1);

        var node = new Node(x, y, walkable);
        nodes[itx] = node;
        itx++;
    }
}

var grid = new PathGrid(nodes);
var startNode = grid.FindNode(new Tuple<byte, byte>(0, 0));
var endNode = grid.FindNode(new Tuple<byte, byte>(2, 2));


var pathFinder = new PathFinder(grid);
var result = await pathFinder.FindPath(startNode.Value, endNode.Value);

Console.ReadLine();