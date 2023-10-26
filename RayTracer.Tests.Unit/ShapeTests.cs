using RayTracer.Extensions;
using RayTracer.Shapes;
using RayTracer.Models;

namespace RayTracer.Tests.Unit;

public class ShapeTests
{
	[Fact]
	public void Parent_ShouldBeNull_WhenInitialized()
	{
		// Arrange
		// Act
		Shape shape = new TestShape();

		// Assert
		Assert.Null(shape.Parent);
	}

	[Fact]
	public void Transform_ShouldBeIdentityMatrix_WhenInitialized()
	{
		// Arrange
		Matrix expected = Matrix.Identity(4);

		// Act
		Shape shape = new TestShape();

		// Assert
		Assert.Equal(expected, shape.Transform);
	}

	[Fact]
	public void Transform_ShouldBeSetCorrectly()
	{
		// Arrange
		Matrix expected = Matrix.Translation(2, 3, 4);

		// Act
		Shape shape = new TestShape(expected);

		// Assert
		Assert.Equal(expected, shape.Transform);
	}

	[Fact]
	public void Material_ShouldBeDefaultMaterial_WhenInitialized()
	{
		// Arrange
		Material defaultMaterial = new();

		// Act
		Shape shape = new TestShape();

		// Assert
		Assert.Equal(defaultMaterial, shape.Material);
	}

	[Fact]
	public void Material_ShouldBeSet()
	{
		// Arrange
		Shape shape = new TestShape();
		Material material = new() { Ambient = 1.0 };

		// Act
		shape.Material = material;

		// Assert
		Assert.Equal(material, shape.Material);
	}

	[Fact]
	public void Intersect_ShouldCallLocalIntersectWithScaleTransformedRay()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Matrix transform = Matrix.Scaling(2, 2, 2);
		Shape shape = new TestShape(transform);
		Point expectedOrigin = new(0, 0, -2.5);
		Vector expectedDirection = new(0, 0, 0.5);

        // Act
        _ = shape.Intersect(ray);

        // Assert
        var localRay = ((TestShape)shape).SavedLocalRay!;
		Assert.Equal(expectedOrigin, localRay.Origin);
		Assert.Equal(expectedDirection, localRay.Direction);
	}

	[Fact]
	public void Intersect_ShouldCallLocalIntersectWithTranslateTransformedRay()
	{
		// Arrange
		Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
		Matrix transform = Matrix.Translation(5, 0, 0);
		Shape shape = new TestShape(transform);
		Point expectedOrigin = new(-5, 0, -5);
		Vector expectedDirection = new(0, 0, 1);

		// Act
		_ = shape.Intersect(ray);

		// Assert
		var localRay = ((TestShape)shape).SavedLocalRay!;
		Assert.Equal(expectedOrigin, localRay.Origin);
		Assert.Equal(expectedDirection, localRay.Direction);
	}

	[Fact]
	public void Normal_ShouldWork_WhenShapeTranslated()
	{
		// Arrange
		Matrix transform = Matrix.Translation(0, 1, 0);
		Shape shape = new TestShape(transform);
		Point point = new(0, 1.70711, -0.70711);
		Vector expected = new(0, 0.70711, -0.70711);

		// Act
		var actual = shape.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Normal_ShouldWork_WhenShapeTransformed()
	{
		// Arrange
		Matrix transform = Matrix.Identity(4).RotateZ(Math.PI / 5).Scale(1, 0.5, 1);
		Shape shape = new TestShape(transform);
        Point point = new(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
		Vector expected = new(0, 0.97014, -0.24254);

		// Act
		var actual = shape.Normal(point);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Normal_ShouldWork_WhenShapeIsChildOfGroup()
	{
		// Arrange
		Matrix transform1 = Matrix.RotationY(Math.PI / 2);
		Group group1 = new(transform1);

		Matrix transform2 = Matrix.Scaling(1, 2, 3);
		Group group2 = new(transform2);

		group1.AddChild(group2);

		Matrix transform3 = Matrix.Translation(5, 0, 0);
		Sphere sphere = new(transform3);

		group2.AddChild(sphere);

		Vector expected = new(0.2857, 0.4286, -0.8571);

		// Act
		var actual = sphere.Normal(new Point(1.7321, 1.1547, -5.5774));

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void WorldToObject_ShouldConvertWorldPointToObjectPoint()
	{
		// Arrange
		Matrix transform1 = Matrix.RotationY(Math.PI / 2);
		Group group1 = new(transform1);

		Matrix transform2 = Matrix.Scaling(2, 2, 2);
		Group group2 = new(transform2);

		group1.AddChild(group2);

		Matrix transform3 = Matrix.Translation(5, 0, 0);
		Sphere sphere = new(transform3);

		group2.AddChild(sphere);

		Point expected = new(0, 0, -1);

		// Act
		var actual = sphere.WorldToObject(new Point(-2, 0, -10));

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void NormalToWorld_ShouldConvertObjectSpaceNormalToWorldSpace()
	{
		// Arrange
		Matrix transform1 = Matrix.RotationY(Math.PI / 2);
		Group group1 = new(transform1);

		Matrix transform2 = Matrix.Scaling(1, 2, 3);
		Group group2 = new(transform2);

		group1.AddChild(group2);

		Matrix transform3 = Matrix.Translation(5, 0, 0);
		Sphere sphere = new(transform3);

		group2.AddChild(sphere);

		Vector expected = new(0.2857, 0.4286, -0.8571);

		// Act
		var actual = sphere.NormalToWorld(new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ParentSpaceBoundsOf_ShouldReturnCorrectBoundingBox()
	{
		// Arrange
		Matrix transform = Matrix.Scaling(0.5, 2, 4).Translate(1, -3, 5);
		Sphere shape = new(transform);
		Point expectedMinimum = new(0.5, -5, 1);
		Point expectedMaximum = new(1.5, -1, 9);

        // Act
		var result = shape.ParentSpaceBoundsOf();

        // Assert
		Assert.Equal(expectedMinimum, result.Minimum);
		Assert.Equal(expectedMaximum, result.Maximum);
    }
}
