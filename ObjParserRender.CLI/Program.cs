using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Parsers;
using RayTracer.Patterns;
using RayTracer.Shapes;
using RayTracer.Utilities;
using System.Reflection;

namespace ObjParserRender.CLI;

internal class Program
{
    static async Task Main()
    {

        World world = new();

        PointLight lightSource = new(new Point(7, 10, -11), new Color(1, 1, 1));
        world.LightSources.Add(lightSource);

        AddFloorAndWalls(world);

        var objGroup = await LoadObjGroupAsync();
        objGroup.Divide(50);
        world.Objects.Add(objGroup);

        // TODO: try again after bounding boxes implemented
        // OBJ file has thousands of triangles, so render takes time
        var camera = CreateCamera();
        Canvas canvas = camera.Render(world);

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "OBJ_render.ppm");
    }

    static async Task<Group> LoadObjGroupAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var objResourceName = "ObjParserRender.CLI.OBJFiles.teapot.obj";
        using Stream stream = assembly.GetManifestResourceStream(objResourceName)!;
        using StreamReader reader = new(stream);

        Obj obj = new();
        await obj.LoadFromTextReaderAsync(reader);
        return obj.ToGroup();
    }

    static Camera CreateCamera()
    {
        Point from = new(10, 7, -11);
        Point to = new(0, 3, 0);
        Vector up = new(0, 1, 0);
        Matrix cameraTransform = Matrix.View(from, to, up);

        return new Camera(100, 50, Math.PI / 3, cameraTransform);
    }

    static void AddFloorAndWalls(World world)
    {
        var floor = CreateFloor();
        var backdrop = CreateBackdrop();
        var rightWall = CreateLeftWall();
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
                Specular = 0
            },
            Transform = Matrix.RotationY(Math.PI / 4)
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
                Pattern = new CheckersPattern(Color.White, Color.Black)
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
                Pattern = new CheckersPattern(Color.White, Color.Black)
            }
        };
    }
}