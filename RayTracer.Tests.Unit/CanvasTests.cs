using RayTracer.Models;
using Xunit;
namespace RayTracer.Tests.Unit;

public class CanvasTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectProperties_WhenCreated()
	{
		// Arrange
		const int width = 10;
		const int height = 20;

		// Act
		Canvas canvas = new(width, height);

		// Assert
		Assert.Equal(width, canvas.Width);
		Assert.Equal(height, canvas.Height);
	}

	[Fact]
	public void Constructor_ShouldInitializeBlackCanvas_WhenCreated()
	{
		// Arrange
		Color expected = Color.Black;

		// Act
		Canvas canvas = new(10, 20);

		// Assert
		Assert.All(canvas, color => Assert.Equal(expected, color));
	}

	[Fact]
	public void WritePixel_ShouldWork_WhenCalled()
	{
		// Arrange
		Canvas canvas = new(10, 20);
		Color color = new(1, 0, 0);

		// Act
		canvas.WritePixel(2, 3, color);

		// Assert
		Assert.Equal(color, canvas.PixelAt(2, 3));
	}

	[Fact]
	public void Indexer_ShouldWork_WhenCalled()
	{
		// Arrange
		Canvas canvas = new(10, 20);
		Color color = new(1, 0, 0);

		// Act
		canvas[2, 3] = color;

		// Assert
		Assert.Equal(color, canvas[2, 3]);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void WritePixel_ShouldThrowOutOfRangeException_WhenOutOfBounds(int x, int y)
	{
		// Arrange
		const int width = 10;
		const int height = 20;
		Canvas canvas = new(width, height);

		// Act
		Exception e = Record.Exception(() =>
		{
			canvas.WritePixel(x, y, Color.Black);
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(10, 0)]
	[InlineData(0, -1)]
	[InlineData(0, 20)]
	public void PixelAt_ShouldThrowOutOfRangeException_WhenOutOfBounds(int x, int y)
	{
		// Arrange
		const int width = 10;
		const int height = 20;
		Canvas canvas = new(width, height);

		// Act
		Exception e = Record.Exception(() =>
		{
			canvas.PixelAt(x, y);
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(e);
	}
}
