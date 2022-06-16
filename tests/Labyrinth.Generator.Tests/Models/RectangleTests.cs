using Labyrinth.Generator.Models;
using Xunit;

namespace Labyrinth.Generator.Tests.Models;

// todo finish tests
public class The_Rectangle_Record
{
	[Fact]
	public void Should_Be_Immutable()
	{
		var rect1 = new Rectangle(new Position(0, 0), new Position(1, 1));
		var rect2 = rect1 with { BottomRight = new Position(2, 2) };

		Assert.Equal(rect1.BottomRight, new Position(1, 1));
		Assert.Equal(rect2.BottomRight, new Position(2, 2));
	}

	[Fact]
	public void Should_Throw_Exception_If_Dimensions_Invalid()
	{
		Assert.Throws<ArgumentException>(() => new Rectangle(new Position(1, 1), new Position(0, 0)));
	}
}
