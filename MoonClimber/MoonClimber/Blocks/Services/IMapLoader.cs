using System.Collections.Generic;
using System.Drawing;
using MoonClimber.Blocks.Models;

namespace MoonClimber.Blocks.Services
{
    public interface IMapLoader
    {
        MapDataUpdate ActualizeMap(int x, int y, MapData mapData);
        Point GetSpawnPosition();

        MapData InitializeMap(int x, int y);
    }
}
