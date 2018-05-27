using System;
using System.Collections.Generic;
using MoonClimber.Data.ChunkData;

namespace MoonClimber.Blocks
{
    public static class BlockVisualHelper
    {
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

        public static BlockVisualSetup GetVisualSetup(BlockData blockData)
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

        public static string GenerateSpriteName(BlockType blockType, BlockVisualSetup blockVisualSetup)
        {
            return $"{blockType.GetName()}_{blockVisualSetup.SpriteType}.png";
        }
    }

    public class BlockVisualSetup
    {
        public int SpriteType { get; set; }
        public double Angle { get; set; }
    }
}

