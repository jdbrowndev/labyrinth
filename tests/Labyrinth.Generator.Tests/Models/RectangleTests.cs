using Labyrinth.Generator.Models;
using Xunit;

namespace Labyrinth.Generator.Tests.Models;

public class The_Rectangle_Record
{
	[Fact]
	public void Should_Be_Immutable()
	{
		var rect1 = new Rectangle(new Position(0, 0), new Position(1, 1));
		var rect2 = rect1 with { BottomRight = new Position(2, 2) };

		Assert.Equal(new Position(1, 1), rect1.BottomRight);
		Assert.Equal(new Position(2, 2), rect2.BottomRight);
	}

	[Theory]
	[InlineData(0, 0, 1, 0)]
	[InlineData(0, 0, 0, 1)]
	[InlineData(0, 0, 1, 1)]
	public void Should_Accept_Valid_Dimensions(int x1, int y1, int x2, int y2)
	{
		var rect1 = new Rectangle(new Position(x1, y1), new Position(x2, y2));
		var rect2 = rect1 with { BottomRight = new Position(x2 + 1, y2 + 1) };
		_ = rect2 with { TopLeft = new Position(x1 + 1, y1 + 1) };
	}

	[Theory]
	[InlineData(1, 0, 0, 0)]
	[InlineData(0, 1, 0, 0)]
	[InlineData(1, 1, 0, 0)]
	public void Should_Throw_Exception_If_Dimensions_Invalid(int x1, int y1, int x2, int y2)
	{
		Assert.Throws<ArgumentException>(() => new Rectangle(new Position(x1, y1), new Position(x2, y2)));

		var validRect = new Rectangle(new Position(0, 0), new Position(1, 1));
		Assert.Throws<ArgumentException>(() =>
		{
			var rect = validRect with { TopLeft = new Position(x1, y1) };
			_ = rect with { BottomRight = new Position(x2, y2) };
		});
	}
}
