
using RayTracer.Models;
using System.Globalization;

namespace RayTracer.Parsers;

/// <summary>
/// Class that represents the information of the Wavefront OBJ file format.
/// <see cref="LoadFromStringAsync(string)"/> should be called to initialize the object.
/// </summary>
public class Obj
{
    public List<Point> Vertices;

    /// <summary>
    /// Parses the OBJ-formatted <paramref name="objData"/> data and initializes the <see cref="Obj"/>.
    /// </summary>
    public async Task LoadFromStringAsync(string objData)
    {
        Vertices = new();

        using (StringReader reader = new StringReader(objData))
        {
            string? currentLine;
            while ((currentLine = await reader.ReadLineAsync()) != null)
            {
                ProcessLine(currentLine);
            }
        }
    }

    private void ProcessLine(in string line)
    {
        string[] arguments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (arguments.Length == 0) return;

        switch (arguments[0])
        {
            case "v":
                ProcessVertice(arguments);
                break;
        }
    }

    private void ProcessVertice(in string[] arguments)
    {
        if (arguments.Length != 4) return;

        if (!double.TryParse(arguments[1],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out double x)) return;
        if (!double.TryParse(arguments[2],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out double y)) return;
        if (!double.TryParse(arguments[3],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out double z)) return;

        Point vertice = new(x, y, z);
        Vertices.Add(vertice);
    }
}
