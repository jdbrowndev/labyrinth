using Labyrinth.Generator.Models;

namespace Labyrinth.Generator.Generation.BSP;

public class BinarySpacePartitionTree
{
	private readonly int _dimensionX;
	private readonly int _dimensionY;
	private readonly int _maxPartitioning;
	private readonly Random _random;

	public BinarySpacePartitionTree(int dimensionX, int dimensionY, int partitioning, Random random)
	{
		_dimensionX = dimensionX;
		_dimensionY = dimensionY;
		_maxPartitioning = partitioning;
		_random = random;
	}

	public Tile[,] Grid { get; private set; }
	public Node Root { get; private set; }

	public void Generate()
	{
		if (Root != null)
			throw new InvalidOperationException("Tree already generated");

		Grid = new Tile[_dimensionX, _dimensionY];
		var space = new Rectangle(new Position(0, 0), new Position(_dimensionX - 1, _dimensionY - 1));
		Root = new Node(null, space, Grid, _random);

		var currentNodes = new Queue<Node>();
		currentNodes.Enqueue(Root);
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
				else
				{
					left.ConnectChildRooms();
					right.ConnectChildRooms();
				}

				nextNodes.Enqueue(left.Parent);
			}
			currentNodes = nextNodes;
			nextNodes = new Queue<Node>();
		}
		Root.ConnectChildRooms();
	}
}
