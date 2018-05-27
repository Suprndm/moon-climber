using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MoonClimber.Maths;
using MoonClimber.Physics.Forces;
using Odin.Core;
using Xamarin.Forms;

namespace MoonClimber.Physics
{
    /// <summary>
    /// Object that posess physical properties
    /// Each object minds its own physic
    /// An object only moves under effect of Forces
    /// In order to interact with object physics, use method ApplyForce
    /// See Force class for more details
    /// </summary>
    public abstract class PhysicalView : OView, IPhysicalView
    {

        protected PhysicalView(float x, float y, float height, float width) : base(x, y, height, width)
        {
            CollisionPoints = new List<Point>();
            Mass = 1;
            Friction = 0.95f;
            V = new Vector();
            Forces = new ConcurrentBag<Force>();
            PhysicalEngine.Instance.RegisterPhysicalElement(this);
            ForcedMovement = new Vector();

            ControlledMovement = new Vector();
            Movement = new Vector();
        }

        // Set a mass to an object
        // in Kg
        public int Mass { get; set; }

        // Set a friction to an object
        // friction constantly reduce applied speed vectors on object 
        // between 0 and 1
        public double Friction { get; set; }


        public Vector A { get; set; }

        public Vector V { get; set; }

        public Vector Movement { get; set; }

        public double RealX
        {
            get => _x;
            set => _x = (float)value;
        }

        public double RealY
        {
            get => _y;
            set => _y = (float)value;
        }



        public ConcurrentBag<Force> Forces { get; set; }


        public void ApplyForce(Force force)
        {
            Forces.Add(force);
        }

        public Vector ControlledMovement { get; set; }

        public Vector ForcedMovement { get; set; }

        public IList<Point> CollisionPoints { get; }

        public Point ComputeNextPosition()
        {
            // Update physical model
            foreach (var force in Forces)
            {
                force.Update();
            }

            // Remove expired forces to recycle data
            foreach (var force in Forces.Where(f => f.State == ForceState.Dead).ToList())
            {
                Force deletedForce;
                Forces.TryTake(out deletedForce);
            }

            var totalForce = new Vector();

            // Bilan des forces
            foreach (var force in Forces)
            {
                totalForce = totalForce + force;
            }

            A = totalForce / Mass;

            V = V + A;

            //if (IsInAir)
            //{
            //    Movement = Movement + ControlledMovement * 0.3f;
            //}
            //else
            //{
            //    Movement = Movement + ControlledMovement;
            //}

            return new Point(_x + V.X + Movement.X, _y + V.Y + Movement.Y);
        }

        public bool CanBounce { get; protected set; }
        public bool IsInAir { get; set; }
    }
}
