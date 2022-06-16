namespace Labyrinth.Generator.Models;

public class Room
{
	// todo need to decide how to store the room's rectangle within the overall grid

	private readonly Tile[,] _tiles;

	public Room( Tile[,] tiles )
	{
		_tiles = tiles;
	}

	public Tile GetTile(Position position)
	{
		return _tiles[position.X, position.Y];
	}
}
