using System;
using System.Collections.Generic;
using System.Text;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks.Models
{
    public class MapDataUpdate
    {
        public MapDataUpdate(IList<ChunkData> loadedChunks, IList<ChunkData> unloadedChunks)
        {
            LoadedChunks = loadedChunks;
            UnloadedChunks = unloadedChunks;
        }

        public IList<ChunkData> LoadedChunks { get;  }
        public IList<ChunkData> UnloadedChunks { get; }
    }
}
