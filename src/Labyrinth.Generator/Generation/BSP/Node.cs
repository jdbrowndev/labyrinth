using Labyrinth.Generator.Models;

namespace Labyrinth.Generator.Generation.BSP;

public class Node
{
	private readonly Tile[,] _grid;
	private readonly Random _random;

	public Node(Node parent, Rectangle space, Tile[,] grid, Random random)
	{
		Parent = parent;
		Space = space;

		_grid = grid;
		_random = random;
	}

	public Node Parent { get; private set; }
	public Node Left { get; private set; }
	public Node Right { get; private set; }
	public Rectangle Space { get; private set; }

	public void PartitionByX()
	{
		var diffX = Space.BottomRight.X - Space.TopLeft.X;
		var borderX = Space.TopLeft.X + diffX / 2;
		var leftSpace = Space with { BottomRight = Space.BottomRight with { X = borderX } };
		var rightSpace = Space with { TopLeft = Space.TopLeft with { X = borderX } };

		Left = new Node(this, leftSpace, _grid, _random);
		Right = new Node(this, rightSpace, _grid, _random);
	}

	public void PartitionByY()
	{
		var diffY = Space.BottomRight.Y - Space.TopLeft.Y;
		var borderY = Space.TopLeft.Y + diffY / 2;
		var topSpace = Space with { BottomRight = Space.BottomRight with { Y = borderY } };
		var bottomSpace = Space with { TopLeft = Space.TopLeft with { Y = borderY } };

		Left = new Node(this, topSpace, _grid, _random);
		Right = new Node(this, bottomSpace, _grid, _random);
	}

	public void GenerateRoom()
	{
		// room can fill space up to one empty row and one empty column
		var maxDimensionX = Space.BottomRight.X - Space.TopLeft.X + 1;
		var maxDimensionY = Space.BottomRight.Y - Space.TopLeft.Y + 1;
		var dimensionX = _random.Next(2, maxDimensionX);
		var dimensionY = _random.Next(2, maxDimensionY);

		var roomTiles = RoomFactory.GetEmpty(dimensionX, dimensionY);

		// room cannot overlap with right / bottom borders
		var tileStartX = _random.Next(Space.TopLeft.X, Space.BottomRight.X - dimensionX + 1);
		var tileStartY = _random.Next(Space.TopLeft.Y, Space.BottomRight.Y - dimensionY + 1);

		for (var x = 0; x < dimensionX; x++)
		{
			for (var y = 0; y < dimensionY; y++)
			{
				var gridX = tileStartX + x;
				var gridY = tileStartY + y;
				_grid[gridX, gridY] = roomTiles[x, y];
			}
		}
	}
}
