using RayTracer.Models;
using RayTracer.Shapes;
using System.Globalization;
using System.Threading.Tasks.Dataflow;

namespace RayTracer.Parsers;

/// <summary>
/// Class that represents the information of the Wavefront OBJ file format.
/// <see cref="LoadFromStringAsync(string)"/> should be called to initialize the object.
/// </summary>
public class Obj
{
    public List<Point> Vertices = new(0);

    /// <summary>
    /// Represents the named groups of the OBJ file.
    /// </summary>
    public Dictionary<string, Group> NamedGroups = new();
    /// <summary>
    /// Represents the default group of the OBJ file.
    /// </summary>
    public Group DefaultGroup = new();

    /// <summary>
    /// Parses the OBJ-formatted <paramref name="objData"/> data and initializes the <see cref="Obj"/>.
    /// </summary>
    public async Task LoadFromStringAsync(string objData)
    {
        Vertices = new();
        DefaultGroup = new();
        _currentGroupName = null;

        using (StringReader reader = new StringReader(objData))
        {
            string? currentLine;
            while ((currentLine = await reader.ReadLineAsync()) != null)
            {
                ProcessLine(currentLine);
            }
        }
    }

    /// <summary>
    /// Creates a <see cref="Group"/> based on the parsed OBJ string.
    /// </summary>
    /// <remarks><see cref="LoadFromStringAsync(string)"/> should be called before calling this.</remarks>
    public Group ToGroup()
    {
        Group group = new();
        foreach (var child in DefaultGroup)
            group.AddChild(child);
        foreach (var namedGroup in NamedGroups.Values)
            group.AddChild(namedGroup);

        return group;
    }

    private string? _currentGroupName = null;

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
                ProcessFace(arguments);
                break;
            case "g":
                ProcessGroup(arguments);
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

    private void ProcessFace(in string[] arguments)
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

    private void ProcessGroup(string[] arguments)
    {
        if (arguments.Length != 2) return;
        _currentGroupName = arguments[1];
    }

    /// <summary>
    /// Takes vertex indices and creates and adds <see cref="Triangle"/>s
    /// based on them. With more than three indices, polygons are triangulated.
    /// </summary>
    private void FanTriangulation(List<int> pointIndices)
    {
        for (int i = 1; i < pointIndices.Count - 1; i++)
        {
            Triangle triangle = new(Vertices[pointIndices[0]], Vertices[pointIndices[i]], Vertices[pointIndices[i + 1]]);
            if (_currentGroupName != null)
            {
                Group? currentGroup;
                if (!NamedGroups.TryGetValue(_currentGroupName, out currentGroup))
                {
                    NamedGroups[_currentGroupName] = currentGroup = new Group();
                }
                currentGroup.AddChild(triangle);
                continue;
            }

            DefaultGroup.AddChild(triangle);
        }
    }

    private bool IsPointIndexInbounds(int pointIndex) => pointIndex >= 1 && pointIndex <= Vertices.Count;
}
