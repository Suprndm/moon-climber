using System;
using System.Linq;
using System.Threading.Tasks;
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
        private Force _movementForce;
        private Sprite _haloSprite;
        private Rectangle _body;
        private readonly Logger _logger;

        private bool _isTryingToJump;

        public Character(float x, float y, float width, float height) : base(x, y, width, height)
        {
            SetupPhysicalHitbox();
            SetupVisualDisplay();
            _logger = GameServiceLocator.Instance.Get<Logger>();
     
        }

        private void SetupVisualDisplay()
        {
            var haloPaint = new SKPaint() { BlendMode = SKBlendMode.Overlay, Color = CreateColor(255, 255, 255, 150) };
            _haloSprite = new Sprite(SpriteConst.WhiteHalo, 0, 0, Height * 32, Height * 32, haloPaint);
            AddChild(_haloSprite);
            int index = 0;

            var bodyPaint = new SKPaint()
            {
                IsAntialias = true,
                Color = CreateColor(255, 0, 0),
            };

            _body = new Rectangle(-Width / 2, -Height / 2, Width, Height, bodyPaint);
            AddChild(_body);
        }

        private void SetupPhysicalHitbox()
        {
            CollisionPoints.Add(new Point(-Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, -Height / 2));
            CollisionPoints.Add(new Point(Width / 2, Height / 2));
            CollisionPoints.Add(new Point(-Width / 2, Height / 2));

            CollisionPoints.Add(new Point(-Width / 2, 0));
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

        public void TryJump()
        {
            if (!IsInAir)
            {
                Jump();
            }
            else
            {
                RecordJump();
            }
        }

        public async Task RecordJump()
        {

            _isTryingToJump = true;
            await Task.Delay(300);
            _isTryingToJump = false;
        }

        public override void Render()
        {
            if (!IsInAir && _isTryingToJump)
                Jump();
        }

        private void Jump()
        {
            _isTryingToJump = false;
            ApplyForce(new Force(8, -Math.PI / 2, new ExplosiveForceType()));
        }

        public void MoveBy(float x, float y)
        {
         
            _logger.UpdatePermanentText1($"char {_x}, {_y}");


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
