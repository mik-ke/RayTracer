using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using RayTracer.Utilities;

namespace WorldRender.CLI;

internal class Program
{
    static async Task Main()
    {
        World world = new();
        AddFloorAndWalls(world);
        AddSpheres(world);
        AddLightSources(world);

        Point from = new(0, 1.5, -5);
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
        var leftWall = CreateLeftWall(floor);
        var rightWall = CreateRightWall(floor);
        world.Objects.Add(floor);
        world.Objects.Add(leftWall); 
        world.Objects.Add(rightWall); 
    }

    static Sphere CreateFloor()
    {
        Sphere floor = new()
        {
            Transform = Matrix.Scaling(10, 0.01, 10),
            Material = new()
            {
                Color = new(1, 0.9, 0.9),
                Specular = 0
            }
        };

        return floor;
    }

    static Sphere CreateLeftWall(Sphere floor)
    {
        Matrix leftRotationY = Matrix.RotationY(-Math.PI / 4);
        return CreateWall(leftRotationY, floor);
    }

    static Sphere CreateRightWall(Sphere floor)
    {
        Matrix rightRotationY = Matrix.RotationY(Math.PI / 4);
        return CreateWall(rightRotationY, floor);
    }

    static Sphere CreateWall(Matrix rotationY, Sphere floor)
    {
        Sphere wall = new()
        {
            Transform = Matrix.Translation(0, 0, 5)
                * rotationY
                * Matrix.RotationX(Math.PI / 2)
                * Matrix.Scaling(10, 0.01, 10),
            Material = floor.Material
        };

        return wall;
    }

    static void AddSpheres(World world)
    {
        // create large sphere in the middle
        Sphere middle = new()
        {
            Transform = Matrix.Translation(-0.5, 1, 0.5),
            Material = new()
            {
                Color = new(0.1, 1, 0.5),
                Diffuse = 0.7,
                Specular = 0.3
            }
        };

        // smaller green sphere on the right
        Sphere right = new()
        {
            Transform = Matrix.Translation(1.5, 0.5, -0.5) * Matrix.Scaling(0.5, 0.5, 0.5),
            Material = new()
            {
                Color = new(0.5, 1, 0.1),
                Diffuse = 0.7,
                Specular = 0.3
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
                Specular = 0.3
            }
        };

        world.Objects.Add(middle);
        world.Objects.Add(right);
        world.Objects.Add(left);
    }

    static void AddLightSources(World world)
    {
        PointLight lightSource = new(new Point(-10, 10, -10), new Color(1, 1, 1));
        world.LightSources.Add(lightSource);
    }
}