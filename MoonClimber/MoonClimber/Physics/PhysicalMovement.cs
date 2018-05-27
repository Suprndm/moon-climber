using System.Collections.Generic;
using Odin.Maths;
using Xamarin.Forms;

namespace MoonClimber.Physics
{
    public class PhysicalMovement
    {
        public PhysicalMovement(Point initialPos, Point finalPos, IList<Point> corners)
        {
            InitialPos = initialPos;
            FinalPos = finalPos;
            Corners = corners;

            InitialCornersPos = new List<Point>();
            FinalCornersPos = new List<Point>();
            CornersMovements = new List<Segment>();

            foreach (var corner in Corners)
            {
                   var initialCornerPos = new Point(InitialPos.X + corner.X, InitialPos.Y + corner.Y);
                   var finalCornerPos = new Point(FinalPos.X + corner.X, FinalPos.Y + corner.Y);
                InitialCornersPos.Add(initialCornerPos);
                FinalCornersPos.Add(finalCornerPos);

                CornersMovements.Add(new Segment(initialCornerPos, finalCornerPos));
            }
        }

        public Point InitialPos { get;  }
        public Point FinalPos { get;  }
        public IList<Point> Corners { get; }
        public IList<Point> InitialCornersPos { get;}
        public IList<Point> FinalCornersPos { get;  }
        public IList<Segment> CornersMovements { get; }
    }
}
