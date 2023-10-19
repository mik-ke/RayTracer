using RayTracer.Models;
using RayTracer.Parsers;

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
	}

	[Fact]
	public async void LoadFromStringAsync_ShouldSetVerticesCorrectly()
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
}
