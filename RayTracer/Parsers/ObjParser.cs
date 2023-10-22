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
        if (arguments.Length < 4) return;

        List<int> pointIndices = new();
        for (int i = 1; i  < arguments.Length; i++)
        {
            if (!int.TryParse(arguments[i],
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out int pointIndex)) return;
            if (!IsPointIndexInbounds(pointIndex)) return;

            pointIndices.Add(pointIndex - 1);
        }

        FanTriangulation(pointIndices);
    }

    /// <summary>
    /// Takes vertex indices and creates and adds <see cref="Triangle"/>s
    /// based on them. With more than three indices polygons are triangulated.
    /// </summary>
    private void FanTriangulation(List<int> pointIndices)
    {
        for (int i = 1; i < pointIndices.Count - 1; i++)
        {
            Triangle triangle = new(Vertices[pointIndices[0]], Vertices[pointIndices[i]], Vertices[pointIndices[i + 1]]);
            Group.AddChild(triangle);
        }
    }

    private bool IsPointIndexInbounds(int pointIndex) => pointIndex >= 1 && pointIndex <= Vertices.Count;
}
