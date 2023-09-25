using RayTracer.Models;
using Xunit;

namespace RayTracer.Tests.Unit;

public class MaterialTests
{
	[Fact]
	public void Constructor_ShouldInitializeCorrectly_WhenDefaultCtorCalled()
	{
		// Arrange
		Color expectedColor = new(1, 1, 1);
		const double expectedAmbient = 0.1;
		const double expectedDiffuse = 0.9;
		const double expectedSpecular = 0.9;
		const double expectedShininess = 200.0;

		// Act
		Material material = new();

		// Assert
		Assert.Equal(expectedColor, material.Color);
		Assert.Equal(expectedAmbient, material.Ambient);
		Assert.Equal(expectedDiffuse, material.Diffuse);
		Assert.Equal(expectedSpecular, material.Specular);
		Assert.Equal(expectedShininess, material.Shininess);
	}

	[Fact]
	public void Lighting_ShouldWork_WhenEyeBetweenLightAndSurface()
	{
		// Arrange
		Material material = new();
		Point position = new(0, 0, 0);
		Vector eye = new(0, 0, -1);
		Vector normal = new(0, 0, -1);
		PointLight light = new(new Point(0, 0, -10), new Color(1, 1, 1));
		Color expected = new(1.9, 1.9, 1.9);

		// Act
		var result = material.Lighting(light, position, eye, normal);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Lighting_ShouldWork_WhenEyeBetweenLightAndSurfaceAndOffset45Degrees()
	{
		// Arrange
		Material material = new();
		Point position = new(0, 0, 0);
		Vector eye = new(0, Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
		Vector normal = new(0, 0, -1);
		PointLight light = new(new Point(0, 0, -10), new Color(1, 1, 1));
		Color expected = new(1.0, 1.0, 1.0);

		// Act
		var result = material.Lighting(light, position, eye, normal);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Lighting_ShouldWork_WhenEyeOppositeSurfaceAndLightOffset45Degrees()
	{
		// Arrange
		Material material = new();
		Point position = new(0, 0, 0);
		Vector eye = new(0, 0, -1);
		Vector normal = new(0, 0, -1);
		PointLight light = new(new Point(0, 10, -10), new Color(1, 1, 1));
		Color expected = new(0.7364, 0.7364, 0.7364);

		// Act
		var result = material.Lighting(light, position, eye, normal);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Lighting_ShouldWork_WhenEyeInReflectionVectorPath()
	{
		// Arrange
		Material material = new();
		Point position = new(0, 0, 0);
		Vector eye = new(0, -Math.Sqrt(2) / 2, -Math.Sqrt(2) / 2);
		Vector normal = new(0, 0, -1);
		PointLight light = new(new Point(0, 10, -10), new Color(1, 1, 1));
		Color expected = new(1.6364, 1.6364, 1.6364);

		// Act
		var result = material.Lighting(light, position, eye, normal);

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void Lighting_ShouldBeAmbient_WhenLightBehindSurface()
	{
		// Arrange
		Material material = new();
		Point position = new(0, 0, 0);
		Vector eye = new(0, 0, -1);
		Vector normal = new(0, 0, -1);
		PointLight light = new(new Point(0, 0, 10), new Color(1, 1, 1));
		Color expected = new(material.Ambient, material.Ambient, material.Ambient);

		// Act
		var result = material.Lighting(light, position, eye, normal);

		// Assert
		Assert.Equal(expected, result);
	}

    #region equality
    [Fact]
	public void Equals_ShouldReturnTrue_WhenTwoObjectsHaveSameProperties()
	{
		// Arrange
		Material material1 = new()
		{
			Color = new Color(0, 1, 0),
			Ambient = 0.2,
			Diffuse = 0.8,
			Specular = 0.7,
			Shininess = 20
        };
		Material material2 = new()
		{
			Color = new Color(0, 1, 0),
			Ambient = 0.2,
			Diffuse = 0.8,
			Specular = 0.7,
			Shininess = 20
        };

		// Act
		var result = material1.Equals(material2);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public void Equals_ShouldReturnFalse_WhenTwoObjectsHaveDifferentProperties()
	{
		// Arrange
		Material material1 = new()
		{
			Color = new Color(0, 1, 2),
			Ambient = 0.1,
			Diffuse = 0.1,
			Specular = 0.1,
			Shininess = 19
        };
		Material material2 = new()
		{
			Color = new Color(0, 1, 0),
			Ambient = 0.2,
			Diffuse = 0.8,
			Specular = 0.7,
			Shininess = 20
        };

		// Act
		var result = material1.Equals(material2);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void Equals_ShouldReturnFalse_WhenOtherIsNull()
	{
		// Arrange
		Material material1 = new();
		Material material2 = null!;

		// Act
		var result = material1.Equals(material2);

		// Assert
		Assert.False(result);
	}
    #endregion
}
