using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Shapes;
using RayTracer.Utilities;

namespace HexagonRender.CLI;

internal class Program
{
    static async Task Main()
    {
        World world = new();

        PointLight lightSource = new(new Point(-3, 2, 2), new Color(1, 1, 1));
        world.LightSources.Add(lightSource);

        var hexagon = GetHexagon();
        hexagon.Transform = Matrix.Translation(0, 1, 0);
        world.Objects.Add(hexagon);

        var camera = CreateCamera();
        Canvas canvas = camera.Render(world);

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "hexagon_render.ppm");
    }

    static Camera CreateCamera()
    {
        Point from = new(3, 4, -2);
        Point to = new(0, 1, 0);
        Vector up = new(0, 1, 0);
        Matrix cameraTransform = Matrix.View(from, to, up);

        return new Camera(1000, 500, Math.PI / 3, cameraTransform);
    }

    static Group GetHexagon()
    {
        Group hexagon = new();

        Material hexagonMaterial = new()
        {
            Color = new(0.7, 0, 0),
            Transparency = 0.7
        };

        for (int i = 0; i < 6; i++)
        {
            var side = GetHexagonSide(hexagonMaterial);
            side.Transform = Matrix.RotationY(i * Math.PI / 3);
            hexagon.AddChild(side);
        }

        return hexagon;
    }

    static Group GetHexagonSide(Material material)
    {
        Group side = new();
        side.AddChild(GetHexagonCorner(material));
        side.AddChild(GetHexagonEdge(material));
        return side;
    }

    static Sphere GetHexagonCorner(Material material)
    {
        Matrix cornerTransform = Matrix.Scaling(0.25, 0.25, 0.25)
            .Translate(0, 0, -1);
        Sphere corner = new(cornerTransform)
        {
            Material = material
        };
        return corner;
    }

    static Cylinder GetHexagonEdge(Material material)
    {
        Matrix edgeTransform = Matrix.Scaling(0.25, 1, 0.25)
            .RotateZ(-Math.PI / 2)
            .RotateY(-Math.PI / 6)
            .Translate(0, 0, -1);
        Cylinder edge = new(edgeTransform)
        {
            Minimum = 0,
            Maximum = 1,
            Material = material
        };
        return edge;
    }
}