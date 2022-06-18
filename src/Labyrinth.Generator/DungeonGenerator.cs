using Labyrinth.Generator.Generation.BSP;
using Labyrinth.Generator.Models;

namespace Labyrinth.Generator;

public class DungeonGenerator
{
	public Dungeon Generate(int dimensionX = 100, int dimensionY = 100, int partitioning = 5, int? seed = null)
	{
		var random = seed.HasValue ? new Random(seed.Value) : new Random();
		var tree = new BinarySpanningTree(dimensionX, dimensionY, partitioning, random);
		tree.Generate();
		
		var dungeon = new Dungeon(tree.Grid);
		return dungeon;
	}
}
