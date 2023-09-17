using ProjectileProgram.CLI.Models;
using RayTracer.Models;
using RayTracer.Utilities;
using System.Threading.Tasks;

public class Program
{
    static int width = 900;
    static int height = 550;

    public static async Task Main(string[] args)
    {
        var canvas = new Canvas(width, height);

        // projectile starts one unit above the origin
        // velocity is normalized to 1 unit/tick then multiplied to increase magnitude
        var projectile = new Projectile(new Point(0, 1, 0), new Vector(1, 1.8, 0).Normalize() * 11.25);
        WritePointToCanvas(projectile.Position, canvas);

        // gravity -0.1 unit/tick, and wind is -0.01 unit/tick
        var environment = new Environment(new Vector(0, -0.1, 0), new Vector(-0.01, 0, 0));

        // tick until projectile hits the ground (i.e. Y == 0)
        int numberOfTicks = 0;
        while (projectile.Position.Y > 0)
        {
            numberOfTicks++;
            System.Console.WriteLine(projectile.Position);
            projectile = Tick(environment, projectile);
            WritePointToCanvas(projectile.Position, canvas);
        }

        // write ppm to file
        await SaveCanvasToPpmFile(canvas);
    }

    // one tick moves the projectile depending according to the sum of its velocity
    // and the environment's gravity and wind
    static Projectile Tick(Environment environment, Projectile projectile)
    {
        Point position = projectile.Position + projectile.Velocity;
        Vector velocity = projectile.Velocity + environment.Gravity + environment.Wind;
        return new Projectile(position, velocity);
    }

    // marks the point the projectile is in on the canvas
    static void WritePointToCanvas(Point point, Canvas canvas)
    {
        int x = (int)(System.Math.Round(point.X));
        int y = height - (int)(System.Math.Round(point.Y + 1));
        if (x < 0 || x >= width || y < 0 || y >= height) return;
        canvas[x, y] = Red;
    }
    static Color Red = new(1, 0, 0);

    // writes the canvas in PPM format to a file in the executing directory
    static async Task SaveCanvasToPpmFile(Canvas canvas)
    {
        var writer = new PpmWriter();
        using (var fileStream = System.IO.File.Create("projectile_path.ppm"))
        {
            await writer.WriteAsync(canvas, fileStream);
        }
    }
}