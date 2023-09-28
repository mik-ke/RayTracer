using RayTracer.Models;

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
        Sphere sphere2 = new();
        sphere2.Transform = Matrix.Scaling(0.5, 0.5, 0.5);

        World world = new() { LightSource = light };
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
        Assert.Null(world.LightSource);
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
}
