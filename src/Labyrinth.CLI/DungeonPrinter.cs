using Labyrinth.Generator.Models;

namespace Labyrinth.CLI;

public class DungeonPrinter
{
	// Prints one symbol per tile
	//
	// Symbols:
	// top/bottom only -
	// left/right only |
	// corner +
	// door D
	public void Print(Dungeon dungeon, string outputPath)
	{
		using var fs = new FileStream(outputPath, FileMode.Create);
		using var writer = new StreamWriter(fs);

		Tile current;
		for (var y = 0; y < dungeon.DimensionY; y++)
		{
			for (var x = 0; x < dungeon.DimensionX; x++)
			{
				current = dungeon.GetTile(new Position(x, y));
				if (current == null)
					writer.Write(' ');
				else if (HasWall(current.Left, current.Right) && !HasWall(current.Top, current.Bottom))
					writer.Write('|');
				else if (!HasWall(current.Left, current.Right) && HasWall(current.Top, current.Bottom))
					writer.Write('-');
				else if (HasWall(current.Left, current.Right) && HasWall(current.Top, current.Bottom))
					writer.Write('+');
				else if (HasDoor(current.Top, current.Right, current.Bottom, current.Left))
					writer.Write('D');
				else
					writer.Write(' ');
			}
			writer.WriteLine();
		}
	}

	private bool HasWall(params TileSide[] sides)
	{
		return sides.Any(x => x == TileSide.Wall);
	}

	private bool HasDoor(params TileSide[] sides)
	{
		return sides.Any(x => x == TileSide.Door);
	}
}
