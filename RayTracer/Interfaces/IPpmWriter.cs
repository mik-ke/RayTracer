using RayTracer.Models;

namespace RayTracer.Interfaces;

public interface IPpmWriter
{
    /// <summary>
    /// Writes the <paramref name="canvas"/> in PPM format to the given <paramref name="stream"/>.
    /// </summary>
    Task WriteAsync(Canvas canvas, Stream stream);
}
