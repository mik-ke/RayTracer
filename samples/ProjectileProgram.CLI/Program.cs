using ProjectileProgram.CLI.Models;
using RayTracer.Models;

// projectile starts one unit above the origin
// velocity is normalized to 1 unit/tick
var projectile = new Projectile(new Point(0, 1, 0), (new Vector(1, 1, 0).Normalize()));

// gravity -0.1 unit/tick, and wind is -0.01 unit/tick
var environment = new Environment(new Vector(0, -0.1, 0), new Vector(-0.01, 0, 0));

// tick until projectile hits the ground (i.e. Y == 0)
int numberOfTicks = 0;
while (projectile.Position.Y > 0)
{
    numberOfTicks++;
    System.Console.WriteLine(projectile.Position);
    projectile = Tick(environment, projectile);
}

// one tick moves the projectile depending according to the sum of its velocity
// and the environment's gravity and wind
static Projectile Tick(Environment environment, Projectile projectile)
{
    Point position = projectile.Position + projectile.Velocity;
    Vector velocity = projectile.Velocity + environment.Gravity + environment.Wind;
    return new Projectile(position, velocity);
}
