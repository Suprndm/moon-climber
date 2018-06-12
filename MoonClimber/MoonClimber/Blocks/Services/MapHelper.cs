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
                blockData.Left = mapData.GetBlockByCoordinates(blockData.X - 1, blockData.Y);
                blockData.Right = mapData.GetBlockByCoordinates(blockData.X + 1, blockData.Y);
                blockData.Top = mapData.GetBlockByCoordinates(blockData.X, blockData.Y + 1);
                blockData.Bottom = mapData.GetBlockByCoordinates(blockData.X, blockData.Y - 1);
            }
        }
    }
}
