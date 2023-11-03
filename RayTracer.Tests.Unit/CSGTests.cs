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
}
