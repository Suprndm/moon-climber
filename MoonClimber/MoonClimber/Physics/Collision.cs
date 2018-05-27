using Odin.Maths;
using Xamarin.Forms;

namespace MoonClimber.Physics
{
    public class Collision
    {
        public Collision(Segment s1, Segment s2, Point point)
        {
            S1 = s1;
            S2 = s2;
            Point = point;

            var segment = new Segment(S1.P1, Point);
            DistanceToInitialS1 = segment.Distance;

            var refractionVector = new SVector( S1.P2.X - Point.X, S1.P2.Y- Point.Y);

            // vertical case
            if (s2.P1.X == s2.P2.X)
            {
                refractionVector.X = -refractionVector.X;
            }
            else
            {
                refractionVector.Y = -refractionVector.Y;
            }

            ReflectedPosition = new Point(point.X + refractionVector.X, point.Y + refractionVector.Y);
            var reflectionSegment = new Segment(Point, ReflectedPosition);
            ReflectionAngle = reflectionSegment.Angle;
        }

        public Segment S1 { get; }
        public Segment S2 { get; }
        public Point Point { get; }
        public double DistanceToInitialS1 { get; }
        public Point ReflectedPosition { get; set; }
        public double ReflectionAngle { get; set; }
    }
}
