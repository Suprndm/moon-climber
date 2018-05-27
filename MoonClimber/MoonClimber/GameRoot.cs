using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoonClimber.Blocks.Services;
using MoonClimber.Data.ChunkData;
using MoonClimber.Game.Sprites;
using MoonClimber.Navigation;
using Odin.Core;
using Odin.Navigation;
using Odin.Paint;
using Odin.Services;
using Odin.Sprites;
using Unity;

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
                await SpriteLoader.Instance.Initialize<GameRoot>(
                    new List<string>
                    {
                        SpriteConst.white_block_0,
                        SpriteConst.white_block_1,
                        SpriteConst.white_block_2,
                        SpriteConst.white_block_22,
                        SpriteConst.white_block_3,
                        SpriteConst.white_block_4,
                        SpriteConst.BaseMap,
                    },
                    "Resources/Graphics",
                    ScreenWidth,
                    ScreenHeight);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        
        }

        public override void RegisterServices(UnityContainer container)
        {
            container.RegisterType<IChunkDataProvider, ChunkDataProvider>();
            container.RegisterType<IChunkDataRepository, ChunkDataRepository>();
            container.RegisterType<IMapLoader, MapLoader>();
        }

        protected override void OnInitialized()
        {
            AddChild(new NavigationLayer());
            Navigator.Instance.GoToInitialPage(PageType.Game);
        }
       
    }
}
