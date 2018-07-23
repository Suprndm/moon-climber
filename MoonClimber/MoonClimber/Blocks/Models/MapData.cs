using System.Collections.Generic;
using System.Linq;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks.Models
{
    public class MapData
    {
        public MapData(IList<ChunkData> chunks)
        {
            Initialize(chunks);
        }

        public void Initialize(IList<ChunkData> chunks)
        {
            Chunks = chunks;

            Blocks = new List<BlockData>();
            foreach (var chunk in Chunks)
            {
                foreach (var block in chunk.Blocks)
                {
                    Blocks.Add(block);
                }
            }

            if (!Blocks.Any())
                return;

            MinX = Blocks.Min(b => b.AbsoluteX);
            MaxX = Blocks.Max(b => b.AbsoluteX);
            MinY = Blocks.Min(b => b.AbsoluteY);
            MaxY = Blocks.Max(b => b.AbsoluteY);

            Width = MaxX - MinX + 1;
            Height = MaxY - MinY + 1;

            BlocksArray = new BlockData[Width, Height];

            foreach (var blockData in Blocks)
            {
                BlocksArray[blockData.AbsoluteX - MinX, blockData.AbsoluteY - MinY] = blockData;
            }

            Count = Blocks.Count;
        }

        public MapData()
        {
            Chunks = new List<ChunkData>();
            Blocks = new List<BlockData>();

            MinX = 0;
            MaxX = 0;
            MinY = 0;
            MaxY = 0;

            Width = 0;
            Height = 0;
        }

        public IList<BlockData> Blocks { get; set; }
        public BlockData[,] BlocksArray { get; set; }
        public IList<ChunkData> Chunks { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public int Count { get; set; }

        public BlockData GetBlockByCoordinates(int x, int y)
        {
            var i = x - MinX;
            var j = y - MinY;

            return !IsInsideMap(x, y) ? null : BlocksArray[i, j];
        }

        public bool IsInsideMap(int x, int y)
        {
            var i = x - MinX;
            var j = y - MinY;

            if (i < 0 || i >= Width || j < 0 || j >= Height)
                return false;

            return true;
        }
    }
}
