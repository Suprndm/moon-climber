using System.Collections.Generic;
using System.Drawing;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks.Services
{
    public interface IChunkDataProvider
    {
        IList<ChunkData> GetClosestChunks(int x, int y);

        ChunkData GetCurrentChunk(int x, int y);

        ChunkData GetByCoordinates(int chunkX, int chunkY);

        Point GetSpawnPosition();
    }
}
