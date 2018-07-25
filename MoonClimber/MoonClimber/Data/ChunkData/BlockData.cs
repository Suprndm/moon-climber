using System;
using MoonClimber.Blocks;
using Newtonsoft.Json;

namespace MoonClimber.Data.ChunkData
{
    public class BlockData
    {
        public int RelativeX { get; set; }
        public int RelativeY { get; set; }

        public int AbsoluteX { get; set; }
        public int AbsoluteY { get; set; }

        public BlockType Type { get; set; }

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
