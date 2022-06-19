using Labyrinth.Generator.Models;
using Xunit;

namespace Labyrinth.Generator.Tests.Models;

public class The_Position_Record
{
	[Fact]
	public void Should_Be_Immutable()
	{
		var position1 = new Position(0, 0);
		var position2 = position1 with { Y = 1 };

		Assert.Equal(new Position(0, 0), position1);
		Assert.Equal(new Position(0, 1), position2);
	}

	[Theory]
	[InlineData(0, 0)]
	[InlineData(1, 0)]
	[InlineData(0, 1)]
	public void Should_Accept_Valid_Coordinates(int x, int y)
	{
		var position1 = new Position(x, y);
		var position2 = position1 with { X = x + 1 };
		_ = position2 with { Y = y + 1 };
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(0, -1)]
	public void Should_Throw_Exception_If_Coordinates_Invalid(int x, int y)
	{
		Assert.Throws<ArgumentException>(() => new Position(x, y));

		var validPosition = new Position(0, 0);
		Assert.Throws<ArgumentException>(() =>
		{
			_ = validPosition with { X = x };
			_ = validPosition with { Y = y };
		});
	}
}
