using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class GroupTests
{
	[Fact]
	public void Constructor_ShouldInitializeEmptyCollection()
	{
		// Arrange
		// Act
		Group group = new();

		// Assert
		Assert.Empty(group);
	}

	[Fact]
	public void AddChild_ShouldSetParent_WhenShapeGiven()
	{
		// Arrange
		Group group = new();
		Shape shape = new TestShape();

		// Act
		group.AddChild(shape);

		// Assert
		Assert.Equal(group, shape.Parent);
	}

	[Fact]
	public void AddChild_ShouldAddShapeToCollection()
	{
		// Arrange
		Group group = new();
		Shape shape = new TestShape();
		const int expectedCount = 1;

		// Act
		group.AddChild(shape);

		// Assert
		Assert.Equal(expectedCount, group.Count);
		Assert.Contains(shape, group);
	}

	[Fact]
	public void Interesect_ShouldBeEmpty_WhenGroupEmpty()
	{
		// Arrange
		Group group = new();
		Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenGroupNonEmpty()
	{
		// Arrange
		Group group = new();
		Sphere sphere1 = new();

		Matrix transform2 = Matrix.Translation(0, 0, -3);
		Sphere sphere2 = new(transform2);

		Matrix transform3 = Matrix.Translation(5, 0, 0);
		Sphere sphere3 = new(transform3);

		group.AddChild(sphere1);
		group.AddChild(sphere2);
		group.AddChild(sphere3);

		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		const int expectedLength = 4;

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
		Assert.Equal(sphere2, result[0].Object);
		Assert.Equal(sphere2, result[1].Object);
		Assert.Equal(sphere1, result[2].Object);
		Assert.Equal(sphere1, result[3].Object);
	}

	[Fact]
	public void Intersect_ShouldBeCorrect_WhenGroupAndShapeTransformed()
	{
		// Arrange
		Matrix groupTransform = Matrix.Scaling(2, 2, 2);
		Group group = new(groupTransform);
		Matrix childTransform = Matrix.Translation(5, 0, 0);
		Sphere child = new(childTransform);
		group.AddChild(child);

		Ray ray = new(new Point(10, 0, -10), new Vector(0, 0, 1));
		const int expectedLength = 2;

		// Act
		var result = group.Intersect(ray);

		// Assert
		Assert.Equal(expectedLength, result.Length);
	}
}
