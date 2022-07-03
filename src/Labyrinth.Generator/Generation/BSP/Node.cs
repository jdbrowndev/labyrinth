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
	public bool IsPartitionedByX { get; private set; }
	public bool IsPartitionedByY { get; private set; }
	public Node Left { get; private set; }
	public Node Right { get; private set; }
	public bool IsLeaf => Left == null && Right == null;
	public Rectangle Space { get; private set; }
	public Rectangle Room { get; private set; }

	public void PartitionByX()
	{
		if (Left != null || Right != null)
			throw new ArgumentException("Node already partitioned");

		var diffX = Space.BottomRight.X - Space.TopLeft.X;
		var borderX = Space.TopLeft.X + diffX / 2;
		var leftSpace = Space with { BottomRight = Space.BottomRight with { X = borderX } };
		var rightSpace = Space with { TopLeft = Space.TopLeft with { X = borderX } };

		Left = new Node(this, leftSpace, _grid, _random);
		Right = new Node(this, rightSpace, _grid, _random);
		IsPartitionedByX = true;
	}

	public void PartitionByY()
	{
		if (Left != null || Right != null)
			throw new ArgumentException("Node already partitioned");

		var diffY = Space.BottomRight.Y - Space.TopLeft.Y;
		var borderY = Space.TopLeft.Y + diffY / 2;
		var topSpace = Space with { BottomRight = Space.BottomRight with { Y = borderY } };
		var bottomSpace = Space with { TopLeft = Space.TopLeft with { Y = borderY } };

		Left = new Node(this, topSpace, _grid, _random);
		Right = new Node(this, bottomSpace, _grid, _random);
		IsPartitionedByY = true;
	}

	public void GenerateRoom()
	{
		if (Room != null)
			throw new ArgumentException("Room already generated");

		// room can fill space but must leave at least two empty rows and two empty columns
		var maxDimensionX = Space.BottomRight.X - Space.TopLeft.X - 1;
		var maxDimensionY = Space.BottomRight.Y - Space.TopLeft.Y - 1;
		var dimensionX = _random.Next(2, maxDimensionX);
		var dimensionY = _random.Next(2, maxDimensionY);
		var roomTiles = RoomFactory.GetEmpty(dimensionX, dimensionY);

		var tileStartX = _random.Next(Space.TopLeft.X, Space.BottomRight.X - dimensionX - 1);
		var tileStartY = _random.Next(Space.TopLeft.Y, Space.BottomRight.Y - dimensionY - 1);

		Position roomTopLeft = null;
		Position roomBottomRight = null;
		for (var x = 0; x < dimensionX; x++)
		{
			for (var y = 0; y < dimensionY; y++)
			{
				var gridX = tileStartX + x;
				var gridY = tileStartY + y;
				_grid[gridX, gridY] = roomTiles[x, y];

				if (x == 0 && y == 0)
					roomTopLeft = new Position(gridX, gridY);

				if (x == dimensionX - 1 && y == dimensionY - 1)
					roomBottomRight = new Position(gridX, gridY);
			}
		}

		// save room positions for future use
		Room = new Rectangle(roomTopLeft, roomBottomRight);
	}

	public void ConnectChildRooms()
	{
		if (IsLeaf)
			throw new ArgumentException("Cannot connect child rooms on a leaf node");

		if (IsPartitionedByX)
			ConnectChildRoomsPartitionedByX();
		else
			ConnectChildRoomsPartitionedByY();
	}

	private void ConnectChildRoomsPartitionedByX()
	{
		// todo implement
	}

	private void ConnectChildRoomsPartitionedByY()
	{
		var topRoom = GetRoomsRecursive(Left).MaxBy(x => x.BottomRight.Y);
		var bottomRoom = GetRoomsRecursive(Right).MinBy(x => x.TopLeft.Y);

		var topDoor = new Position(_random.Next(topRoom.BottomLeft.X, topRoom.BottomRight.X + 1), topRoom.BottomRight.Y);
		_grid[topDoor.X, topDoor.Y].Bottom = TileSide.Door;

		var bottomDoor = new Position(_random.Next(bottomRoom.TopLeft.X, bottomRoom.TopRight.X + 1), bottomRoom.TopLeft.Y);
		_grid[bottomDoor.X, bottomDoor.Y].Top = TileSide.Door;

		var targetPosition = bottomDoor with { Y = bottomDoor.Y - 1 };

		var prevPosition = topDoor with { Y = topDoor.Y + 1 };
		var prevPositionDown = true;
		var prevPositionRight = false;
		var prevPositionLeft = false;

		var prevTile = new Tile { Top = TileSide.Door };
		_grid[prevPosition.X, prevPosition.Y] = prevTile;

		while (prevPosition != targetPosition)
		{
			var moveX = prevPosition.X != targetPosition.X;
			var moveY = prevPosition.Y != targetPosition.Y;

			if (moveX && moveY)
			{
				moveX = _random.Next(2) == 0;
			}

			Position nextPosition;
			Tile nextTile;
			if (moveX)
			{
				if (targetPosition.X - prevPosition.X > 0)
				{
					// move right
					nextPosition = prevPosition with { X = prevPosition.X + 1 };

					// fix prevTile
					prevTile.Bottom = TileSide.Wall;
					if (prevPositionRight)
						prevTile.Top = TileSide.Wall;
					if (prevPositionDown)
						prevTile.Left = TileSide.Wall;

					// set next tile
					if (nextPosition == targetPosition)
					{
						nextTile = new Tile
						{
							Top = TileSide.Wall,
							Right = TileSide.Wall,
							Bottom = TileSide.Door
						};
					}
					else
					{
						nextTile = new Tile { Top = TileSide.Wall };
					}

					prevPositionRight = true;
					prevPositionLeft = false;
				}
				else
				{
					// move left
					nextPosition = prevPosition with { X = prevPosition.X - 1 };

					// fix prevTile
					prevTile.Bottom = TileSide.Wall;
					if (prevPositionLeft)
						prevTile.Top = TileSide.Wall;
					if (prevPositionDown)
						prevTile.Right = TileSide.Wall;

					// set next tile
					if (nextPosition == targetPosition)
					{
						nextTile = new Tile
						{
							Top = TileSide.Wall,
							Left = TileSide.Wall,
							Bottom = TileSide.Door
						};
					}
					else
					{
						nextTile = new Tile { Top = TileSide.Wall };
					}

					prevPositionRight = false;
					prevPositionLeft = true;
				}

				prevPositionDown = false;
			}
			else // moveY
			{
				// move down
				nextPosition = prevPosition with { Y = prevPosition.Y + 1 };

				// fix prevTile
				if (prevPositionDown || prevPositionLeft)
					prevTile.Left = TileSide.Wall;
				if (prevPositionDown || prevPositionRight)
					prevTile.Right = TileSide.Wall;

				// set next tile
				if (nextPosition == targetPosition)
				{
					nextTile = new Tile
					{
						Left = TileSide.Wall,
						Right = TileSide.Wall,
						Bottom = TileSide.Door
					};
				}
				else
				{
					nextTile = new Tile();
				}

				prevPositionDown = true;
				prevPositionRight = false;
				prevPositionLeft = false;
			}

			_grid[nextPosition.X, nextPosition.Y] = nextTile;
			prevPosition = nextPosition;
			prevTile = nextTile;
		}
	}

	private IEnumerable<Rectangle> GetRoomsRecursive(Node node)
	{
		if (node.IsLeaf)
		{
			return new[] { node.Room };
		}

		var leftRooms = GetRoomsRecursive(node.Left);
		var rightRooms = GetRoomsRecursive(node.Right);
		return leftRooms.Concat(rightRooms).ToList();
	}
}
