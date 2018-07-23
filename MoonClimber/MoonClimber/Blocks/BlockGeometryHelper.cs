using System;
using System.Collections.Generic;
using MoonClimber.Blocks.Models;
using MoonClimber.Data.ChunkData;
using Odin.Maths;
using Xamarin.Forms;

namespace MoonClimber.Blocks
{
    public static class BlockGeometryHelper
    {
        public static IList<BlockData> GetAllBlocksCrossedBySegment(Segment s, MapData mapData, float blockSize)
        {
            var blocks = new List<BlockData>();
            var beginBlockX = (int)Math.Round(s.P1.X / blockSize);
            var beginBlockY = (int)Math.Round(s.P1.Y / blockSize);

            var endBlockX = (int)Math.Round(s.P2.X / blockSize);
            var endBlockY = (int)Math.Round(s.P2.Y / blockSize);
            if (beginBlockX > endBlockX)
            {
                var temp = beginBlockX;
                beginBlockX = endBlockX;
                endBlockX = temp;
            }

            if (beginBlockY > endBlockY)
            {
                var temp = beginBlockY;
                beginBlockY = endBlockY;
                endBlockY = temp;
            }

            for (int i = beginBlockX; i <= endBlockX; i++)
            {
                for (int j = beginBlockY; j <= endBlockY; j++)
                {
                    var block = mapData.GetBlockByCoordinates(i, j);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }
            }

            return blocks;
        }

        public static IList<Segment> GetBlockSegments(BlockData blockData, float blocksize)
        {
            var blockX = blockData.AbsoluteX;
            var blockY = blockData.AbsoluteY;

            var segments = new List<Segment>();

            var topLeftPoint = new Point((blockX - 0.5) * blocksize, (blockY - 0.5) * blocksize);
            var topRightPoint = new Point((blockX + 0.5) * blocksize, (blockY - 0.5) * blocksize);
            var bottomRightPoint = new Point((blockX + 0.5) * blocksize, (blockY + 0.5) * blocksize);
            var bottomLeftPoint = new Point((blockX - 0.5) * blocksize, (blockY + 0.5) * blocksize);

            segments.Add(new Segment(topLeftPoint, topRightPoint));
            segments.Add(new Segment(topRightPoint, bottomRightPoint));
            segments.Add(new Segment(bottomRightPoint, bottomLeftPoint));
            segments.Add(new Segment(bottomLeftPoint, topLeftPoint));

            return segments;
        }
    }
}
