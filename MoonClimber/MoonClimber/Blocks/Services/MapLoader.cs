using System;
using System.Collections.Generic;
using System.Drawing;
using MoonClimber.Blocks.Models;
using MoonClimber.Data.ChunkData;
using Odin.Services;

namespace MoonClimber.Blocks.Services
{
    public class MapLoader : IMapLoader
    {
        private readonly ChunkDataProvider _chunkDataProvider;
        private bool _isInitialized;
        private ChunkData _lastCurrentChunk;
        private IList<BlockData> _preloadedBlocks;
        private readonly Logger _logger;

        public MapLoader(ChunkDataProvider chunkDataProvider)
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _chunkDataProvider = chunkDataProvider;
        }

        public MapData ActualizeMap(int x, int y)
        {
            if (!_isInitialized)
            {
                _preloadedBlocks = PreloadBlocks(x, y);
                _lastCurrentChunk = _chunkDataProvider.GetCurrentChunk(x, y);
                _isInitialized = true;
            }
            else
            {
                var newCurrentChunk = _chunkDataProvider.GetCurrentChunk(x, y);

                // If current chunk is not the center chunk of the loaded chunks, we reload the loaded chunks
                if (_lastCurrentChunk.X != newCurrentChunk.X || _lastCurrentChunk.Y != newCurrentChunk.Y)
                {
                    _preloadedBlocks = PreloadBlocks(x, y);
                    _lastCurrentChunk = newCurrentChunk;
                }
            }

            if (_preloadedBlocks.Count > 0)
            {
                var surroundingBlocksList = new List<BlockData>();

                var blockSize = GameRoot.ScreenWidth * AppSettings.BlockScreenRatioX;

                var surroundingBlocksXCount = (int)(AppSettings.MapPreloadedRatio * GameRoot.ScreenWidth / blockSize);

                var beginX = (int)(x - surroundingBlocksXCount / 2);
                var endX = (int)(x + surroundingBlocksXCount / 2);
                var beginY = (int)(y - surroundingBlocksXCount / 2);
                var endY = (int)(y + surroundingBlocksXCount / 2);

                foreach (var block in _preloadedBlocks)
                {
                    if (block.X >= beginX && block.X < endX && block.Y >= beginY && block.Y < endY)
                    {
                        surroundingBlocksList.Add(block);
                    }
                }

                return new MapData(surroundingBlocksList);
            }

            return new MapData();
        }

        public Point GetSpawnPosition()
        {
            return _chunkDataProvider.GetSpawnPosition();
        }


        private IList<BlockData> PreloadBlocks(int x, int y)
        {
            try
            {
                var loadedChunks = _chunkDataProvider.GetClosestChunks(x, y);
                var preloadedBlocks = new List<BlockData>();

                foreach (var chunk in loadedChunks)
                {
                    foreach (var block in chunk.Blocks)
                    {
                        preloadedBlocks.Add(new BlockData()
                        {
                            X = block.X + AppSettings.ChunckSize * chunk.X,
                            Y = block.Y + AppSettings.ChunckSize * chunk.Y,
                        });
                    }
                }

                // Compute Neighbours
                var tempMap = new MapData(preloadedBlocks);
                tempMap.ComputeNeighbours();

                _logger.Log($"Preloaded  {preloadedBlocks.Count} blocks");

                return tempMap.Blocks;
            }
            catch (Exception e)
            {
                _logger.Log($"Failed to Preloaded blocks for {x}-{y}");
                _logger.Log(e.ToString());
                throw;
            }
           
        }
    }
}
