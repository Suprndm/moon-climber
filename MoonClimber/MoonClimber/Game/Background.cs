using MoonClimber.Game.Sprites;
using Odin.Core;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Game
{
    public class Background : OView
    {
        public Background()
        {
            var sprite = new Sprite(
                SpriteConst.Background, ORoot.ScreenWidth / 2, 
                ORoot.ScreenHeight / 2, 
                ORoot.ScreenWidth, 
                ORoot.ScreenHeight,
                new SKPaint {Color = CreateColor(255, 255, 255)});

            AddChild(sprite);
        }
    }
}
