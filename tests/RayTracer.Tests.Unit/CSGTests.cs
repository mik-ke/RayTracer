using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class CSGTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly()
	{
		// Arrange
		Sphere left = new();
		Cube right = new();
		Operation operation = Operation.Union;

		// Act
		var result = new CSG(operation, left, right);

		// Assert
		Assert.Equal(left, result.Left);
		Assert.Equal(right, result.Right);
		Assert.Equal(operation, result.Operation);
	}

	[Theory]
	[InlineData(true, true, true, false)]
	[InlineData(true, true, false, true)]
	[InlineData(true, false, true, false)]
	[InlineData(true, false, false, true)]
	[InlineData(false, true, true, false)]
	[InlineData(false, true, false, false)]
	[InlineData(false, false, true, true)]
	[InlineData(false, false, false, true)]
	public void IntersectionAllowed_ShouldReturnCorrectResult_WhenUnionOperation(
		bool leftHit, bool inLeft, bool inRight, bool expected)
	{
		// Arrange
        Operation operation = Operation.Union;

        // Act
        var result = CSG.IntersectionAllowed(operation, leftHit, inLeft, inRight);

        // Assert
        Assert.Equal(expected, result);
	}

	[Theory]
	[InlineData(true, true, true, true)]
	[InlineData(true, true, false, false)]
	[InlineData(true, false, true, true)]
	[InlineData(true, false, false, false)]
	[InlineData(false, true, true, true)]
	[InlineData(false, true, false, true)]
	[InlineData(false, false, true, false)]
	[InlineData(false, false, false, false)]
	public void IntersectionAllowed_ShouldReturnCorrectResult_WhenIntersectionOperation(
		bool leftHit, bool inLeft, bool inRight, bool expected)
	{
        // Arrange
        Operation operation = Operation.Intersection;

        // Act
        var result = CSG.IntersectionAllowed(operation, leftHit, inLeft, inRight);

        // Assert
        Assert.Equal(expected, result);
    }

	[Theory]
	[InlineData(true, true, true, false)]
	[InlineData(true, true, false, true)]
	[InlineData(true, false, true, false)]
	[InlineData(true, false, false, true)]
	[InlineData(false, true, true, true)]
	[InlineData(false, true, false, true)]
	[InlineData(false, false, true, false)]
	[InlineData(false, false, false, false)]
	public void IntersectionAllowed_ShouldReturnCorrectResult_WhenDifferenceOperation(
		bool leftHit, bool inLeft, bool inRight, bool expected)
	{
        // Arrange
        Operation operation = Operation.Difference;

        // Act
		var result = CSG.IntersectionAllowed(operation, leftHit, inLeft, inRight);

        // Assert
        Assert.Equal(expected, result);
    }

	[Fact]
	public void FilterIntersections_ShouldReturnCorrectResult_WhenUnionOperation()
	{
        // Arrange
        Sphere left = new();
        Cube right = new();
        Operation operation = Operation.Union;
        CSG csg = new(operation, left, right);
        Intersection intersectionOne = new(1, left);
        Intersection intersectionTwo = new(2, right);
        Intersection intersectionThree = new(3, left);
        Intersection intersectionFour = new(4, right);
        Intersections intersections = new(intersectionOne, intersectionTwo, intersectionThree, intersectionFour);
        Intersections expected = new(intersectionOne, intersectionFour);

        // Act
        var result = csg.FilterIntersections(intersections);

        // Assert
        Assert.Equal(expected, result);
    }

	[Fact]
	public void FilterIntersections_ShouldReturnCorrectResult_WhenIntersectionOperation()
	{
        // Arrange
        Sphere left = new();
        Cube right = new();
        Operation operation = Operation.Intersection;
        CSG csg = new(operation, left, right);
        Intersection intersectionOne = new(1, left);
        Intersection intersectionTwo = new(2, right);
        Intersection intersectionThree = new(3, left);
        Intersection intersectionFour = new(4, right);
        Intersections intersections = new(intersectionOne, intersectionTwo, intersectionThree, intersectionFour);
        Intersections expected = new(intersectionTwo, intersectionThree);

        // Act
        var result = csg.FilterIntersections(intersections);

        // Assert
        Assert.Equal(expected, result);
    }

	[Fact]
	public void FilterIntersections_ShouldReturnCorrectResult_WhenDifferenceOperation()
	{
        // Arrange
        Sphere left = new();
        Cube right = new();
        Operation operation = Operation.Difference;
        CSG csg = new(operation, left, right);
        Intersection intersectionOne = new(1, left);
        Intersection intersectionTwo = new(2, right);
        Intersection intersectionThree = new(3, left);
        Intersection intersectionFour = new(4, right);
        Intersections intersections = new(intersectionOne, intersectionTwo, intersectionThree, intersectionFour);
        Intersections expected = new(intersectionOne, intersectionTwo);

        // Act
        var result = csg.FilterIntersections(intersections);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Intersect_ShouldReturnFalse_WhenRayMisses()
    {
        // Arrange
        CSG csg = new(Operation.Union, new Sphere(), new Cube());
        Ray ray = new(new Point(0, 2, -5), new Vector(0, 0, 1));

        // Act
        var result = csg.Intersect(ray);

        // Assert
        Assert.Equal(Intersections.Empty, result);
    }

    [Fact]
    public void Intersect_ShouldReturnTrue_WhenRayHits()
    {
        // Arrange
        Sphere sphereOne = new();
        Sphere sphereTwo = new(Matrix.Translation(0, 0, 0.5));
        CSG csg = new(Operation.Union, sphereOne, sphereTwo);
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));

        // Act
        var result = csg.Intersect(ray);

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Equal(4, result[0].T);
        Assert.Equal(sphereOne, result[0].Object);
        Assert.Equal(6.5, result[1].T);
        Assert.Equal(sphereTwo, result[1].Object);
    }

    [Fact]
    public void Intersect_ShouldNotTestChildren_WhenBoundingBoxMissed()
    {
        // Arrange
        TestShape left = new();
        TestShape right = new();
        CSG csg = new(Operation.Difference, left, right);
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 1, 0));

        // Act
        csg.Intersect(ray);

        // Assert
        Assert.Null(left.SavedLocalRay);
        Assert.Null(right.SavedLocalRay);
    }

    [Fact]
    public void Intersect_ShouldTestChildren_WhenBoundingBoxHit()
    {
        // Arrange
        TestShape left = new();
        TestShape right = new();
        CSG csg = new(Operation.Difference, left, right);
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));

        // Act
        csg.Intersect(ray);

        // Assert
        Assert.NotNull(left.SavedLocalRay);
        Assert.NotNull(right.SavedLocalRay);
    }

    [Fact]
    public void BoundsOf_ShouldReturnBoundingBoxThatContainsChildren()
    {
        // Arrange
        Sphere sphereOne = new();
        Sphere sphereTwo = new(Matrix.Translation(2, 3, 4));
        CSG csg = new(Operation.Difference, sphereOne, sphereTwo);

        // Act
        var result = csg.BoundsOf();

        // Assert
        Assert.Equal(new Point(-1, -1, -1), result.Minimum);
        Assert.Equal(new Point(3, 4, 5), result.Maximum);
    }

    [Fact]
    public void Divide_ShouldDivideChildren()
    {
        // Arrange
        Sphere sphereOne = new(Matrix.Translation(-1.5, 0, 0));
        Sphere sphereTwo = new(Matrix.Translation(1.5, 0, 0));
        Group left = new(new[] { sphereOne, sphereTwo });
        Sphere sphereThree = new(Matrix.Translation(0, 0, -1.5));
        Sphere sphereFour = new(Matrix.Translation(0, 0, 1.5));
        Group right = new(new[] { sphereThree, sphereFour });
        CSG csg = new(Operation.Difference, left, right);

        // Act
        csg.Divide(1);

        // Assert
        Assert.True(left[0] is Group);
        Assert.Equal(sphereOne, ((Group)left[0])[0]);
        Assert.True(left[1] is Group);
        Assert.Equal(sphereTwo, ((Group)left[1])[0]);
        Assert.True(right[0] is Group);
        Assert.Equal(sphereThree, ((Group)right[0])[0]);
        Assert.True(right[1] is Group);
        Assert.Equal(sphereFour, ((Group)right[1])[0]);
    }
}
