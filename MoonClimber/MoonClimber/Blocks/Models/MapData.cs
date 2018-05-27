using System.Collections.Generic;
using System.Linq;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks.Models
{
    public class MapData
    {
        public MapData(IList<BlockData> blocks)
        {
            Blocks = blocks;

            if(!blocks.Any())
                return;

            MinX = blocks.Min(b => b.X);
            MaxX = blocks.Max(b => b.X);
            MinY = blocks.Min(b => b.Y);
            MaxY = blocks.Max(b => b.Y);

            Width = MaxX - MinX + 1;
            Height = MaxY - MinY + 1;

            BlocksArray = new BlockData[Width, Height];

            foreach (var blockData in Blocks)
            {
                BlocksArray[blockData.X - MinX, blockData.Y - MinY] = blockData;
            }

            Count = Blocks.Count;
        }

        public MapData()
        {
            Blocks = new List<BlockData>();

            MinX = 0;
            MaxX = 0;
            MinY = 0;
            MaxY = 0;

            Width = 0;
            Height = 0;
        }

        public IList<BlockData> Blocks { get; }
        public BlockData[,] BlocksArray { get; }

        public int Width { get; }
        public int Height { get; }

        public int MinX { get; }
        public int MaxX { get; }
        public int MinY { get; }
        public int MaxY { get; }

        public int Count { get; }

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
