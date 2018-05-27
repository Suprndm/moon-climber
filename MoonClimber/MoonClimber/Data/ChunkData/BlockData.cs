using Newtonsoft.Json;

namespace MoonClimber.Data.ChunkData
{
    public class BlockData
    {
        public int X { get; set; }
        public int Y { get; set; }

        [JsonIgnore]
        public BlockData Left { get; set; }

        [JsonIgnore]
        public BlockData Right { get; set; }

        [JsonIgnore]
        public BlockData Top { get; set; }

        [JsonIgnore]
        public BlockData Bottom { get; set; }

        public bool IsSpawnBlock { get; set; }
    }
}
