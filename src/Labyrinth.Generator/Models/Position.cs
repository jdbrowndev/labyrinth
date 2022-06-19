namespace Labyrinth.Generator.Models;

public record Position
{
	public Position(int x, int y)
	{
		Validate(x, y);

		_x = x;
		_y = y;
	}

	private int _x;
	public int X
	{
		get => _x;
		init
		{
			Validate(value, _y);
			_x = value;
		}
	}

	private int _y;
	public int Y
	{
		get => _y;
		init
		{
			Validate(_x, value);
			_y = value;
		}
	}

	private void Validate(int x, int y)
	{
		if (x < 0 || y < 0)
			throw new ArgumentException("Position cannot contain negative values");
	}
}
