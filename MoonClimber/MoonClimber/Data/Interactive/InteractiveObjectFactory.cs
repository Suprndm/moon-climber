using MoonClimber.Interactive;
using SkiaSharp;

namespace MoonClimber.Data.Interactive
{
    public static class InteractiveObjectFactory
    {
        public static InteractiveObject BuildFromColor(SKColor color, int absoluteX, int absoluteY)
        {
            if (color.Blue == 255)
            {
                return new TreeObject(absoluteX, absoluteY);
            }
            else
            {
                return null;
            }
        }
    }
}
