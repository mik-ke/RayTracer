
using RayTracer.Shapes;

namespace RayTracer.Parsers;

/// <summary>
/// Parser for the Wavefront OBJ file format.
/// </summary>
public class ObjParser
{
    /// <summary>
    /// Parses the OBJ-formatted <paramref name="objData"/> data into a <see cref="Shapes.Group"/>.
    /// </summary>
    public async Task<Group> Parse(string objData)
    {
        Group group = new();
        using (StringReader reader = new StringReader(objData))
        {
            string? currentLine;
            while ((currentLine = await reader.ReadLineAsync()) != null)
            {

            }
        }
        return null!;
    }
}
