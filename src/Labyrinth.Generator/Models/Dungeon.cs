namespace Labyrinth.Generator.Models;

public class Dungeon
{
	private readonly int _sizeX;
	private readonly int _sizeY;
	private readonly Room[,] _grid;

	public Dungeon(int sizeX, int sizeY)
	{
		_sizeX = sizeX;
		_sizeY = sizeY;
		_grid = new Room[sizeX, sizeY];
	}

	public Room GetRoom(Position position)
	{
		return _grid[position.X, position.Y];
	}

	public void AddRoom(Room room)
	{
		// todo - ensure no overlaps, then add the room in
	}
}
