using RayTracer.Shapes;
using Xunit;

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
}
