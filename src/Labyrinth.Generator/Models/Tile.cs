namespace Labyrinth.Generator.Models;

public record Tile
{
	public TileSide Top { get; set; }
	public TileSide Right { get; set; }
	public TileSide Bottom { get; set; }
	public TileSide Left { get; set; }
}