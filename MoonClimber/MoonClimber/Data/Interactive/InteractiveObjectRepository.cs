using System;
using System.Collections.Generic;
using MoonClimber.Game.Sprites;
using MoonClimber.Interactive;
using Odin.Services;
using Odin.Sprites;
using SkiaSharp;

namespace MoonClimber.Data.Interactive
{
    public class InteractiveObjectRepository : IInteractiveObjectRepository
    {
        protected IList<InteractiveObject> Data;
        private readonly Logger _logger;

        public InteractiveObjectRepository(Logger logger)
        {
            _logger = logger;
        }

        public InteractiveObject Get(int id)
        {
            return null;
        }

        public IList<InteractiveObject> GetAll()
        {
            return Data;
        }

        public int Count()
        {
            return Data.Count;
        }

        public void Initialize()
        {
            var objects = new List<InteractiveObject>();

            try
            {

                var spriteData = SpriteLoader.Instance.GetData(SpriteConst.BaseMap);
                var height = (int)spriteData.Bounds.Height;
                var width = (int)spriteData.Bounds.Width;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pixelColor = spriteData.Bitmap.GetPixel(i, j);
                        if (IsBlue(pixelColor))
                        {
                            var obj = InteractiveObjectFactory.BuildFromColor(pixelColor, i, j);
                            if (obj != null)
                            {
                                objects.Add(obj);
                            }
                        }
                    }
                }

                _logger.Log($"Loaded {objects.Count} interactiveObjects");
            }
            catch (Exception e)
            {
                _logger.Log($"Unable to load blocks from {SpriteConst.BaseMap} - {e.Message}");
            }

            Data = objects;
        }


        private bool IsBlue(SKColor color)
        {
            return color.Blue > 0
                   && color.Red == 0
                   && color.Red == 0;
        }
    }
}
