using RayTracer.Utilities;
using System.Collections;

namespace RayTracer.Models;

/// <summary>
/// <see cref="Canvas"/> represents a width times height structure containing <see cref="Color"/>s in each pixel.
/// </summary>
public sealed class Canvas : IEnumerable<Color>
{
    #region fields
    readonly Color[,] _canvas;
    #endregion

    #region properties
    public double Width { get; private set; }
    public double Height { get; private set; }
    #endregion

    /// <summary>
    /// Creates a <see cref="Canvas"/> with the given <paramref name="width"/> and <paramref name="height"/>.
    /// Initializes each pixel as black (0, 0, 0).
    /// </summary>
    public Canvas(int width, int height) : this(width, height, Color.Black)
    {
    }

    /// <summary>
    /// Creates a <see cref="Canvas"/> with the given <paramref name="width"/> and <paramref name="height"/>.
    /// Initializes each pixel as the given <paramref name="color"/>.
    /// </summary>
    public Canvas(int width, int height, Color color)
    {
        Width = width;
        Height = height;

        _canvas = new Color[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                _canvas[x, y] = color;
    }

    public Color this[int x, int y]
    {
        get => PixelAt(x, y);
        set => WritePixel(x, y, value);
    }

    /// <summary>
    /// Writes the given <paramref name="color"/> to the given (<paramref name="x"/>, <paramref name="y"/>) coordinate.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> or <paramref name="y"/> is out of bounds.</exception>
    public void WritePixel(int x, int y,  Color color)
    {
        if (x < 0 || x >=  Width)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));

        _canvas[x, y] = color;
    }

    /// <summary>
    /// Returns the <see cref="Color"/> at the given (<paramref name="x"/>, <paramref name="y"/>) coordinate.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="x"/> or <paramref name="y"/> is out of bounds.</exception>
    public Color PixelAt(int x, int y)
    {
        if (x < 0 || x >=  Width)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));

        return _canvas[x, y];
    }

    #region IEnumerable
    public IEnumerator<Color> GetEnumerator()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                yield return _canvas[x, y];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Task SaveToPpmFileAsync(PpmWriter writer, string v)
    {
        throw new NotImplementedException();
    }
    #endregion
}
