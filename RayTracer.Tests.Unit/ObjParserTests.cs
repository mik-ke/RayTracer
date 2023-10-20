using RayTracer.Models;
using RayTracer.Parsers;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class ObjTests
{
	[Fact]
	public async Task LoadFromStringAsync_ShouldNotThrow_WhenObjStringGibberish()
	{
		// Arrange
		const string objData =
@"
There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.
";
		Obj obj = new();

		// Act
		// Assert
		await obj.LoadFromStringAsync(objData);
		Assert.Empty(obj.Vertices);
		Assert.Empty(obj.Group);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldSetVerticesCorrectly()
	{
		// Arrange
		const string objData =
@"
v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0 0
v 1 1 0
";
		Obj obj = new();

		Point expectedVertice1 = new(-1, 1, 0);
		Point expectedVertice2 = new(-1, 0.5, 0);
		Point expectedVertice3 = new(1, 0, 0);
		Point expectedVertice4 = new(1, 1, 0);

		// Act
		await obj.LoadFromStringAsync(objData);

		// Assert
		Assert.Equal(expectedVertice1, obj.Vertices[0]);
		Assert.Equal(expectedVertice2, obj.Vertices[1]);
		Assert.Equal(expectedVertice3, obj.Vertices[2]);
		Assert.Equal(expectedVertice4, obj.Vertices[3]);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldAddTrianglesToGroup_WhenFacesInObjString()
	{
		// Arrange
		const string objData =
@"
v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0

f 1 2 3
f 1 3 4
";
		Obj obj = new();

		// Act
		await obj.LoadFromStringAsync(objData);

		// Assert
		var childOne = (Triangle)obj.Group[0];
		var childTwo = (Triangle)obj.Group[1];
		Assert.Equal(obj.Vertices[0], childOne.Point1);
		Assert.Equal(obj.Vertices[1], childOne.Point2);
		Assert.Equal(obj.Vertices[2], childOne.Point3);
		Assert.Equal(obj.Vertices[0], childTwo.Point1);
		Assert.Equal(obj.Vertices[2], childTwo.Point2);
		Assert.Equal(obj.Vertices[3], childTwo.Point3);
	}
}
