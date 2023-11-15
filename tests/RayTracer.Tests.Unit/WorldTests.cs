using RayTracer.Models;
using RayTracer.Shapes;

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
        Sphere shape = (Sphere)world.Objects[0];
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
        Sphere shape = (Sphere)world.Objects[1];
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
        var actual = world.ShadeHit(computations, 5);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShadeHit_ShouldBeCorrect_WhenMaterialReflective()
    {
        // Arrange
        World world = DefaultTestWorld();
        Matrix transform = Matrix.Translation(0, -1, 0);
        Plane shape = new(transform);
        shape.Material.Reflective = 0.5;
        world.Objects.Add(shape);
        Ray ray = new(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        Intersection intersection = new(Math.Sqrt(2), shape);
        Computations computations = new(intersection, ray);
        Color expected = new(0.87677, 0.92436, 0.82918);

        // Act
        var actual = world.ShadeHit(computations, 5);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShadeHit_ShouldBeCorrect_WhenMaterialTransparent()
    {
        // Arrange
        World world = DefaultTestWorld();

        Matrix floorTransform = Matrix.Translation(0, -1, 0);
        Plane floor = new(floorTransform);
        floor.Material.Transparency = 0.5;
        floor.Material.RefractiveIndex = 1.5;
        world.Objects.Add(floor);

        Matrix ballTransform = Matrix.Translation(0, -3.5, -0.5);
        Sphere ball = new(ballTransform);
        ball.Material.Color = new(1, 0, 0);
        ball.Material.Ambient = 0.5;
        world.Objects.Add(ball);

        Ray ray = new(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        Intersection intersection = new(Math.Sqrt(2), floor);
        Intersections intersections = new(intersection);
        Computations computations = new(intersection, ray, intersections);
        Color expected = new(0.93642, 0.68642, 0.68642);

        // Act
        var actual = world.ShadeHit(computations);

        // Assert
        Assert.Equal(expected, actual);
    }


    [Fact]
    public void ShadeHit_ShouldBeCorrect_WhenMaterialTransparentAndReflective()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));

        Matrix floorTransform = Matrix.Translation(0, -1, 0);
        Plane floor = new(floorTransform);
        floor.Material.Reflective = 0.5;
        floor.Material.Transparency = 0.5;
        floor.Material.RefractiveIndex = 1.5;
        world.Objects.Add(floor);

        Matrix ballTransform = Matrix.Translation(0, -3.5, -0.5);
        Sphere ball = new(ballTransform);
        ball.Material.Color = new(1, 0, 0);
        ball.Material.Ambient = 0.5;
        world.Objects.Add(ball);

        Intersection intersection = new(Math.Sqrt(2), floor);
        Intersections intersections = new(intersection);
        Computations computations = new(intersection, ray, intersections);
        Color expected = new(0.93391, 0.69643, 0.69243);

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
        Sphere outer = (Sphere)world.Objects[0];
        Sphere inner = (Sphere)world.Objects[1];
        outer.Material.Ambient = 1;
        inner.Material.Ambient = 1;
        Ray ray = new(new Point(0, 0, 0.75), new Vector(0, 0, -1));
        Color expected = inner.Material.Color;

        // Act
        var actual = world.ColorAt(ray);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact(Timeout = 100)]
    public void ColorAt_ShouldTerminate_WhenTwoMutuallyReflectiveSurfaces()
    {
        // Arrange
        World world = new();
        world.LightSources.Add(new PointLight(new Point(0, 0, 0), new Color(1, 1, 1)));
        Plane lower = new(Matrix.Translation(0, -1, 0));
        lower.Material.Reflective = 1;
        world.Objects.Add(lower);
        Plane upper = new(Matrix.Translation(0, 1, 0));
        upper.Material.Reflective = 1;
        world.Objects.Add(upper);
        Ray ray = new(new Point(0, 0, 0), new Vector(0, 1, 0));

        // Act
        // Assert
        world.ColorAt(ray);
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

    [Fact]
    public void ReflectedColor_ShouldReturnBlack_WhenNonreflectiveMaterial()
    {
        // Arrange
        World world = DefaultTestWorld();
        Ray ray = new(new Point(0, 0, 0), new Vector(0, 0, 1));
        Shape shape = world.Objects[1];
        shape.Material.Ambient = 1;
        Intersection intersection = new(1, shape);
        Computations computations = new(intersection, ray);
        Color expected = Color.Black;

        // Act
        var actual = world.ReflectedColor(computations);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ReflectedColor_ShouldReturnCorrect_WhenReflectiveMaterial()
    {
        // Arrange
        World world = DefaultTestWorld();
        Matrix transform = Matrix.Translation(0, -1, 0);
        Plane shape = new(transform);
        shape.Material.Reflective = 0.5;
        world.Objects.Add(shape);
        Ray ray = new(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        Intersection intersection = new(Math.Sqrt(2), shape);
        Computations computations = new(intersection, ray);
        Color expected = new(0.19032, 0.2379, 0.14274);

        // Act
        var actual = world.ReflectedColor(computations);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ReflectedColor_ShouldReturnBlackInstantly_WhenAtMaximumRecursiveDepth()
    {
        // Arrange
        World world = DefaultTestWorld();
        Matrix transform = Matrix.Translation(0, -1, 0);
        Plane shape = new(transform);
        shape.Material.Reflective = 0.5;
        world.Objects.Add(shape);
        Ray ray = new(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2) / 2, Math.Sqrt(2) / 2));
        Intersection intersection = new(Math.Sqrt(2), shape);
        Computations computations = new(intersection, ray);
        Color expected = new(0, 0, 0);

        // Act
        var actual = world.ReflectedColor(computations, 0);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RefractedColor_ShouldReturnBlack_WhenHitObjectOpaque()
    {
        // Arrange
        World world = DefaultTestWorld();
        Shape shape = world.Objects[0];
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
        Intersections intersections = new(
            new Intersection(4, shape),
            new Intersection(6, shape));
        Computations computations = new(intersections[0], ray, intersections);
        Color expected = Color.Black;

        // Act
        var actual = world.RefractedColor(computations, 5);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RefractedColor_ShouldReturnBlackInstantly_WhenAtMaximumRecursiveDepth()
    {
        // Arrange
        World world = DefaultTestWorld();
        Shape shape = world.Objects[0];
        shape.Material.Transparency = 1.0;
        shape.Material.RefractiveIndex = 1.5;
        Ray ray = new(new Point(0, 0, -5), new Vector(0, 0, 1));
        Intersections intersections = new(
            new Intersection(4, shape),
            new Intersection(6, shape));
        Computations computations = new(intersections[0], ray, intersections);
        Color expected = Color.Black;

        // Act
        var actual = world.RefractedColor(computations, 0);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RefractedColor_ShouldReturnBlack_WhenTotalInternalReflection()
    {
        // Arrange
        World world = DefaultTestWorld();
        Shape shape = world.Objects[0];
        shape.Material.Transparency = 1.0;
        shape.Material.RefractiveIndex = 1.5;
        Ray ray = new(new Point(0, 0, Math.Sqrt(2) / 2), new Vector(0, 1, 0));
        Intersections intersections = new(
            new Intersection(-Math.Sqrt(2) / 2, shape),
            new Intersection(Math.Sqrt(2) / 2, shape));
        Computations computations = new(intersections[1], ray, intersections);
        Color expected = Color.Black;

        // Act
        var actual = world.RefractedColor(computations, 5);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RefractedColor_ShouldReturnCorrect_WhenPassesThrough()
    {
        // Arrange
        World world = DefaultTestWorld();
        Shape shapeA = world.Objects[0];
        shapeA.Material.Ambient = 1.0;
        shapeA.Material.Pattern = new TestPattern();
        Shape shapeB = world.Objects[1];
        shapeB.Material.Transparency = 1.0;
        shapeB.Material.RefractiveIndex = 1.5;
        Ray ray = new(new Point(0, 0, 0.1), new Vector(0, 1, 0));
        Intersections intersections = new(
            new Intersection(-0.9899, shapeA),
            new Intersection(-0.4899, shapeB),
            new Intersection(0.4899, shapeB),
            new Intersection(0.9899, shapeA));
        Computations computations = new(intersections[2], ray, intersections);
        Color expected = new(0, 0.99888, 0.04725);

        // Act
        var actual = world.RefractedColor(computations, 5);

        // Assert
        Assert.Equal(expected, actual);
    }
}
