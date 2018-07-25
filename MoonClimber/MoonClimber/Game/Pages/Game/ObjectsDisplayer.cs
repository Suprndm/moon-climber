using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using MoonClimber.Blocks;
using MoonClimber.Data.Interactive;
using MoonClimber.Physics;
using Odin.Containers;
using Odin.Core;
using Odin.Services;

namespace MoonClimber.Game.Pages.Game
{
    public class ObjectsDisplayer : OView
    {
        private readonly IInteractiveObjectRepository _interactiveObjectRepository;

        private Point _lastPositionProcessed;

        private Container _objectsContainer;

        public ObjectsDisplayer()
        {
            _interactiveObjectRepository = GameServiceLocator.Instance.Get<IInteractiveObjectRepository>();
        }

        private Task InitializeDisplay(int charSpawnX, int charSpawnY)
        {
            return Task.Run(() =>
            {
                try
                {
                    _lastPositionProcessed = new Point(charSpawnX, charSpawnY);

                    // InitialSetup
                    foreach (var obj in _interactiveObjectRepository.GetAll())
                    {
                        _objectsContainer.AddChild(obj);
                        obj.Load(obj.AbsoluteX * ORoot.ScreenUnit* AppSettings.BlockSizeU, obj.AbsoluteY * ORoot.ScreenUnit * AppSettings.BlockSizeU);
                    }

                }
                catch (Exception e)
                {
                }

            });
        }


        public async Task Initialize(int charSpawnX, int charSpawnY)
        {
            _objectsContainer = new Container();
            AddChild(_objectsContainer);
            await InitializeDisplay(charSpawnX, charSpawnY);
        }
    }
}
