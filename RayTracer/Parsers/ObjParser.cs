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
    public List<Vector> Normals = new(0);

    /// <summary>
    /// Represents the named groups of the OBJ file.
    /// </summary>
    public readonly Dictionary<string, Group> NamedGroups = new();
    /// <summary>
    /// Represents the default group of the OBJ file.
    /// </summary>
    public Group DefaultGroup = new();

    /// <summary>
    /// Parses the OBJ-formatted <paramref name="objData"/> data and initializes the <see cref="Obj"/>.
    /// </summary>
    public async Task LoadFromStringAsync(string objData)
    {
        using StringReader reader = new(objData);
        await LoadFromTextReaderAsync(reader);
    }

    /// <summary>
    /// Parses the OBJ-formatted file in <paramref name="objFilePath"/> and initialized the <see cref="Obj"/>.
    /// </summary>
    public async Task LoadFromFileAsync(string objFilePath)
    {
        using StreamReader reader = new(objFilePath);
        await LoadFromTextReaderAsync(reader);
    }

    public async Task LoadFromTextReaderAsync(TextReader reader)
    {
        ResetValues();
        string? currentLine;
        while ((currentLine = await reader.ReadLineAsync()) != null)
        {
            ProcessLine(currentLine);
        }
    }

    /// <summary>
    /// Creates a <see cref="Group"/> based on the parsed OBJ string.
    /// </summary>
    /// <remarks>
    /// see cref="LoadFromStringAsync(string)"/> or <see cref="LoadFromFileAsync(string)"/> should be called before calling this.
    /// </remarks>
    public Group ToGroup()
    {
        Group group = new();
        foreach (Shape child in DefaultGroup)
            group.AddChild(child);
        foreach (Group namedGroup in NamedGroups.Values)
            group.AddChild(namedGroup);

        return group;
    }

    private string? _currentGroupName;

    private void ProcessLine(in string line)
    {
        var arguments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (arguments.Length == 0) return;

        switch (arguments[0])
        {
            case "v":
                ProcessVertex(arguments);
                break;
            case "vn":
                ProcessNormal(arguments);
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

    private void ProcessNormal(string[] arguments)
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

        Vector normal = new(x, y, z);
        Normals.Add(normal);
    }

    private void ProcessFace(in string[] arguments)
    {
        if (arguments.Length < 4) return;

        List<int> pointIndices = new();
        List<int>? normalIndices = null;
        for (int i = 1; i  < arguments.Length; i++)
        {
            string[] parts = arguments[i].Split('/');
            
            if (!int.TryParse(parts[0],
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out int pointIndex)) return;
            if (!IsPointIndexInbounds(pointIndex)) return;
            pointIndices.Add(pointIndex - 1);

            if (parts.Length <= 2) continue;
            
            if (!int.TryParse(parts[2],
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out int normalIndex)) return;
            if (!IsNormalIndexInbounds(normalIndex)) return;
            normalIndices ??= new();
            normalIndices.Add(normalIndex - 1);
        }
        
        if (normalIndices?.Count > 0 && pointIndices.Count != normalIndices.Count)
            throw new ArgumentException("The number of normals and points must be the same.");

        FanTriangulation(pointIndices, normalIndices);
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
    private void FanTriangulation(List<int> pointIndices, List<int>? normalIndices)
    {
        for (int i = 1; i < pointIndices.Count - 1; i++)
        {
            Triangle triangle;
            if (normalIndices is not null)
            {
                triangle = new SmoothTriangle(
                    Vertices[pointIndices[0]], Vertices[pointIndices[i]], Vertices[pointIndices[i + 1]],
                    Normals[normalIndices[0]], Normals[normalIndices[i]], Normals[normalIndices[i + 1]]);
            }
            else
            {
                triangle = new(Vertices[pointIndices[0]], Vertices[pointIndices[i]], Vertices[pointIndices[i + 1]]);
            }
            
            if (_currentGroupName != null)
            {
                if (!NamedGroups.TryGetValue(_currentGroupName, out Group? currentGroup))
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
    private bool IsNormalIndexInbounds(int normalIndex) => normalIndex >= 1 && normalIndex <= Normals.Count;

    private void ResetValues()
    {
        Vertices = new();
        Normals = new();
        DefaultGroup = new();
        _currentGroupName = null;
    }
}
