namespace Labyrinth.Generator.Models;

public class Dungeon
{
	private readonly Tile[,] _tiles;

	public Dungeon(Tile[,] tiles)
	{
		_tiles = tiles;
	}

	public int DimensionX => _tiles.GetLength(0);
	public int DimensionY => _tiles.GetLength(1);

	public Tile GetTile(Position position)
	{
		return _tiles[position.X, position.Y];
	}
}
