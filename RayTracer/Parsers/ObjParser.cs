
using RayTracer.Models;
using RayTracer.Shapes;
using System.Globalization;

namespace RayTracer.Parsers;

/// <summary>
/// Class that represents the information of the Wavefront OBJ file format.
/// <see cref="LoadFromStringAsync(string)"/> should be called to initialize the object.
/// </summary>
public class Obj
{
    public List<Point> Vertices = new(0);

    /// <summary>
    /// Represents the OBJ file as a <see cref="Shapes.Group"/>
    /// </summary>
    public Group Group = new();

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
                ProcessVertex(arguments);
                break;
            case "f":
                ProcessTriangle(arguments);
                break;
        }
    }

    private void ProcessVertex(in string[] arguments)
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

        Point vertex = new(x, y, z);
        Vertices.Add(vertex);
    }

    private void ProcessTriangle(in string[] arguments)
    {
        if (arguments.Length != 4) return;

        if (!int.TryParse(arguments[1],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out int point1Index)) return;
        if (!IsPointIndexInbounds(point1Index)) return;

        if (!int.TryParse(arguments[2],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out int point2Index)) return;
        if (!IsPointIndexInbounds(point2Index)) return;

        if (!int.TryParse(arguments[3],
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out int point3Index)) return;
        if (!IsPointIndexInbounds(point3Index)) return;


        Triangle triangle = new(Vertices[point1Index - 1], Vertices[point2Index - 1], Vertices[point3Index - 1]);
        Group.AddChild(triangle);
    }

    private bool IsPointIndexInbounds(int pointIndex) => pointIndex >= 1 && pointIndex <= Vertices.Count;
}
