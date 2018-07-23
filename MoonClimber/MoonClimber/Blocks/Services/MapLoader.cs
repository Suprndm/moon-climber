using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoonClimber.Blocks.Models;
using MoonClimber.Data.ChunkData;
using Odin.Core;
using Odin.Maths;
using Odin.Services;

namespace MoonClimber.Blocks.Services
{
    public class MapLoader : IMapLoader
    {
        private readonly ChunkDataProvider _chunkDataProvider;
        private ChunkData _lastCurrentChunk;
        private readonly Logger _logger;

        public MapLoader(ChunkDataProvider chunkDataProvider)
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _chunkDataProvider = chunkDataProvider;
        }

        public MapDataUpdate ActualizeMap(int x, int y, MapData mapData)
        {
            var currentChunk = _chunkDataProvider.GetCurrentChunk(x, y);
            var loadedChunks = new List<ChunkData>();
            var unloadedChunks = new List<ChunkData>();

            if (currentChunk == null) return new MapDataUpdate(loadedChunks, unloadedChunks);
        

            try
            {

                var blockSize = ORoot.ScreenUnit * AppSettings.BlockSizeU;
                var refreshedChunks = mapData.Chunks.ToList();

                if (_lastCurrentChunk != currentChunk)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            var xReference = (int)(x + (i - 4) * currentChunk.Size);
                            var yReference = (int)(y + (j - 4) * currentChunk.Size);

                            var chunk = _chunkDataProvider.GetCurrentChunk(xReference, yReference);

                            if (chunk != null)
                            {
                                var distance = MathHelper.Distance(new Xamarin.Forms.Point(x * blockSize, y * blockSize),
                                    new Xamarin.Forms.Point(chunk.Center.X, chunk.Center.Y));

                                if (mapData.Chunks.Contains(chunk) && distance >= 4 * currentChunk.Size * blockSize)
                                {
                                    unloadedChunks.Add(chunk);
                                    refreshedChunks.Remove(chunk);
                                }
                                else if (!mapData.Chunks.Contains(chunk) && distance <= 3 * currentChunk.Size*blockSize)
                                {
                                    loadedChunks.Add(chunk);
                                    refreshedChunks.Add(chunk);
                                }
                            }
                        }
                    }
                }

                mapData.Initialize(refreshedChunks);
                _logger.Log($"Map Actualization succeded");
            }
            catch (Exception e)
            {
                _logger.Log($"Map Actualization failed : {e.Message}");
            }

            return new MapDataUpdate(loadedChunks, unloadedChunks);

        }

        public Point GetSpawnPosition()
        {
            return _chunkDataProvider.GetSpawnPosition();
        }

        public MapData InitializeMap(int x, int y)
        {
            var currentChunk = _chunkDataProvider.GetCurrentChunk(x, y);

            var nbOfBlocksPerChunkRow = (int)Math.Sqrt(AppSettings.BlocksPerChunk);
            var chunkSize = AppSettings.BlockSizeU * nbOfBlocksPerChunkRow * ORoot.ScreenUnit;

            var chunks = new List<ChunkData>();
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        var xReference = (int)(x + (i - 3)* nbOfBlocksPerChunkRow);
                        var yReference = (int)(y + (j - 3)* nbOfBlocksPerChunkRow);

                        var chunk = _chunkDataProvider.GetCurrentChunk(xReference, yReference);
                        if (chunk != null)
                        {
                            chunks.Add(chunk);
                        }
                    }
                }

                _lastCurrentChunk = currentChunk;


            }
            catch (Exception e)
            {
                _logger.Log($"Map Initialization failed : {e.Message}");
            }

            _logger.Log($"Map Initialization succeded");

            return new MapData(chunks);
        }
    }
}
