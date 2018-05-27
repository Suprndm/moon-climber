using System;
using System.Linq;
using MoonClimber.Game.Sprites;
using MoonClimber.Physics;
using MoonClimber.Physics.Forces;
using Odin.Services;
using Odin.Sprites;
using SkiaSharp;
using Xamarin.Forms;
using Rectangle = Odin.Shapes.Rectangle;

namespace MoonClimber.Game.Characters
{
    public class Character : PhysicalView
    {
        private readonly Force _movementForce;
        private readonly Sprite _haloSprite;
        private readonly Rectangle _body;
        private readonly Logger _logger;
        public Character(float x, float y, float width, float height) : base(x, y, width, height)
        {
            CollisionPoints.Add(new Point(-Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, Height / 2));
            CollisionPoints.Add(new Point(-Width / 2, Height / 2));

            CollisionPoints.Add(new Point( -Width / 2, 0));
            CollisionPoints.Add(new Point(Width / 2, 0));

            CanBounce = false;
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _movementForce = new Force(0, 0, new HumanForceType(), -1);
            ApplyForce(_movementForce);
            var haloPaint = new SKPaint() {BlendMode = SKBlendMode.Overlay, Color = CreateColor(255, 255, 255, 150)};
            _haloSprite = new Sprite(SpriteConst.WhiteHalo, 0, 0, height * 32, height * 32, haloPaint);
            AddChild(_haloSprite);
            int index = 0;

            //Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            //{
            //    var values = (Enum.GetValues(typeof(SKBlendMode)).Cast<SKBlendMode>()).ToList();
            
            //    if (index >= values.Count)
            //        index = 0;
            
            
            //    haloPaint.BlendMode = values[index];
            //    _logger.UpdatePermanentText1(haloPaint.BlendMode.ToString());
            //    index++;
            //    return true;
            //});

    

            var bodyPaint = new SKPaint()
            {
                IsAntialias = true,
                Color = CreateColor(255, 0, 0),
            };

                _body = new Rectangle( -width/2,-height/2,width, height, bodyPaint);
            AddChild(_body);

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
    }
}
