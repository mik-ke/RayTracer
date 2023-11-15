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

    /// <summary>
    /// Saves the <see cref="Canvas"/> as a PPM format image to the given <paramref name="path"/>
    /// </summary>
    public static async Task SaveToPpmFileAsync(this Canvas canvas, IPpmWriter ppmWriter, string path)
    {
        using var stream = File.Create(path);
        await ppmWriter.WriteAsync(canvas, stream);
    }
}
