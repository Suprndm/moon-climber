using System;
using System.Threading.Tasks;
using MoonClimber.Maths;
using MoonClimber.Physics;
using Odin.Controls;
using Odin.Core;
using Odin.Navigation.Pages;
using Odin.Scene;
using Odin.Services;
using Odin.UIElements.Buttons;

namespace MoonClimber.Game.Pages.Game
{
    public class GamePage : PageBase
    {
        private float _tempX;
        private float _tempY;
        public float CharacterSpawnX { get; set; }
        public float CharacterSpawnY { get; set; }
        private float _blockSize;
        private readonly MapDisplayer _mapDisplayer;
        private readonly ObjectsDisplayer _objectsDisplayer;
        private Scene _scene;
        private Characters.Character _character;
        private Joystick _joystick;
        private readonly Logger _logger;
        public GamePage()
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            DeclarePannable(this);

            _blockSize = ORoot.ScreenUnit * AppSettings.BlockSizeU;

            var _background = new Background();
            AddChild(_background);

            _scene = new Scene();
            AddChild(_scene);

            _mapDisplayer = new MapDisplayer();
            _scene.AddChild(_mapDisplayer);

            _objectsDisplayer = new ObjectsDisplayer();
            _scene.AddChild(_objectsDisplayer);

            var upButton = new TextButton(Width / 2 - 100, 0.9f * Height, 50, "Reset");
            var downButton = new TextButton(Width * 0.8f, 0.5f * Height, 100, "J");


            upButton.Up += () =>
            {
                _character.RealX = CharacterSpawnX;
                _character.RealY = CharacterSpawnY;
                _character.V = new Vector();
            };

            downButton.Down += () =>
            {
                _character.TryJump();
            };

            AddChild(downButton);
            AddChild(upButton);
        }

        private void _joystick_TiltChanged(SkiaSharp.SKPoint tilt)
        {

        }

        public override void Render()
        {
            PhysicalEngine.Instance.RefreshPhysics();

            _tempX = 0;
            _tempY = 0;

            if (_character != null)
            {
                var tilt = _joystick.Tilt;
                float characterMoveSpeed = ORoot.ScreenUnit *  AppSettings.BlockSizeU / 8f;
                var charMoveX = (float)tilt.X * characterMoveSpeed;
                var charMoveY = (float)tilt.Y * characterMoveSpeed;
                _character.MoveBy(charMoveX, charMoveY);

            }
        }

        protected  override async void OnActivated(object parameter = null)
        {
            var spawnPosition = _mapDisplayer.GetSpawnPosition();
            CharacterSpawnX = _blockSize * spawnPosition.X;
            CharacterSpawnY = _blockSize * (spawnPosition.Y - 1);

            await _mapDisplayer.Initialize(spawnPosition.X, spawnPosition.Y);
            await _objectsDisplayer.Initialize(spawnPosition.X, spawnPosition.Y);

            _character = new Characters.Character(CharacterSpawnX, CharacterSpawnY, _blockSize * 0.8f, _blockSize * 2 * 0.8f);
            _scene.AddChild(_character);
            _scene.AttachCameraTo(_character);

            _joystick = new Joystick();
            AddChild(_joystick);

            _joystick.TiltChanged += _joystick_TiltChanged;
        }

        protected override void OnDeactivated()
        {

        }
    }
}
