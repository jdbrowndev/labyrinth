namespace Labyrinth.Generator.Models;

public record Rectangle
{
	public Rectangle(Position topLeft, Position bottomRight)
	{
		_topLeft = topLeft;
		_bottomRight = bottomRight;

		Validate();
	}

	private Position _topLeft;
	public Position TopLeft
	{
		get => _topLeft;
		
		init
		{
			_topLeft = value;
			Validate();
		}
	}

	private Position _bottomRight;
	public Position BottomRight
	{
		get => _bottomRight;

		init
		{
			_bottomRight = value;
			Validate();
		}
	}

	private void Validate()
	{
		if (_bottomRight.X < _topLeft.X || _bottomRight.Y < _topLeft.Y)
			throw new ArgumentException("Rectangle has invalid dimensions");
	}

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