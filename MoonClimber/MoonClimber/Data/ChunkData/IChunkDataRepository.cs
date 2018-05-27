using System.Drawing;
using Odin.Data;

namespace MoonClimber.Data.ChunkData
{
    public interface IChunkDataRepository : IRepository<ChunkData>
    {
        Point GetSpawnPosition();
    }
}
