using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Utilities;

internal class Program
{
    static async Task Main()
    {
        World world = new();
        AddFloorAndWalls(world);
        AddLeftWallDecorations(world);
        AddTable(world);
        AddShapesOnTable(world);
        AddMirror(world);
        AddLightSources(world);

        var camera = CreateCamera();
        Canvas canvas = camera.Render(world);

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "cube_render.ppm");
    }

    static Camera CreateCamera()
    {
        Point from = new(10, 7, -11);
        Point to = new(0, 3, 0);
        Vector up = new(0, 1, 0);
        Matrix cameraTransform = Matrix.View(from, to, up);

        return new Camera(250, 125, Math.PI / 3, cameraTransform);
    }

    static void AddFloorAndWalls(World world)
    {
        var floor = CreateFloor();
        var backdrop = CreateBackdrop();
        var leftWall = CreateLeftWall();

        world.Objects.Add(floor);
        world.Objects.Add(backdrop);
        world.Objects.Add(leftWall);
    }

    static Plane CreateFloor()
    {
        return new Plane()
        {
            Material = new()
            {
                Color = Color.White,
                Specular = 0,
                Diffuse = 0.7,
                Pattern = new CheckersPattern(Color.Black, Color.White),
            }
        };
    }

    static Plane CreateBackdrop()
    {
        return new Plane()
        {
            Transform = Matrix.RotationX(Math.PI / 2).Translate(0, 0, 10),
            Material = new()
            {
                Color = Color.White,
                Specular = 0,
                Diffuse = 0.7,
                Pattern = new StripePattern(new Color(0.4, 0.2, 0), new Color(0.8, 0.4, 0), Matrix.Scaling(0.7, 1, 1))
            }
        };
    }

    static Plane CreateLeftWall()
    {
        return new Plane()
        {
            Transform = Matrix.RotationX(Math.PI / 2).RotateY(Math.PI / 2).Translate(-10, 0, 0),
            Material = new()
            {
                Color = Color.White,
                Specular = 0,
                Diffuse = 0.7,
                Pattern = new StripePattern(new Color(0.4, 0.2, 0), new Color(0.8, 0.4, 0), Matrix.Scaling(0.7, 1, 1))
            }
        };
    }

    static void AddLeftWallDecorations(World world)
    {
        Matrix redTransform = Matrix.Scaling(0.1, 1, 1).Translate(-9.99, 5, -1);
        Cube redSquare = new(redTransform)
        {
            Material = new() { Color = new(1, 0, 0) }
        };

        Matrix greenTransform = Matrix.Scaling(0.1, 0.5, 0.5).Translate(-9.99, 5.5, 1);
        Cube greenSquare = new(greenTransform)
        {
            Material = new() { Color = new(0, 1, 0) }
        };

        Matrix blueTransform = Matrix.Scaling(0.1, 0.5, 0.5).Translate(-9.99, 4.5, 1);
        Cube blueSquare = new(blueTransform)
        {
            Material = new() { Color = new(0, 0, 1) }
        };

        world.Objects.Add(redSquare);
        world.Objects.Add(greenSquare);
        world.Objects.Add(blueSquare);
    }

    static void AddTable(World world)
    {
        AddLegs(world);
        AddTableSurface(world);
    }

    static void AddLegs(World world)
    {
        var leg1 = CreateLeg();
        leg1.Transform = leg1.Transform.Translate(-3, 0, -2);
        var leg2 = CreateLeg();
        leg2.Transform = leg2.Transform.Translate(-3, 0, 2);
        var leg3 = CreateLeg();
        leg3.Transform = leg3.Transform.Translate(3, 0, -2);
        var leg4 = CreateLeg();
        leg4.Transform = leg4.Transform.Translate(3, 0, 2);

        world.Objects.Add(leg1);
        world.Objects.Add(leg2);
        world.Objects.Add(leg3);
        world.Objects.Add(leg4);
    }

    static Cube CreateLeg()
    {
        Matrix transform = Matrix.Scaling(0.1, 3, 0.1);
        return new Cube(transform)
        {
            Material = new()
            {
                Color = new Color(0.8, 0.4, 0),
                Diffuse = 0.7,
                Reflective = 0.1
            }
        };
    }

    static void AddTableSurface(World world)
    {
        Matrix transform = Matrix.Scaling(3.2, 0.1, 2.2).Translate(0, 3, 0);
        var surface = new Cube(transform)
        {
            Material = new()
            {
                Color = Color.White,
                Diffuse = 0.7,
                Pattern = new StripePattern(new Color(0.4, 0.2, 0), new Color(0.8, 0.4, 0), Matrix.Scaling(0.1, 1, 0.2)),
                Reflective = 0.4
            }
        };
        world.Objects.Add(surface);
    }

    static void AddShapesOnTable(World world)
    {
        Matrix blueTransform = Matrix.Scaling(0.3, 0.3, 0.3).Translate(0, 3.5, 0).RotateY(Math.PI / 5);
        var blueCube = new Cube(blueTransform)
        {
            Material = new()
            {
                Color = new(0, 0, 0.7),
                Diffuse = 0.7,
                Reflective = 0.7,
                Transparency = 0.9,
                RefractiveIndex = 1.5
            }
        };

        Matrix redTransform = Matrix.Scaling(0.5, 0.5, 0.5).Translate(2, 3.5, 0);
        var redSphere = new Sphere(redTransform)
        {
            Material = new()
            {
                Color = new(0.5, 0, 0),
                Diffuse = 0.7,
                Reflective = 0.9,
                Transparency = 0.3,
                RefractiveIndex = 1.5
            }
        };

        Matrix brownTransform = Matrix.Translation(2, 3.15, 0);
        var brownOpenCylinder = new Cylinder(brownTransform)
        {
            Minimum = 0,
            Maximum = 0.5,
            Material = new()
            {
                Color = new(0.8, 0.4, 0),
                Diffuse = 0.3,
                Reflective = 1,
                Transparency = 0.95,
                RefractiveIndex = 1.5
            }
        };

        Matrix greenTransform = Matrix.Scaling(0.5, 0.5, 0.5).Translate(-1, 3.15, -1.25);
        var greenCylinder = new Cylinder(greenTransform)
        {
            Minimum = 0,
            Maximum = 3,
            Material = new()
            {
                Color = new(0, 0.3, 0),
                Diffuse = 0.3,
                Reflective = 1,
                Transparency = 0.7,
                RefractiveIndex = 1.5
            }
        };

        world.Objects.Add(blueCube);
        world.Objects.Add(redSphere);
        world.Objects.Add(brownOpenCylinder);
        world.Objects.Add(greenCylinder);
    }

    static void AddMirror(World world)
    {
        Matrix transform = Matrix.Scaling(5, 3, 1).Translate(0, 5, 9.9);
        var mirror = new Cube(transform)
        {
            Material = new()
            {
                Color = new(0, 0, 0),
                Diffuse = 0.7,
                Reflective = 1.0,
            }
        };
        Matrix frameTransform = Matrix.Scaling(5.2, 3.2, 1).Translate(0, 5, 9.95);
        var frame = new Cube(frameTransform)
        {
            Material = new()
            {
                Color = new(0.2, 0.098, 0),
                Diffuse = 0.7
            }
        };

        world.Objects.Add(mirror);
        world.Objects.Add(frame);
    }

    static void AddLightSources(World world)
    {
        PointLight lightSource = new(new Point(7, 10, -11), new Color(1, 1, 1));
        world.LightSources.Add(lightSource);
    }
}