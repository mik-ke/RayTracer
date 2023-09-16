using RayTracer.Interfaces;
using RayTracer.Models;
using System.Text;

namespace RayTracer.Utilities;

public class PpmWriter : IPpmWriter
{
    private const string MAGICNUMBER = "P3";
    private const string MAXCOLORVALUE = "255";
    private const int MAXLINELENGTH = 70;
    public async Task WriteAsync(Canvas canvas, Stream stream)
    {
        using var writer = new StreamWriter(stream);

        await WriteHeaderAsync(canvas, writer);
        await WritePixelDataAsync(canvas, writer);
    }

    /// <summary>
    /// Writes the PPM header to the stream.
    /// </summary>
    private async Task WriteHeaderAsync(Canvas canvas, StreamWriter streamWriter)
    {
        await streamWriter.WriteLineAsync(MAGICNUMBER);
        await streamWriter.WriteLineAsync($"{canvas.Width} {canvas.Height}");
        await streamWriter.WriteLineAsync(MAXCOLORVALUE);
    }

    /// <summary>
    /// Writes the PPM pixel data to the stream.
    /// </summary>
    private async Task WritePixelDataAsync(Canvas canvas, StreamWriter streamWriter)
    {
        for (int y = 0; y < canvas.Height; y++)
        {
            int currentLineLength = 0;
            for (int x = 0; x < canvas.Width; x++)
            {
                Color color = canvas[x, y];
                var rString = GetScaledRGBValue(color.R).ToString();
                var gString = GetScaledRGBValue(color.G).ToString();
                var bString = GetScaledRGBValue(color.B).ToString();
                currentLineLength = await WriteColorComponentAsync(rString, streamWriter, currentLineLength, isFirstR: x == 0);
                currentLineLength = await WriteColorComponentAsync(gString, streamWriter, currentLineLength, isFirstR: false);
                currentLineLength = await WriteColorComponentAsync(bString, streamWriter, currentLineLength, isFirstR: false);
            }
            await streamWriter.WriteLineAsync();
        }
    }

    /// <summary>
    /// Writes the given color component to the stream. If <paramref name="currentLineLength"/> would go over <see cref="MAXLINELENGTH"/>
    /// writes the component to a new line, otherwise to the current line.
    /// Returns the line length after writing the color component.
    /// </summary>
    private async Task<int> WriteColorComponentAsync(string colorComponent, StreamWriter streamWriter, int currentLineLength, bool isFirstR)
    {
        int spaceNeeded = colorComponent.Length + (isFirstR ? 0 : 1);
        if (currentLineLength + spaceNeeded > MAXLINELENGTH)
        {
            await streamWriter.WriteLineAsync();
            currentLineLength = colorComponent.Length;
        }
        else if (!isFirstR)
        {
            await streamWriter.WriteAsync(" ");
            currentLineLength += spaceNeeded;
        }
        else
        {
            currentLineLength += spaceNeeded;
        }

        await streamWriter.WriteAsync(colorComponent);

        return currentLineLength;
    }

    /// <summary>
    /// Returns the canvas' R/G/B value scaled to 0-255.
    /// A scaled value greater than 255 is clamped to 255.
    /// A scaled value less than 0 is clamped to 0.
    /// </summary>
    private byte GetScaledRGBValue(double rgbValue)
    {
        if (rgbValue < 0) return 0;
        if (rgbValue > 1) return 255;
        return Convert.ToByte(rgbValue * 255);
    }

    /// <summary>
    /// Returns a <see cref="Canvas"/> converted to a string representation of a PPM format image
    /// </summary>
    public async Task<string> GetPpmStringAsync(Canvas canvas)
    {
        using var stream = new MemoryStream();
        await WriteAsync(canvas, stream);
        return Encoding.Default.GetString(stream.ToArray());
    }
}
