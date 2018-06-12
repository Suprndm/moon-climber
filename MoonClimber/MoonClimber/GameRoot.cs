using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoonClimber.Blocks;
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
                        SpriteConst.rock_block_1,
                        SpriteConst.rock_block_2,
                        SpriteConst.rock_block_3,
                        SpriteConst.rock_block_4,
                        SpriteConst.rock_block_5,
                        SpriteConst.rock_block_6,
                        SpriteConst.rock_block_7,
                        SpriteConst.rock_block_8,
                        SpriteConst.rock_block_9,
                        SpriteConst.rock_block_10,
                        SpriteConst.rock_block_11,
                        SpriteConst.rock_block_12,
                        SpriteConst.rock_block_13,
                        SpriteConst.rock_block_14,
                        SpriteConst.rock_block_15,
                        SpriteConst.rock_block_16,
                        SpriteConst.SmallWhiteHalo,
                        SpriteConst.WhiteHalo,
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
            container.RegisterType<BlockSpriteProvider>();
        }

        protected override void OnInitialized()
        {
            AddChild(new NavigationLayer());
            Navigator.Instance.GoToInitialPage(PageType.Game);
        }
       
    }
}
