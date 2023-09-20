using RayTracer.Interfaces;
using RayTracer.Models;
using System.Text;

namespace RayTracer.Extensions;

public static class CanvasExtensions
{
    /// <summary>
    /// Returns the <see cref="Canvas"/> converted to a string representation of a PPM format image
    /// </summary>
    public static async Task<string> GetAsPpmString(this Canvas canvas, IPpmWriter ppmWriter)
    {
        using var stream = new MemoryStream();
        await ppmWriter.WriteAsync(canvas, stream);
        return Encoding.Default.GetString(stream.ToArray());
    }
}
