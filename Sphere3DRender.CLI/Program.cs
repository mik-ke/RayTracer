using RayTracer.Models;
using RayTracer.Utilities;
using RayTracer.Extensions;
using RayTracer.Shapes;

namespace Sphere3DRender.CLI;

/// <summary>
/// Creates a 3D rendering of a sphere
/// </summary>
internal class Program
{
    #region fields
    static readonly Point _rayOrigin = new(0, 0, -5);
    static readonly double _wallZ = 10;
    static readonly double _wallSize = 7;
    static readonly double _wallHalf = _wallSize / 2;
    static readonly int _canvasPixels = 100;
    static readonly double _pixelSize = _wallSize / _canvasPixels;
    #endregion

    static async Task Main()
    {
        Canvas canvas = new(_canvasPixels, _canvasPixels);
        Sphere sphere = new();
        sphere.Material.Color = new Color(0, 0, 0.7); //new(1, 0.2, 1);
        // material prop testing
        //sphere.Material.Ambient = 0.9;
        //sphere.Material.Specular = 0.1;
        //sphere.Material.Diffuse = 0.1;

        // create a light source with a white light behind, above and to the left of the eye
        var lightPosition = new Point(-10, 10, -10);
        var lightColor = new Color(1, 1, 1);
        PointLight light = new(lightPosition, lightColor);

        // transform testing
        //sphere.Transform = Matrix.Scaling(1, 0.5, 1);
        //sphere.Transform = Matrix.Scaling(0.5, 1, 1);
        //sphere.Transform = Matrix.Scaling(0.5, 1, 1).RotateZ(Math.PI / 4);
        //sphere.Transform = Matrix.Scaling(0.5, 1, 1).Shear(1, 0, 0, 0, 0, 0);

        for (int y = 0; y < _canvasPixels; y++)
        {
            var worldY = GetWorldY(y);

            for (int x = 0; x < _canvasPixels; x++)
            {
                var worldX = GetWorldX(x);

                // the point on the wall the ray will target
                var position = new Point(worldX, worldY, _wallZ);

                Ray ray = new(_rayOrigin, (position - _rayOrigin).Normalize());
                var intersections = sphere.Intersect(ray);
                var hit = intersections.Hit();
                if (hit != null)
                {
                    var point = ray.Position(hit.T);
                    var normal = hit.Object.Normal(point);
                    var eyeDirection = -ray.Direction;

                    var lighting = sphere.Material.Lighting(light, point, eyeDirection, normal);

                    canvas[x, y] = lighting;
                }
            }
        }

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "3dsphere.ppm");
    }

    /// <summary>
    /// Converts object y coordinate to a world coordinate
    /// </summary>
    /// <returns>The world y coordinate</returns>
    static double GetWorldY(int y) => _wallHalf - _pixelSize * y;

    /// <summary>
    /// Converts object x coordinate to a world coordinate
    /// </summary>
    /// <returns>The world x coordinate</returns>
    static double GetWorldX(int x) => -_wallHalf + _pixelSize * x;
}