using RayTracer.Models;
using RayTracer.Parsers;
using RayTracer.Shapes;

namespace RayTracer.Tests.Unit;

public class ObjTests
{
	[Fact]
	public async Task LoadFromStringAsync_ShouldNotThrow_WhenObjStringGibberish()
	{
		// Arrange
		const string objData =
			"""
			There was a young lady named Bright
			who traveled much faster than light.
			She set out one day
			in a relative way,
			and came back the previous night.
			""";
		Obj obj = new();

		// Act
		// Assert
		await obj.LoadFromStringAsync(objData);
		Assert.Empty(obj.Vertices);
		Assert.Empty(obj.DefaultGroup);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldSetVerticesCorrectly()
	{
		// Arrange
		const string objData =
			"""
			v -1 1 0
			v -1.0000 0.5000 0.0000
			v 1 0 0
			v 1 1 0
			""";
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
			"""
			v -1 1 0
			v -1 0 0
			v 1 0 0
			v 1 1 0

			f 1 2 3
			f 1 3 4
			""";
		Obj obj = new();

		// Act
		await obj.LoadFromStringAsync(objData);

		// Assert
		var childOne = (Triangle)obj.DefaultGroup[0];
		var childTwo = (Triangle)obj.DefaultGroup[1];
		Assert.Equal(obj.Vertices[0], childOne.Point1);
		Assert.Equal(obj.Vertices[1], childOne.Point2);
		Assert.Equal(obj.Vertices[2], childOne.Point3);
		Assert.Equal(obj.Vertices[0], childTwo.Point1);
		Assert.Equal(obj.Vertices[2], childTwo.Point2);
		Assert.Equal(obj.Vertices[3], childTwo.Point3);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldCreateTriangles_WhenPolygonsInObjString()
	{
		// Arrange
		const string objData =
			"""
			v -1 1 0
			v -1 0 0
			v 1 0 0
			v 1 1 0
			v 0 2 0

			f 1 2 3 4 5
			""";
		Obj obj = new();

		// Act
		await obj.LoadFromStringAsync(objData);

		// Assert
		var childOne = (Triangle)obj.DefaultGroup[0];
		var childTwo = (Triangle)obj.DefaultGroup[1];
		var childThree = (Triangle)obj.DefaultGroup[2];
		Assert.Equal(obj.Vertices[0], childOne.Point1);
		Assert.Equal(obj.Vertices[1], childOne.Point2);
		Assert.Equal(obj.Vertices[2], childOne.Point3);
		Assert.Equal(obj.Vertices[0], childTwo.Point1);
		Assert.Equal(obj.Vertices[2], childTwo.Point2);
		Assert.Equal(obj.Vertices[3], childTwo.Point3);
		Assert.Equal(obj.Vertices[0], childThree.Point1);
		Assert.Equal(obj.Vertices[3], childThree.Point2);
		Assert.Equal(obj.Vertices[4], childThree.Point3);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldSetNamedGroups_WhenGroupsInObjStr()
	{
		// Arrange
		const string objData =
			"""
			v -1 1 0
			v -1 0 0
			v 1 0 0
			v 1 1 0

			g FirstGroup
			f 1 2 3
			g SecondGroup
			f 1 3 4
			""";
		Obj obj = new();

		// Act
		await obj.LoadFromStringAsync(objData);

		// Assert
		var groupOne = obj.NamedGroups["FirstGroup"];
		var groupTwo = obj.NamedGroups["SecondGroup"];
		var triangleOne = (Triangle)groupOne[0];
		var triangleTwo = (Triangle)groupTwo[0];
		Assert.Equal(obj.Vertices[0], triangleOne.Point1);
		Assert.Equal(obj.Vertices[1], triangleOne.Point2);
		Assert.Equal(obj.Vertices[2], triangleOne.Point3);
		Assert.Equal(obj.Vertices[0], triangleTwo.Point1);
		Assert.Equal(obj.Vertices[2], triangleTwo.Point2);
		Assert.Equal(obj.Vertices[3], triangleTwo.Point3);
	}
	
	[Fact]
	public async Task LoadFromStringAsync_ShouldCreateNormalVector_WhenVNInObjString()
	{
		// Arrange
		const string objData =
			"""
			vn 0 0 1
			vn 0.707 0 -0.707
			vn 1 2 3
			""";
		Vector expectedNormal1 = new(0, 0, 1);
		Vector expectedNormal2 = new(0.707, 0, -0.707);
		Vector expectedNormal3 = new(1, 2, 3);
		Obj obj = new();
		
		// Act
		await obj.LoadFromStringAsync(objData);
		
		// Assert
		Assert.Equal(expectedNormal1, obj.Normals[0]);
		Assert.Equal(expectedNormal2, obj.Normals[1]);
		Assert.Equal(expectedNormal3, obj.Normals[2]);
	}

	[Fact]
	public async Task LoadFromStringAsync_ShouldSetSmoothTriangleNormals_WhenVNInObjString()
	{
		// Arrange
		const string objData =
			"""
			v 0 1 0
			v -1 0 0
			v 1 0 0

			vn -1 0 0
			vn 1 0 0
			vn 0 1 0

			f 1//3 2//1 3//2
			f 1/0/3 2/102/1 3/14/2
			""";
		Obj obj = new();
		SmoothTriangle expectedTriangle = new(
			new Point(0, 1, 0),
			new Point(-1, 0, 0),
			new Point(1, 0, 0),
			new Vector(0, 1, 0),
			new Vector(-1, 0, 0),
			new Vector(1, 0, 0));
		
		// Act
		await obj.LoadFromStringAsync(objData);
		var triangle = (SmoothTriangle)obj.DefaultGroup[0];
		var triangle2 = (SmoothTriangle)obj.DefaultGroup[1];
		
		// Assert
		Assert.Equal(expectedTriangle.Point1, triangle.Point1);
		Assert.Equal(expectedTriangle.Point2, triangle.Point2);
		Assert.Equal(expectedTriangle.Point3, triangle.Point3);
		Assert.Equal(expectedTriangle.Normal1, triangle.Normal1);
		Assert.Equal(expectedTriangle.Normal2, triangle.Normal2);
		Assert.Equal(expectedTriangle.Normal3, triangle.Normal3);
		Assert.Equal(expectedTriangle.Point1, triangle2.Point1);
		Assert.Equal(expectedTriangle.Point2, triangle2.Point2);
		Assert.Equal(expectedTriangle.Point3, triangle2.Point3);
		Assert.Equal(expectedTriangle.Normal1, triangle2.Normal1);
		Assert.Equal(expectedTriangle.Normal2, triangle2.Normal2);
		Assert.Equal(expectedTriangle.Normal3, triangle2.Normal3);
	}
	
	[Fact]
	public async Task ToGroup_ShouldConvertObjToGroup()
	{
		// Arrange
		const string objData =
			"""
			v -1 1 0
			v -1 0 0
			v 1 0 0
			v 1 1 0

			g FirstGroup
			f 1 2 3
			g SecondGroup
			f 1 3 4
			""";
		Obj obj = new();
		await obj.LoadFromStringAsync(objData);
		var groupOne = obj.NamedGroups["FirstGroup"];
		var groupTwo = obj.NamedGroups["SecondGroup"];

		// Act
		var result = obj.ToGroup();

		// Assert
		Assert.Contains(groupOne, result);
		Assert.Contains(groupTwo, result);
	}
}
