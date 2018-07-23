using System.Collections.Generic;
using MoonClimber.Blocks.Models;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks.Services
{
    public static class MapHelper
    {
        public static void ComputeNeighbours(this MapData mapData)
        {
            foreach (var blockData in mapData.Blocks)
            {
                blockData.Left = mapData.GetBlockByCoordinates(blockData.AbsoluteX - 1, blockData.AbsoluteY);
                blockData.Right = mapData.GetBlockByCoordinates(blockData.AbsoluteX + 1, blockData.AbsoluteY);
                blockData.Top = mapData.GetBlockByCoordinates(blockData.AbsoluteX, blockData.AbsoluteY + 1);
                blockData.Bottom = mapData.GetBlockByCoordinates(blockData.AbsoluteX, blockData.AbsoluteY - 1);
            }
        }
    }
}
