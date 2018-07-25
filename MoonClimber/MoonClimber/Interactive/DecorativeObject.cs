using Odin.Core;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Interactive
{
    public class DecorativeObject : InteractiveObject
    {
        protected Sprite Sprite;
        private readonly string _spriteName;
        protected float SpriteWidthU;
        protected float SpriteHeightU;
        private readonly float _adjustmentXU;
        private readonly float _adjustmentYU;

        public DecorativeObject(string spriteName, float spriteWidthU, float spriteHeightU, float adjustmentXu, float adjustmentYu, int absoluteX, int absoluteY) : base(absoluteX, absoluteY)
        {
            _spriteName = spriteName;
            SpriteWidthU = spriteWidthU;
            SpriteHeightU = spriteHeightU;
            _adjustmentXU = adjustmentXu;
            _adjustmentYU = adjustmentYu;
        }

        public override void Load(float x, float y)
        {
            Sprite = new Sprite(
                _spriteName, 
                x+ _adjustmentXU * ORoot.ScreenUnit, 
                y+ _adjustmentYU * ORoot.ScreenUnit, 
                SpriteWidthU * ORoot.ScreenUnit, 
                SpriteHeightU * ORoot.ScreenUnit, 
                new SKPaint() { Color = CreateColor(255, 255, 255) });

            AddChild(Sprite);

            base.Load(x, y);
        }

        
    }
}
