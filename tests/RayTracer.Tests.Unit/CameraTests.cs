using RayTracer.Extensions;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class CameraTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		const int horizontalSize = 160;
		const int verticalSize = 120;
		const double fieldOfView = Math.PI / 2;
		Matrix expectedTransform = Matrix.Identity(4);

		// Act
		var result = new Camera(horizontalSize, verticalSize, fieldOfView);

		// Assert
		Assert.Equal(horizontalSize, result.HorizontalSize);
		Assert.Equal(verticalSize, result.VerticalSize);
		Assert.Equal(fieldOfView, result.FieldOfView);
		Assert.Equal(expectedTransform, result.Transform);
	}

	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenTransformGiven()
	{
		// Arrange
		const int horizontalSize = 160;
		const int verticalSize = 120;
		const double fieldOfView = Math.PI / 2;
		Matrix transform = Matrix.RotationY(Math.PI / 4) * Matrix.Translation(0, -2, 5);

		// Act
		var result = new Camera(horizontalSize, verticalSize, fieldOfView, transform);

		// Assert
		Assert.Equal(horizontalSize, result.HorizontalSize);
		Assert.Equal(verticalSize, result.VerticalSize);
		Assert.Equal(fieldOfView, result.FieldOfView);
		Assert.Equal(transform, result.Transform);
	}

	[Fact]
	public void PixelSize_ShouldBeCorrect_WhenCanvasHorizontal()
	{
		// Arrange
		Camera camera = new(200, 125, Math.PI / 2);
		const double expected = 0.01;

		// Act
		var actual = camera.PixelSize;

		// Assert
		Assert.True(expected.IsEqualTo(actual));
	}

	[Fact]
	public void PixelSize_ShouldBeCorrect_WhenCanvasVertical()
	{
		// Arrange
		Camera camera = new(125, 200, Math.PI / 2);
		const double expected = 0.01;

		// Act
		var actual = camera.PixelSize;

		// Assert
		Assert.True(expected.IsEqualTo(actual));
	}

	[Fact]
	public void RayForPixel_ShouldWork_WhenGoingThroughCanvasCenter()
	{
		// Arrange
		Camera camera = new(201, 101, Math.PI / 2);
		Point expectedOrigin = new(0, 0, 0);
		Vector expectedDirection = new(0, 0, -1);

		// Act
		var result = camera.RayForPixel(100, 50);

		// Assert
		Assert.Equal(expectedOrigin, result.Origin);
		Assert.Equal(expectedDirection, result.Direction);
	}

	[Fact]
	public void RayForPixel_ShouldWork_WhenGoingThroughCanvasCorner()
	{
		// Arrange
		Camera camera = new(201, 101, Math.PI / 2);
		Point expectedOrigin = new(0, 0, 0);
		Vector expectedDirection = new(0.66519, 0.33259, -0.66851);

		// Act
		var result = camera.RayForPixel(0, 0);

		// Assert
		Assert.Equal(expectedOrigin, result.Origin);
		Assert.Equal(expectedDirection, result.Direction);
	}

	[Fact]
	public void RayForPixel_ShouldWork_WhenCameraTransformed()
	{
		// Arrange
		Matrix transform = Matrix.RotationY(Math.PI / 4) * Matrix.Translation(0, -2, 5);
		Camera camera = new(201, 101, Math.PI / 2, transform);
		Point expectedOrigin = new(0, 2, -5);
		Vector expectedDirection = new(Math.Sqrt(2) / 2, 0, -Math.Sqrt(2) / 2);

		// Act
		var result = camera.RayForPixel(100, 50);

		// Assert
		Assert.Equal(expectedOrigin, result.Origin);
		Assert.Equal(expectedDirection, result.Direction);
	}

	// nonrigorous test to show how render ought to work
	[Fact]
	public void Render_ShouldRenderWorldCorrectly()
	{
		// Arrange
		World world = WorldTests.DefaultTestWorld();
		Point from = new(0, 0, -5);
		Point to = new(0, 0, 0);
		Vector up = new(0, 1, 0);
		Matrix transform = Matrix.View(from, to, up);
		Camera camera = new(11, 11, Math.PI / 2, transform);
		Color expectedColor = new(0.38066, 0.47583, 0.2855);

		// Act
		var result = camera.Render(world);

		// Assert
		Assert.Equal(expectedColor, result.PixelAt(5, 5));
	}
}
