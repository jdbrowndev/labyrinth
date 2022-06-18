namespace Labyrinth.Generator.Models;

public class Tile
{
	public Tile(TileSide top, TileSide right, TileSide bottom, TileSide left)
	{
		Top = top;
		Right = right;
		Bottom = bottom;
		Left = left;
	}

	public TileSide Top { get; private set; }
	public TileSide Right { get; private set; }
	public TileSide Bottom { get; private set; }
	public TileSide Left { get; private set; }
}
