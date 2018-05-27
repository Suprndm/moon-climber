using System.Collections.Concurrent;
using System.Collections.Generic;
using MoonClimber.Maths;
using MoonClimber.Physics.Forces;
using Xamarin.Forms;

namespace MoonClimber.Physics
{
    public interface IPhysicalView
    {
        ConcurrentBag<Force> Forces { get; set; }

        int Mass { get; set; }

        Vector A { get; set; }
        Vector V { get; set; }
        double RealX { get; set; }
        double RealY { get; set; }
        void ApplyForce(Force force);

        Vector ControlledMovement { get; set; }


        Vector Movement { get; set; }

        IList<Point> CollisionPoints { get; }

        Point ComputeNextPosition();

        bool CanBounce { get; }
        bool IsInAir { get; set; }
    }
}
