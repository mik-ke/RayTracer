using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Utilities;

namespace ClockProgram.CLI
{
    internal class Program
    {
        static int size = 800;
        static int clockRadius = 300;
        static int canvasMidPoint = 400;
        static Color red = new(1, 0, 0);

        static async Task Main(string[] args)
        {
            Canvas canvas = new(size, size);
            canvas[canvasMidPoint, canvasMidPoint] = red;

            Point twelve = new(0, 1, 0);
            WritePointToCanvas(twelve, canvas);
            CreateClock(canvas, twelve);

            var writer = new PpmWriter();
            await canvas.SaveToPpmFileAsync(writer, "clock.ppm");
        }

        /// <summary>
        /// Creates a clock with the twelve point being the start.
        /// Then rotating pi / 6 along the Z axis (in inverse to move left to right, see left-hand rule)
        /// </summary>
        static void CreateClock(Canvas canvas, Point twelve)
        {
            for (byte b = 1; b < 12; b++)
            {
                Point clockPoint = (Point)(Matrix.RotationZ(b * Math.PI / 6).Inverse() * twelve);
                WritePointToCanvas(clockPoint, canvas);
            }
        }


        /// <summary>
        /// Marks the point on the canvas.
        /// The x and z components are multiplied by the radius
        /// and then moved to the center of the canvas.
        /// </summary>
        static void WritePointToCanvas(Point point, Canvas canvas)
        {
            int x = (int)Math.Round(point.X * clockRadius) + 400;
            int y = size - ((int)Math.Round(point.Y * clockRadius) + 400 + 1);
            canvas[x, y] = red;
        }
    }
}