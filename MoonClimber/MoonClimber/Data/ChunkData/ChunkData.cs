using System;
using System.Collections.Generic;
using Odin.Core;
using Xamarin.Forms;

namespace MoonClimber.Data.ChunkData
{
    public class ChunkData
    {

        public ChunkData(int id, int x, int y, int size)
        {
            Id = id;
            X = x;
            Y = y;
            Size = size;
            Blocks = new List<BlockData>();
            Center = new Point((X+0.5)* Size * AppSettings.BlockSizeU * ORoot.ScreenUnit, (Y + 0.5) * Size * AppSettings.BlockSizeU * ORoot.ScreenUnit);
            TopLeft = new Point(X * Size * AppSettings.BlockSizeU * ORoot.ScreenUnit, Y * Size * AppSettings.BlockSizeU * ORoot.ScreenUnit);
        }

        /// <summary>
        /// Number of blocks per row/column
        /// </summary>
        public int Size { get;}
        public int Id { get;  }
        public int X { get;  }
        public int Y { get; }
        public IList<BlockData> Blocks { get; }
        public Point Center { get; }
        public Point TopLeft { get; }

        public override string ToString()
        {
            return $"X: {X} Y: {Y} - TopLeft ({TopLeft.X}, {TopLeft.Y} Blocks:{Blocks.Count}";
        }
    }
}
