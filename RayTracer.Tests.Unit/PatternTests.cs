using RayTracer.Models;
using RayTracer.Patterns;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class PatternTests
{
	[Fact]
	public void Constructor_ShouldSetIdentityMatrix_WhenTransformNotGiven()
	{
		// Arrange
		Matrix expected = Matrix.Identity(4);

		// Act
		Pattern pattern = new TestPattern();

		// Assert
		Assert.Equal(expected, pattern.Transform);
	}

	[Fact]
	public void Constructor_ShouldSetTransform_WhenGiven()
	{
		// Arrange
		Matrix transform = Matrix.Translation(1, 2, 3);

		// Act
		Pattern pattern = new TestPattern(transform);

		// Assert
		Assert.Equal(transform, pattern.Transform);
	}

	[Fact]
	public void PatternAtShape_ShouldWork_WhenShapeTransformed()
	{
		// Arrange
		Matrix transform = Matrix.Scaling(2, 2, 2);
		Shape shape = new Sphere(transform);
		Point point = new(2, 3, 4);
		Pattern pattern = new TestPattern();
		Color expected = new Color(1, 1.5, 2);

		// Act
		var actual = pattern.PatternAtShape(shape, point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void PatternAtShape_ShouldWork_WhenPatternTransformed()
	{
		// Arrange
		Shape shape = new Sphere();
		Point point = new(2, 3, 4);
		Matrix transform = Matrix.Scaling(2, 2, 2);
		Pattern pattern = new TestPattern(transform);
		Color expected = new(1, 1.5, 2);

		// Act
		var actual = pattern.PatternAtShape(shape, point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void PatternAtShape_ShouldWork_WhenShapeAndPatternTransformed()
	{
		// Arrange
		Matrix shapeTransform = Matrix.Scaling(2, 2, 2);
		Shape shape = new Sphere(shapeTransform);
		Point point = new(2.5, 3, 3.5);
		Matrix patternTransform = Matrix.Translation(0.5, 1, 1.5);
		Pattern pattern = new TestPattern(patternTransform);
		Color expected = new(0.75, 0.5, 0.25);

		// Act
		var actual = pattern.PatternAtShape(shape, point);

		// Assert
		Assert.Equal(expected, actual);
	}
}
