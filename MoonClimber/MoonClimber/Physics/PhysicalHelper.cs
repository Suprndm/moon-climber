using MoonClimber.Maths.Segments;
using Odin.Maths;
using Xamarin.Forms;

namespace MoonClimber.Physics
{
    public static class PhysicalHelper
    {
        public static bool GetCollisionPoint(Segment s1, Segment s2, out Collision collision )
        {
            collision = null;

            var v1 = new SVector(s1.P1.X, s1.P1.Y);
            var v2 = new SVector(s1.P2.X, s1.P2.Y);
            var v3 = new SVector(s2.P1.X, s2.P1.Y);
            var v4 = new SVector(s2.P2.X, s2.P2.Y);

           

            if (SegmentHelper.LineSegementsIntersect(v1, v2, v3, v4, out var collisionPoint))
            {
                var collisionPos =  new Point(collisionPoint.X, collisionPoint.Y);

                collision = new Collision(s1, s2, collisionPos);

                return true;
            }

            return false;
        }
    }
}
