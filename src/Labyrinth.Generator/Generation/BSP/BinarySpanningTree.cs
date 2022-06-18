using Labyrinth.Generator.Models;

namespace Labyrinth.Generator.Generation.BSP;

public class BinarySpanningTree
{
	private readonly int _dimensionX;
	private readonly int _dimensionY;
	private readonly int _maxPartitioning;
	private readonly Random _random;

	public BinarySpanningTree(int dimensionX, int dimensionY, int partitioning, Random random)
	{
		_dimensionX = dimensionX;
		_dimensionY = dimensionY;
		_maxPartitioning = partitioning;
		_random = random;
	}

	public Tile[,] Generate()
	{
		var grid = new Tile[_dimensionX, _dimensionY];
		var space = new Rectangle(new Position(0, 0), new Position(_dimensionX - 1, _dimensionY - 1));
		var root = new Node(null, space, grid, _random);

		var currentNodes = new Queue<Node>();
		currentNodes.Enqueue(root);
		var nextNodes = new Queue<Node>();
		for (var level = 1; level <= _maxPartitioning; level++)
		{
			while (currentNodes.Any())
			{
				var node = currentNodes.Dequeue();
				var direction = _random.Next(2);

				if (direction == 0)
					node.PartitionByX();
				else
					node.PartitionByY();

				nextNodes.Enqueue(node.Left);
				nextNodes.Enqueue(node.Right);
			}
			currentNodes = nextNodes;
			nextNodes = new Queue<Node>();
		}

		for (var level = _maxPartitioning; level >= 1; level--)
		{
			while (currentNodes.Any())
			{
				var left = currentNodes.Dequeue();
				var right = currentNodes.Dequeue();

				if (level == _maxPartitioning)
				{
					left.GenerateRoom();
					right.GenerateRoom();
				}

				// todo connect left to right

				nextNodes.Enqueue(left.Parent);
			}
			currentNodes = nextNodes;
			nextNodes = new Queue<Node>();
		}

		return grid;
	}
}
