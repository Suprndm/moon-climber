using System;
using MoonClimber.Physics;
using MoonClimber.Physics.Forces;
using SkiaSharp;
using Xamarin.Forms;

namespace MoonClimber.Game.Characters
{
    public class Character : PhysicalView
    {
        private readonly Force _movementForce;
        public Character(float x, float y, float height, float width) : base(x, y, height, width)
        {
            CollisionPoints.Add(new Point(-Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, Height / 2));
            CollisionPoints.Add(new Point(-Width / 2, Height / 2));

            CollisionPoints.Add(new Point( -Width / 2, 0));
            CollisionPoints.Add(new Point(Width / 2, 0));

            CanBounce = false;

            _movementForce = new Force(0, 0, new HumanForceType(), -1);
            ApplyForce(_movementForce);
        }

        public void MoveTo(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public void Jump()
        {
            if (!IsInAir)
                ApplyForce(new Force(8, -Math.PI / 2, new ExplosiveForceType()));
        }

        public void MoveBy(float x, float y)
        {

            Movement.Amount = x;


            //X += x/100;
            // //_x += x;
            // //_y += y;

            //var angle = MathHelper.Angle(new SKPoint(_x, _y), new SKPoint(_x + x, _y + y));
            //var distance = MathHelper.Distance(new SKPoint(_x, _y), new SKPoint(_x + x, _y + y));
            //if (distance > 0)
            //{
            //    //Logger.Log(distance.ToString());
            //    ApplyForce(new Force(distance / 10, angle, new HumanForceType()));
            //}
        }

        public override void Render()
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = CreateColor(255, 0, 0);
                Canvas.DrawRect(SKRect.Create(X - Width / 2, Y - Height / 2, Width, Height), paint);
            }
        }
    }
}
