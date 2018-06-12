using System;
using System.Collections.Generic;
using System.Text;
using MoonClimber.Data.ChunkData;
using MoonClimber.Game.Deblocks;
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
        public Chunk(ChunkData chunkData) : base(
            chunkData.X * ORoot.U * AppSettings.ChunckSizeU,
            chunkData.Y * ORoot.U * AppSettings.ChunckSizeU,
            ORoot.U * AppSettings.ChunckSizeU,
            ORoot.U * AppSettings.ChunckSizeU)
        {
            var blockSpriteProvider = GameServiceLocator.Instance.Get<BlockSpriteProvider>();

            ChunkData = chunkData;

            _paint = new SKPaint() {Color = new SKColor(255, 255, 255)};

            var surface = SKSurface.Create(
                (int)(ORoot.U * AppSettings.ChunckSizeU),
                (int)(ORoot.U * AppSettings.ChunckSizeU), 
                SKColorType.Rgba8888, 
                SKAlphaType.Unpremul);

            var rockBlockType = new RockBlockType();
            var canvas = surface.Canvas;

            foreach (var block in chunkData.Blocks)
            {
                var sprite = blockSpriteProvider.GetBlockSprite(rockBlockType, block);
                sprite.X = block.X - chunkData.X * Width;
                sprite.Y = block.Y - chunkData.Y * Height;

                sprite.SetCanvas(canvas);
                sprite.Render(new OViewState());
            }

            _chunkImage = surface.Snapshot();

            surface.Dispose();
        }

        public override void Render()
        {
            Canvas.DrawImage(_chunkImage, X, Y, _paint);
        }

    }
}
