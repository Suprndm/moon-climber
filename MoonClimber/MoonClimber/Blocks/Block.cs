using MoonClimber.Data.ChunkData;
using MoonClimber.Game.Sprites;
using Odin.Core;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Blocks
{
    public class Block : OView
    {
        private Sprite _sprite;
        private float _angle;
        public BlockData Data { get; set; }
        public BlockType Type { get; set; }


        public Block(float height, float width) : base(0, 0, height, width)
        {
            IsEnabled = false;
            _sprite = new Sprite(SpriteConst.rock_block_1, 0, 0, Width, Width, new SKPaint { Color = new SKColor(255, 255, 255) });
            AddChild(_sprite);
        }

        public void Setup(BlockData blockData, float x, float y, BlockType blockType)
        {
            Data = blockData;
            Type = blockType;

            IsEnabled = true;

            X = x;
            Y = y;

            var blockSetup = BlockVisualHelper.GetVisualSetup(blockData);

            _sprite.UpdateSprite(BlockVisualHelper.GenerateSpriteName(blockType, blockSetup));
            _sprite.Angle = (float)blockSetup.Angle;
        }

        public void UpdateData(BlockData blockData)
        {
            var blockSetup = BlockVisualHelper.GetVisualSetup(blockData);

            _sprite.UpdateSprite(BlockVisualHelper.GenerateSpriteName(Type, blockSetup));
            _sprite.Angle = (float)blockSetup.Angle;
        }

        public void Recycle()
        {
            Data = null;
            IsEnabled = false;
        }

        public bool IsRecycled()
        {
            return !IsEnabled;
        }

        public override void Render()
        {
        }
    }
}
