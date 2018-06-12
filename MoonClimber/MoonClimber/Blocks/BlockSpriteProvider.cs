using System;
using System.Collections.Generic;
using System.Text;
using MoonClimber.Data.ChunkData;
using MoonClimber.Game.Deblocks;
using Odin.Core;
using Odin.Services;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Blocks
{
    public class BlockSpriteProvider
    {
        IDictionary<BlockType, IDictionary<int, Sprite>> _spritesData;
        private readonly Logger _logger;
        private static IDictionary<int, BlockVisualSetup> _setups = new Dictionary<int, BlockVisualSetup>()
        {
            // isolated
            { 1, new BlockVisualSetup() { Angle = 0 , SpriteType =  16} },

            // One corner
            { 2, new BlockVisualSetup() { Angle = 0 , SpriteType =  3} },
            { 3, new BlockVisualSetup() { Angle = 0 , SpriteType =  4} },
            { 5, new BlockVisualSetup() { Angle = 0, SpriteType =  1} },
            { 7, new BlockVisualSetup() { Angle = 0 , SpriteType =  2} },

            // Two joinned corners
            { 6, new BlockVisualSetup() { Angle = 0 , SpriteType =  7} },
            { 15, new BlockVisualSetup() { Angle = 0 , SpriteType =  8} },
            { 35, new BlockVisualSetup() { Angle =0 , SpriteType =  5} },
            { 14, new BlockVisualSetup() { Angle =0 , SpriteType =  6} },

            // Two opposite corners
            { 10, new BlockVisualSetup() { Angle = 0 , SpriteType =  13} },
            { 21, new BlockVisualSetup() { Angle = 0 , SpriteType =  14} },

            // three corners
            { 30, new BlockVisualSetup() { Angle = 0 , SpriteType = 12} },
            { 105, new BlockVisualSetup() { Angle =0 , SpriteType =  9} },
            { 70, new BlockVisualSetup() { Angle = 0 , SpriteType =  10} },
            { 42, new BlockVisualSetup() { Angle = 0 , SpriteType =  11} },
            
            // four corners
            { 210, new BlockVisualSetup() { Angle = 0 , SpriteType =  15} },
        };

        public BlockSpriteProvider( )
        {
            _logger =  GameServiceLocator.Instance.Get<Logger>();
            _spritesData = new Dictionary<BlockType, IDictionary<int, Sprite>>();

            var paint = new SKPaint
            {
                Color = new SKColor(255, 255, 255)
            };

            try
            {
                // Load rockSprites
                var rockSprites = new Dictionary<int, Sprite>();
                var rockBlockType = new RockBlockType();
                foreach (var setup in _setups.Values)
                {
                    var spriteName = GenerateSpriteName(rockBlockType, setup);
                    var sprite = new Sprite(spriteName, 0, 0, AppSettings.ChunckSizeU * ORoot.U, AppSettings.ChunckSizeU * ORoot.U, paint);
                    rockSprites.Add(setup.SpriteType, sprite);
                }

                _spritesData.Add(new KeyValuePair<BlockType, IDictionary<int, Sprite>>(new RockBlockType(), rockSprites));
            }
            catch (Exception e)
            {
                _logger.Log($"Failed to load blockSprites :{e.Message}");
            }

            _logger.Log($"Successfully load blockSprites");
        }

        public Sprite GetBlockSprite(BlockType blockType, BlockData blockData)
        {
            var setup = GetVisualSetup(blockData);
            return _spritesData[blockType][setup.SpriteType];
        }


        private BlockVisualSetup GetVisualSetup(BlockData blockData)
        {
            var blockOrientationScore = 1;

            if (blockData.Left != null)
                blockOrientationScore *= 2;
            if (blockData.Bottom != null)
                blockOrientationScore *= 3;
            if (blockData.Right != null)
                blockOrientationScore *= 5;
            if (blockData.Top != null)
                blockOrientationScore *= 7;

            return _setups[blockOrientationScore];
        }

        private string GenerateSpriteName(BlockType blockType, BlockVisualSetup blockVisualSetup)
        {
            return $"{blockType.GetName()}_{blockVisualSetup.SpriteType}.png";
        }
    }
}
