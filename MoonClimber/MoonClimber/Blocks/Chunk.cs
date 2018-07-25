using System;
using System.Collections.Generic;
using System.Text;
using MoonClimber.Data.ChunkData;
using Odin.Containers;
using Odin.Core;
using Odin.Services;
using SkiaSharp;

namespace MoonClimber.Blocks
{
    public class Chunk : OView
    {
        private readonly SKImage _chunkImage;
        private readonly SKPaint _paint;

        public ChunkData ChunkData { get; set; }
        private readonly Logger _logger;

        public Chunk(ChunkData chunkData) : base(
           (float)chunkData.TopLeft.X,
           (float)chunkData.TopLeft.Y,
            chunkData.Size * AppSettings.BlockSizeU * ORoot.ScreenUnit,
            chunkData.Size * AppSettings.BlockSizeU * ORoot.ScreenUnit)
        {
            var blockSpriteProvider = GameServiceLocator.Instance.Get<BlockSpriteProvider>();
            _logger = GameServiceLocator.Instance.Get<Logger>();

            ChunkData = chunkData;

            _paint = new SKPaint() { Color = new SKColor(255, 255, 255) };

            var surface = SKSurface.Create(
                (int)Width,
                (int)Height,
                SKColorType.Rgba8888,
                SKAlphaType.Unpremul);

            var canvas = surface.Canvas;

            int blockCount = 0;

            foreach (var block in chunkData.Blocks)
            {
                var sprite = blockSpriteProvider.GetBlockSprite(BlockType.rock_block, block);
                sprite.X = block.RelativeX * AppSettings.BlockSizeU * ORoot.ScreenUnit + sprite.Width / 2;
                sprite.Y = block.RelativeY * AppSettings.BlockSizeU * ORoot.ScreenUnit + sprite.Height / 2;

                sprite.SetCanvas(canvas);
                sprite.Render(new OViewState());

                blockCount++;
            }

            _logger.Log($"block at {chunkData.X}-{chunkData.Y} draw {blockCount} sprites");

            _y -= AppSettings.BlockSizeU * ORoot.ScreenUnit / 2;
            _x -= AppSettings.BlockSizeU * ORoot.ScreenUnit / 2;

            _chunkImage = surface.Snapshot();

            surface.Dispose();
        }

        public override void Render()
        {
            Canvas.DrawImage(_chunkImage, X, Y, _paint);
        }

    }
}
