namespace RayTracer.Models;

public sealed class Camera
{
    #region properties
    /// <summary>
    /// Horizontal size (in pixels) of the canvas the picture will be rendered to.
    /// </summary>
    public int HorizontalSize { get; set; }

    /// <summary>
    /// Vertical size (in pixels) of the canvas the picture will be rendered to.
    /// </summary>
    public int VerticalSize { get; set; }

    /// <summary>
    /// The angle that describes how much the <see cref="Camera"/> can see.
    /// </summary>
    public double FieldOfView { get; set; }

    /// <summary>
    /// Matrix describing how the world is oriented relative to the <see cref="Camera"/>.
    /// </summary>
    public Matrix Transform { get; set; }

    /// <summary>
    /// The size of a single pixel in world space.
    /// </summary>
    public double PixelSize { get; private set; }

    /// <summary>
    /// Half the width of the canvas in world space.
    /// </summary>
    public double HalfWidth { get; private set; }

    /// <summary>
    /// Half the height of the canvas in world space.
    /// </summary>
    public double HalfHeight { get; private set; }
    #endregion

    public Camera(int horizontalSize, int verticalSize, double fieldOfView, Matrix? transform = null)
    {
        HorizontalSize = horizontalSize;
        VerticalSize = verticalSize;
        FieldOfView = fieldOfView;
        Transform = transform ?? Matrix.Identity(4);
        CalculatePixelSize();
    }

    /// <summary>
    /// Initializes <see cref="PixelSize"/>, <see cref="HalfWidth"/> and <see cref="HalfHeight"/>.
    /// </summary>
    private void CalculatePixelSize()
    {
        double halfView = Math.Tan(FieldOfView / 2);
        double aspect = (double)HorizontalSize / VerticalSize;
        if (aspect >= 1)
        {
            HalfWidth = halfView;
            HalfHeight = halfView / aspect;
        }
        else
        {
            HalfWidth = halfView * aspect;
            HalfHeight = halfView;
        }

        PixelSize = (HalfWidth * 2) / HorizontalSize;
    }

    /// <summary>
    /// Calculates the world coordinates at the center of the given pixel
    /// and constructs a <see cref="Ray"/> that passes through that point
    /// </summary>
    /// <returns>A new <see cref="Ray"/> that passes through the pixel center</returns>
    public Ray RayForPixel(int pixelX, int pixelY)
    {
        // the offset from the edge of the canvas to the pixel's center
        double xOffset = (pixelX + 0.5) * PixelSize;
        double yOffset = (pixelY + 0.5) * PixelSize;

        double worldX = HalfWidth - xOffset;
        double worldY = HalfHeight - yOffset;

        var pixel = (Point)(Transform.Inverse() * new Point(worldX, worldY, -1));
        var origin = (Point)(Transform.Inverse() * new Point(0, 0, 0));
        var direction = (pixel - origin).Normalize();

        return new Ray(origin, direction);
    }

    /// <summary>
    /// Renders the given <paramref name="world"/>.
    /// </summary>
    /// <returns>A new <see cref="Canvas"/>.</returns>
    public Canvas Render(World world)
    {
        Canvas image = new(HorizontalSize, VerticalSize);

        for (int y = 0 ; y < VerticalSize; y++)
        {
            for (int x = 0 ; x < HorizontalSize; x++)
            {
                Ray ray = RayForPixel(x, y);
                Color color = world.ColorAt(ray);
                image[x, y] = color;
            }
        }

        return image;
    }
}
