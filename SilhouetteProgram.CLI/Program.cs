using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Utilities;

namespace SilhouetteProgram.CLI;

/// <summary>
/// The program draws the silhouette of a sphere on to a wall (i.e. a canvas)
/// then creates a PPM image file of the canvas.
/// </summary>
internal class Program
{
    #region fields
    static readonly Point _rayOrigin = new Point(0, 0, -5);
    static readonly double _wallZ = 10;
    static readonly double _wallSize = 7;
    static readonly double _wallHalf = _wallSize / 2;
    static readonly int _canvasPixels = 100;
    static readonly double _pixelSize = _wallSize / _canvasPixels;
    static readonly Color _red = new Color(1, 0, 0);
    #endregion

    static async Task Main(string[] args)
    {
        Canvas canvas = new(_canvasPixels, _canvasPixels);
        Sphere sphere = new();
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
                if (intersections.Hit() != null)
                {
                    canvas[x, y] = _red;
                }
            }
        }

        var writer = new PpmWriter();
        await canvas.SaveToPpmFileAsync(writer, "silhouette.ppm");
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