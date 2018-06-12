using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoonClimber.Data.ChunkData;
using Odin.Core;
using Odin.Services;

namespace MoonClimber.Blocks.Services
{
    public class ChunkDataProvider: IChunkDataProvider
    {
        private readonly IChunkDataRepository _chunkDataRepository;
        private readonly Logger _logger;
        public ChunkDataProvider(IChunkDataRepository chunkDataRepository, Logger logger)
        {
            _chunkDataRepository = chunkDataRepository;
            _logger = logger;
        }

        public IList<ChunkData> GetClosestChunks(int x, int y)
        {
            var closestChunks = new List<ChunkData>();
            var currentChunk = GetCurrentChunk(x, y);

            if (currentChunk == null)
                return closestChunks;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var chunkX = currentChunk.X + i;
                    var chunkY = currentChunk.Y + j;

                    var correspondingChunk = GetByCoordinates(chunkX, chunkY);
                    if (correspondingChunk != null)
                    {
                        closestChunks.Add(correspondingChunk);
                    }
                }
            }

            return closestChunks;
        }

        public ChunkData GetCurrentChunk(int x, int y)
        {
            var chunkSize = AppSettings.ChunckSizeU * ORoot.U;

            var chunkX = (int)(x / chunkSize);
            var chunkY = (int)(y / chunkSize);
            var correspondingChunk = GetByCoordinates(chunkX, chunkY);

            if (correspondingChunk == null)
            {
                _logger.Log("Out of the MAP - no chunk data here");
            }
            else
            {
                _logger.Log($"Found Chunk for {x}-{y}");
            }

            return correspondingChunk;
        }

        public ChunkData GetByCoordinates(int chunkX, int chunkY)
        {
            return _chunkDataRepository.GetAll().FirstOrDefault(chunk => chunk.X == chunkX && chunk.Y == chunkY);
        }

        public Point GetSpawnPosition()
        {
            return _chunkDataRepository.GetSpawnPosition();
        }
    }
}
