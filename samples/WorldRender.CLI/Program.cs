using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using RayTracer.Utilities;
using RayTracer.Patterns;

namespace WorldRender.CLI;

internal class Program
{
    static async Task Main()
    {
        World world = new();
        AddFloorAndWalls(world);
        AddSpheres(world);
        AddLightSources(world);

        Point from = new(0, 1.5, -10);
        Point to = new(0, 1, 0);
        Vector up = new(0, 1, 0);
        Matrix cameraTransform = Matrix.View(from, to, up);
        Camera camera = new(250, 125, Math.PI / 3, cameraTransform);

        Canvas canvas = camera.Render(world);

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "rendered_world.ppm");
    }

    static void AddFloorAndWalls(World world)
    {
        var floor = CreateFloor();
        var backdrop = CreateBackdrop();
        var rightWall = CreateRightWall();
        world.Objects.Add(floor);
        world.Objects.Add(backdrop);
        world.Objects.Add(rightWall);
    }

    static Plane CreateFloor()
    {
        return new Plane()
        {
            Material = new()
            {
                Color = new(1, 0.9, 0.9),
                Specular = 0,
                Pattern = new RingPattern(new Color(0.25, 0.25, 0.25), new Color(0.75, 0.75, 0.75)),
                Reflective = 0.5
            },
            Transform = Matrix.RotationY(Math.PI / 4)
        };
    }

    static Plane CreateBackdrop()
    {
        return new Plane()
        {
            Transform = Matrix.RotationX(Math.PI / 2).Translate(0, 0, 5),
            Material = new()
            {
                Color = Color.White,
                Specular = 0,
                Pattern = new CheckersPattern(Color.White, Color.Black)
            }
        };
    }

    static Plane CreateRightWall()
    {
        return new Plane()
        {
            Transform = Matrix.RotationX(Math.PI / 2).RotateY(Math.PI / 2).Translate(5, 0, 0),
            Material = new()
            {
                Color = Color.White,
                Specular = 0,
                Pattern = new CheckersPattern(Color.White, Color.Black)
            }
        };
    }

    static void AddSpheres(World world)
    {
        // create large sphere in the middle
        Sphere middle = new()
        {
            Transform = Matrix.Translation(-0.5, 1, 0.5),
            Material = new()
            {
                Color = new(1, 0.85, 0.9),
                Diffuse = 0.7,
                Specular = 0.3,
                Reflective = 0.9,
                Transparency = 0.9,
                RefractiveIndex = 1.5
            }
        };

        // smaller sphere on the right
        Sphere right = new()
        {
            Transform = Matrix.Translation(1.5, 0.5, -0.5) * Matrix.Scaling(0.5, 0.5, 0.5),
            Material = new()
            {
                Color = new(0.5, 1, 0.1),
                Diffuse = 0.7,
                Specular = 0.3,
                Pattern = new CheckersPattern(new Color(0, 0, 0.5), new Color(0, 0, 0.9), Matrix.Scaling(0.5, 0.5, 0.5))
            }
        };

        // smallest sphere on the left
        Sphere left = new()
        {
            Transform = Matrix.Translation(-1.5, 0.33, -0.75) * Matrix.Scaling(0.33, 0.33, 0.33),
            Material = new()
            {
                Color = new(1, 0.8, 0.1),
                Diffuse = 0.7,
                Specular = 0.3,
                Pattern = new StripePattern(new Color(0, 1, 0), new Color(0, 0, 0.1), Matrix.RotationZ(Math.PI / 2)),
                Reflective = 0.5,
                Transparency = 0.5
            }
        };

        world.Objects.Add(middle);
        world.Objects.Add(right);
        world.Objects.Add(left);
    }

    static void AddLightSources(World world)
    {
        PointLight lightSource = new(new Point(-3, 3, -3), new Color(1, 1, 1));
        world.LightSources.Add(lightSource);
    }
}