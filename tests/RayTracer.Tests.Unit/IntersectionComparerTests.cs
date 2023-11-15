using RayTracer.Comparers;
using RayTracer.Models;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class IntersectionComparerTests
{
	[Fact]
	public void Compare_ShouldReturnZero_WhenSameTValue()
	{
		// Arrange
		const int expected = 0;
		IntersectionComparer comparer = new();
		Sphere sphere = new();
		Intersection intersection1 = new(1, sphere);
		Intersection intersection2 = new(1, sphere);

		// Act
		var actual = comparer.Compare(intersection1, intersection2);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Compare_ShouldReturnOne_WhenFirstTValueLarger()
	{
		// Arrange
		const int expected = 1;
		IntersectionComparer comparer = new();
		Sphere sphere = new();
		Intersection intersection1 = new(2, sphere);
		Intersection intersection2 = new(1, sphere);

		// Act
		var actual = comparer.Compare(intersection1, intersection2);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Compare_ShouldReturnMinusOne_WhenFirstTValueSmaller()
	{
		// Arrange
		const int expected = -1;
		IntersectionComparer comparer = new();
		Sphere sphere = new();
		Intersection intersection1 = new(1, sphere);
		Intersection intersection2 = new(2, sphere);

		// Act
		var actual = comparer.Compare(intersection1, intersection2);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Compare_ShouldReturnZero_WhenBothNull()
	{
		// Arrange
		const int expected = 0;
		IntersectionComparer comparer = new();

		// Act
		var actual = comparer.Compare(null, null);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Compare_ShouldReturnMinusOne_WhenFirstNull()
	{
		// Arrange
		const int expected = -1;
		IntersectionComparer comparer = new();
		Sphere sphere = new();
		Intersection intersection = new(1, sphere);

		// Act
		var actual = comparer.Compare(null, intersection);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Compare_ShouldReturnOne_WhenSecondNull()
	{
		// Arrange
		const int expected = 1;
		IntersectionComparer comparer = new();
		Sphere sphere = new();
		Intersection intersection = new(1, sphere);

		// Act
		var actual = comparer.Compare(intersection, null);

		// Assert
		Assert.Equal(expected, actual);
	}
}
