namespace Labyrinth.Generator.Models;

public record Rectangle
{
	public Rectangle(Position topLeft, Position bottomRight)
	{
		Validate(topLeft, bottomRight);

		_topLeft = topLeft;
		_bottomRight = bottomRight;
	}

	private Position _topLeft;
	public Position TopLeft
	{
		get => _topLeft;
		
		init
		{
			Validate(value, _bottomRight);
			_topLeft = value;
		}
	}

	public Position TopRight
	{
		get => new Position(BottomRight.X, TopLeft.Y);
	}

	private Position _bottomRight;
	public Position BottomRight
	{
		get => _bottomRight;

		init
		{
			Validate(_topLeft, value);
			_bottomRight = value;
		}
	}

	public Position BottomLeft
	{
		get => new Position(TopLeft.X, BottomRight.Y);
	}

	private void Validate(Position topLeft, Position bottomRight)
	{
		if (topLeft == null)
			throw new ArgumentException("Rectangle top left position is required");

		if (bottomRight == null)
			throw new ArgumentException("Rectangle bottom right position is required");

		if (bottomRight.X < topLeft.X || bottomRight.Y < topLeft.Y)
			throw new ArgumentException("Rectangle has invalid dimensions");
	}

	// todo is this method needed?
	public bool Overlaps(Rectangle other)
	{
		if (TopLeft.X > other.BottomRight.X || other.TopLeft.X > BottomRight.X)
		{
			return false;
		}

		if (BottomRight.Y > other.TopLeft.Y || other.BottomRight.Y > TopLeft.Y)
		{
			return false;
		}

		return true;
	}
}