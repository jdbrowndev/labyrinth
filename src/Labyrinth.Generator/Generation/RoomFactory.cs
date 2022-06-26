using Labyrinth.Generator.Models;

namespace Labyrinth.Generator.Generation;

public static class RoomFactory
{
	public static Tile[,] GetEmpty(int dimensionX, int dimensionY)
	{
		var tiles = new Tile[dimensionX, dimensionY];

		for (var x = 0; x < dimensionX; x++)
		{
			for (var y = 0; y < dimensionY; y++)
			{
				var top = y == 0 ? TileSide.Wall : TileSide.Empty;
				var right = x == dimensionX - 1 ? TileSide.Wall : TileSide.Empty;
				var bottom = y == dimensionY - 1 ? TileSide.Wall : TileSide.Empty;
				var left = x == 0 ? TileSide.Wall : TileSide.Empty;

				var tile = new Tile
				{
					Top = top,
					Right = right,
					Bottom = bottom,
					Left = left
				};
				tiles[x, y] = tile;
			}
		}

		return tiles;
	}
}
