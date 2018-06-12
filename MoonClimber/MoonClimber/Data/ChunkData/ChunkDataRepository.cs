using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoonClimber.Blocks.Models;
using MoonClimber.Blocks.Services;
using MoonClimber.Game.Sprites;
using Odin.Core;
using Odin.Services;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Data.ChunkData
{
    public class ChunkDataRepository : IChunkDataRepository
    {
        protected IList<ChunkData> Data;
        private Point _spawnPosition;
        private readonly Logger _logger;

        public ChunkDataRepository(Logger logger)
        {
            _logger = logger;
            Initialize();
        }

        public ChunkData Get(int id)
        {
            return null;
        }

        public IList<ChunkData> GetAll()
        {
            return Data;
        }

        public int Count()
        {
            return Data.Count;
        }

        public Point GetSpawnPosition()
        {
            return _spawnPosition;
        }

        private IList<BlockData> LoadBlocksFromImage(string fileName)
        {
            var blocks = new List<BlockData>();

            try
            {

                var spriteData = SpriteLoader.Instance.GetData(fileName);
                var height = (int)spriteData.Bounds.Height;
                var width = (int)spriteData.Bounds.Width;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixelColor = spriteData.Bitmap.GetPixel(i, j);
                        if (IsWhite(pixelColor))
                        {
                            var block = new BlockData() { X = i, Y = j - height };
                            blocks.Add(block);
                        }
                        else if (IsRed(pixelColor))
                        {
                            _spawnPosition = new Point(i, j - height);
                        }
                    }
                }

                _logger.Log($"Loaded {blocks.Count} blocks");
            }
            catch (Exception e)
            {
                _logger.Log($"Unable to load blocks from {fileName} - {e.Message}");
            }


            return blocks;
        }

        private IList<ChunkData> StoreBlocksInChunks(IList<BlockData> blocks)
        {
            var chunks = new List<ChunkData>();
            var chunkSize = AppSettings.ChunckSizeU * ORoot.U;
            foreach (var block in blocks)
            {
                var chunkX = (int)( block.X / chunkSize);
                var chunkY = (int)(block.Y / chunkSize);

                var correspondingChunk = chunks.FirstOrDefault(chunk => chunk.X == chunkX && chunk.Y == chunkY);
                if (correspondingChunk == null)
                {
                    correspondingChunk = new ChunkData(0, chunkX, chunkY);
                    chunks.Add(correspondingChunk);
                }

                block.X += -(int)(correspondingChunk.X *AppSettings.ChunckSizeU * ORoot.U);
                block.Y += -(int)(correspondingChunk.Y * AppSettings.ChunckSizeU * ORoot.U);

                correspondingChunk.Blocks.Add(block);
            }

            _logger.Log($"Built {chunks.Count} chunks ");

            return chunks;
        }

        private void Initialize()
        {

            var blocks = LoadBlocksFromImage(SpriteConst.BaseMap);

            var chunks = StoreBlocksInChunks(blocks);

            var completeMapData = new MapData(chunks);

            completeMapData.ComputeNeighbours(); 

            Data = chunks;
        }


        private bool IsWhite(SKColor color)
        {
            return color.Red == 255
                   && color.Blue == 255
                  && color.Green == 255;
        }

        private bool IsRed(SKColor color)
        {
            return color.Red == 255
                   && color.Blue == 0
                   && color.Green == 0;
        }

    }
}
