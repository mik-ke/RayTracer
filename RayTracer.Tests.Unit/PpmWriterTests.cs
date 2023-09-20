using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Utilities;
namespace RayTracer.Tests.Unit;

public class PpmWriterTests
{
	[Fact]
	public async Task GetAsPpmString_ShouldHaveCorrectHeader()
	{
		// Arrange
		const int width = 5;
		const int height = 3;
		string expectedHeader = 
@$"P3
{width} {height}
255";
		Canvas canvas = new(width, height);
		PpmWriter writer = new();

		// Act
		string ppm = await canvas.GetAsPpmString(writer);
		string[] ppmByLine = ppm.Split(Environment.NewLine);
		string actualHeader = string.Join(Environment.NewLine, ppmByLine[0..3]);

		// Assert
		Assert.Equal(expectedHeader, actualHeader);
	}

	[Fact]
	public async Task GetAsPpmString_ShouldHaveClampedPixelData_WhenColorValueOutsideScaleBounds()
	{
		// Arrange
		PpmWriter writer = new();
		Canvas canvas = new(5, 3);
		Color c1 = new(1.5, 0, 0);
		Color c2 = new(0, 0.5, 0);
		Color c3 = new(-0.5, 0, 1);
		canvas[0, 0] = c1;
		canvas[2, 1] = c2;
		canvas[4, 2] = c3;
		const string expectedPixelData =
@"255 0 0 0 0 0 0 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 128 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0 0 0 0 0 0 0 255";

		// Act
		string ppm = await canvas.GetAsPpmString(writer);
		string[] ppmByLine = ppm.Split(Environment.NewLine);
		string actualPixelData = string.Join(Environment.NewLine, ppmByLine[3..6]);

		// Assert
		Assert.Equal(expectedPixelData, actualPixelData);
	}

	[Fact]
	public async void GetAsPpmString_ShouldSplitPixelData_WhenLinesLongerThanMaxLineLength()
	{
		// Arrange
		PpmWriter writer = new();
		Canvas canvas = new(10, 2, new Color(1, 0.8, 0.6));
		const string expectedPixelData =
@"255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204
153 255 204 153 255 204 153 255 204 153 255 204 153";

		// Act
		string ppm = await canvas.GetAsPpmString(writer);
		string[] ppmByLine = ppm.Split(Environment.NewLine);
		string actualPixelData = string.Join(Environment.NewLine, ppmByLine[3..7]);

		// Assert
		Assert.Equal(expectedPixelData, actualPixelData);
	}

	[Fact]
	public async void GetAsPpmString_ShouldTerminateWithNewLineCharacter()
	{
		// Arrange
		PpmWriter writer = new();
		Canvas canvas = new(5, 3);

		// Act
		string ppm = await canvas.GetAsPpmString(writer);

		// Assert
		Assert.EndsWith(Environment.NewLine, ppm);
	}
}
