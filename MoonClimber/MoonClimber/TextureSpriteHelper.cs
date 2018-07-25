using System;
using System.Collections.Generic;
using MoonClimber.Blocks;
using SkiaSharp;

namespace MoonClimber
{
    public static class TextureSpriteHelper
    {
        private static IList<string> _blockTypes;

        public static IList<string> GetAllTexturesSpritesFileNames()
        {
            var filenames = new List<string>();
            var values = Enum.GetValues(typeof(BlockType));
            foreach (var value in values)
            {
                for (int i = 1; i < 17; i++)
                {
                    filenames.Add($"Textures/{value}/{value}_{i}.png");
                }
            }

            return filenames;
        }

        public static BlockType GetBlockTypeFromColor(SKColor color)
        {
            if (color.Red == 255)
                return BlockType.rock_block;

            if (color.Red == 125)
                return BlockType.grass_block;

            return BlockType.rock_block;
        }
    }
}
