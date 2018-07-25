using MoonClimber.Interactive;
using Odin.Data;

namespace MoonClimber.Data.Interactive
{
    public interface IInteractiveObjectRepository : IRepository<InteractiveObject>
    {
        void Initialize();
    }
}
