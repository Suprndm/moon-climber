using System.Collections.Generic;
using Odin.Core;
using Xamarin.Forms;

namespace MoonClimber.Data.ChunkData
{
    public class ChunkData
    {
        public ChunkData(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
            Blocks = new List<BlockData>();
            Center = new Point((X+0.5)*AppSettings.ChunckSizeU * ORoot.U, (Y + 0.5) * AppSettings.ChunckSizeU * ORoot.U);
            TopLeft = new Point(X*AppSettings.ChunckSizeU * ORoot.U, Y * AppSettings.ChunckSizeU * ORoot.U);
        }

        public int Id { get;  }
        public int X { get;  }
        public int Y { get; }
        public IList<BlockData> Blocks { get; }
        public Point Center { get; }
        public Point TopLeft { get; }
    }
}
