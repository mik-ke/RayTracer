using RayTracer.Models;
using RayTracer.Patterns;

namespace RayTracer.Tests.Unit;

public class RingPatternTests
{
	[Theory]
	[MemberData(nameof(PatternAtData))]
	public void PatternAt_ShouldExtendInBothXAndZ(Point point, Color expected)
	{
		// Arrange
		RingPattern pattern = new(Color.White, Color.Black);

		// Act
		var actual = pattern.PatternAt(point);

		// Assert
		Assert.Equal(expected, actual);
	}
	public static IEnumerable<object[]> PatternAtData =>
		new List<object[]> {
            new object[] { new Point(0, 0, 0), Color.White },
            new object[] { new Point(1, 0, 0), Color.Black },
            new object[] { new Point(0, 0, 1), Color.Black },
			// 0.708 = slightly more than sqrt(2) / 2
            new object[] { new Point(0.708, 0, 0.708), Color.Black }
		};
}
