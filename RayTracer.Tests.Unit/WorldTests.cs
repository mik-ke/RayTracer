using RayTracer.Models;
using RayTracer.Shapes;
using Xunit;

namespace RayTracer.Tests.Unit;

public class WorldTests
{
    /// <summary>
    /// Returns a default world for various tests.
    /// Contains a light source at (-10, 10, -10)
    /// and two concentric spheres where the outermost is a unit sphere
    /// and the innnermost has a radius of 0.5. Both lie at the origin.
    /// </summary>
	public static World DefaultTestWorld()
	{
        PointLight light = new(new Point(-10, 10, -10), new Color(1, 1, 1));
        Sphere sphere1 = new();
        sphere1.Material.Color = new(0.8, 1.0, 0.6);
        sphere1.Material.Diffuse = 0.7;
        sphere1.Material.Specular = 0.2;
        Sphere sphere2 = new()
        {
            Transform = Matrix.Scaling(0.5, 0.5, 0.5)
        };

        World world = new();
        world.LightSources.Add(light);
        world.Objects.Add(sphere1);
        world.Objects.Add(sphere2);

        return world;
	}

	[Fact]
	public void Constructor_ShouldInitializeEmptyWorld()
	{
		// Arrange
		// Act
		World world = new();

        // Assert
        Assert.Empty(world.Objects);
        Assert.Empty(world.LightSources);
	}

    [Fact]
    public void Intersect_ShouldReturnCorrectTValues_WhenCalledWithDefaultWorld()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
        const int expectedLength = 4;
        const double t1 = 4;
        const double t2 = 4.5;
        const double t3 = 5.5;
        const double t4 = 6;

        // Act
        Intersections result = world.Intersect(ray);

        // Assert
        Assert.Equal(expectedLength, result.Length);
        Assert.Equal(t1, result[0].T);
        Assert.Equal(t2, result[1].T);
        Assert.Equal(t3, result[2].T);
        Assert.Equal(t4, result[3].T);
    }

    [Fact]
    public void ShadeHit_ShouldBeCorrectColor_WhenComputationsGiven()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
        Sphere shape = world.Objects[0];
        Intersection intersection = new(4, shape);
        Computations computations = new(intersection, ray);
        Color expected = new(0.38066, 0.47583, 0.2855);

        // Act
        var actual = world.ShadeHit(computations);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShadeHit_ShouldBeCorrectColor_WhenIntersectionInsideObject()
    {
        // Arrange
        World world = DefaultTestWorld();
        world.LightSources.Clear();
        world.LightSources.Add(new PointLight(new Point(0, 0.25, 0), new Color(1, 1, 1)));
        Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));
        Sphere shape = world.Objects[1];
        Intersection intersection = new(0.5, shape);
        Computations computations = new(intersection, ray);
        Color expected = new(0.90498, 0.90498, 0.90498);

        // Act
        var actual = world.ShadeHit(computations);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShadeHit_ShouldBeCorrect_WhenSecondSphereInShadow()
    {
        // Arrange
        World world = new();
        PointLight light = new(new Point(0, 0, -10), new Color(1, 1, 1));
        world.LightSources.Add(light);
        Sphere sphere1 = new();
        world.Objects.Add(sphere1);
        Sphere sphere2 = new()
        {
            Transform = Matrix.Translation(0, 0, 10)
        };
        world.Objects.Add(sphere2);
        Ray ray = new(new Point(0, 0, 5), new Vector(0, 0, 1));
        Intersection intersection = new(4, sphere2);
        Computations computations = new(intersection, ray);
        Color expected = new(0.1, 0.1, 0.1);

        // Act
        var actual = world.ShadeHit(computations);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ColorAt_ShouldBeBlack_WhenRayMisses()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 1, 0));
        Color expected = Color.Black;

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ColorAt_ShouldBeCorrect_WhenRayHits()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
        Color expected = new(0.38066, 0.47583, 0.2855);

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ColorAt_ShouldBeCorrect_WhenRayBetweenOuterAndInnerSpheres()
    {
        // Arrange
        World world = DefaultTestWorld();
        Sphere outer = world.Objects[0];
        Sphere inner = world.Objects[1];
        outer.Material.Ambient = 1;
        inner.Material.Ambient = 1;
        Ray ray = new(new Point(0, 0, 0.75), new Vector(0, 0, -1));
        Color expected = inner.Material.Color;

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsShadowed_ShouldBeFalse_WhenNothingCollinearWithPointAndLight()
    {
        // Arrange
        World world = DefaultTestWorld();
        Point point = new(0, 10, 0);
        PointLight light = world.LightSources[0];

        // Act
        var result = world.IsShadowed(point, light);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsShadowed_ShouldBeTrue_WhenObjectBetweenLightAndPoint()
    {
        // Arrange
        World world = DefaultTestWorld();
        Point point = new(10, -10, 10);
        PointLight light = world.LightSources[0];

        // Act
        var result = world.IsShadowed(point, light);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsShadowed_ShouldBeFalse_WhenObjectBehindLight()
    {
        // Arrange
        World world = DefaultTestWorld();
        Point point = new(-20, 20, -20);
        PointLight light = world.LightSources[0];

        // Act
        var result = world.IsShadowed(point, light);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsShadowed_ShouldBeFalse_WhenObjectBehindPoint()
    {
        // Arrange
        World world = DefaultTestWorld();
        Point point = new(-2, 2, -2);
        PointLight light = world.LightSources[0];

        // Act
        var result = world.IsShadowed(point, light);

        // Assert
        Assert.False(result);
    }
}
