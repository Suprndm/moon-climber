using System.Drawing;
using MoonClimber.Blocks.Models;

namespace MoonClimber.Blocks.Services
{
    public interface IMapLoader
    {
        MapData ActualizeMap(int x, int y);
        Point GetSpawnPosition();
    }
}
