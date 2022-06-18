using Labyrinth.CLI;
using Labyrinth.Generator;

var generator = new DungeonGenerator();
var dungeon = generator.Generate();

var printer = new DungeonPrinter();
printer.Print(dungeon, "output.txt");
