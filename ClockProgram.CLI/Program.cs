using RayTracer.Extensions;
using RayTracer.Models;
using RayTracer.Utilities;

namespace ClockProgram.CLI
{
    internal class Program
    {
        static int size = 800;
        static int clockRadius = 300;
        static Color red = new(1, 0, 0);
        static async Task Main(string[] args)
        {
            Canvas canvas = new(size, size);
            canvas[400, 400] = red;

            Point twelve = new(0, 1, 0);
            WritePointToCanvas(twelve, canvas);
            CreateClock(canvas, twelve);
            WriteThreePm(canvas);

            var writer = new PpmWriter();
            await canvas.SaveToPpmFileAsync(writer, "clock.ppm");
        }

        static void CreateClock(Canvas canvas, Point twelve)
        {
            for (byte b = 1; b < 12; b++)
            {
                Matrix clockPoint = Matrix.RotationZ(b * Math.PI / 6).Inverse() * twelve;
                WritePointToCanvas(new Point(clockPoint[0, 0], clockPoint[1, 0], 0), canvas);
            }
        }

        static void WriteThreePm(Canvas canvas)
        {
            for (int i = 0; i < 150; i++)
            {
                canvas[i + 250, 400] = red;
            }
            for (int i = 0; i < 275; i++)
            {
                canvas[400, i + 125] = red;
            }
        }

        /// <summary>
        /// Marks the point on the canvas
        /// </summary>
        static void WritePointToCanvas(Point point, Canvas canvas)
        {
            int x = (int)Math.Round(point.X * clockRadius) + 400;
            int y = size - ((int)Math.Round(point.Y * clockRadius) + 400 + 1);
            canvas[x, y] = red;
        }
    }
}