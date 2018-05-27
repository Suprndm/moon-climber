using System.Collections.Generic;

namespace MoonClimber.Data.ChunkData
{
    public class ChunkData
    {
        public ChunkData()
        {
            Blocks = new List<BlockData>();
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public IList<BlockData> Blocks { get; set; }
    }
}
