using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoonClimber.Blocks;
using MoonClimber.Blocks.Services;
using MoonClimber.Data.ChunkData;
using MoonClimber.Data.Interactive;
using MoonClimber.Game.Sprites;
using MoonClimber.Navigation;
using Odin.Core;
using Odin.Navigation;
using Odin.Paint;
using Odin.Services;
using Odin.Sprites;
using Unity;
using Unity.Lifetime;

namespace MoonClimber
{
    public class GameRoot : ORoot
    {
        private PaintStorage _paintStorage;

        public override OdinSettings BuildSettings()
        {
            return new OdinSettings(true);
        }

        public override async Task LoadAssets()
        {
            try
            {
                var spriteFileNames = new List<string>
                {
                    SpriteConst.SmallWhiteHalo,
                    SpriteConst.WhiteHalo,
                    SpriteConst.BaseMap,
                    SpriteConst.Tree,
                    SpriteConst.Background
                };

                foreach (var fileName in TextureSpriteHelper.GetAllTexturesSpritesFileNames())
                {
                    spriteFileNames.Add(fileName);
                }

                await SpriteLoader.Instance.Initialize<GameRoot>(
                    spriteFileNames,
                    "Resources/Graphics",
                    ScreenWidth,
                    ScreenHeight);


               var chunkDataRepository = GameServiceLocator.Instance.Get<IChunkDataRepository>();
               chunkDataRepository.Initialize();

                var interactiveObjectRepository = GameServiceLocator.Instance.Get<IInteractiveObjectRepository>();
                interactiveObjectRepository.Initialize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
        }

        public override void RegisterServices(UnityContainer container)
        {
            container.RegisterType<IChunkDataProvider, ChunkDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChunkDataRepository, ChunkDataRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IInteractiveObjectRepository, InteractiveObjectRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMapLoader, MapLoader>(new ContainerControlledLifetimeManager());
            container.RegisterType<BlockSpriteProvider>(new ContainerControlledLifetimeManager());
        }

        protected override void OnInitialized()
        {
            AddChild(new NavigationLayer());
            Navigator.Instance.GoToInitialPage(PageType.Game);
        }
       
    }
}
