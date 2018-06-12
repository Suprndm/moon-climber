using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MoonClimber.Blocks;
using MoonClimber.Blocks.Models;
using MoonClimber.Blocks.Services;
using MoonClimber.Data.ChunkData;
using MoonClimber.Game.Deblocks;
using MoonClimber.Physics;
using Odin.Containers;
using Odin.Core;
using Odin.Services;
using SkiaSharp;

namespace MoonClimber.Game.Pages.Game
{
    public class MapDisplayer : OView
    {
        private float _blockSize;

        private IList<Block> _blocksList;
        private IMapLoader _mapLoader;
        private readonly Logger _logger;
        private Container _chunksContainer;
        private MapData _mapData;
        private SKPaint _mapPaint;
        private Point _lastPositionProcessed;
        private IList<Chunk> _chunks;
        private bool _isUpdateting;
        private SKImage _skImage;
        public MapDisplayer()
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            try
            {
                _mapLoader = GameServiceLocator.Instance.Get<IMapLoader>();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _isUpdateting = true;
            _mapPaint = new SKPaint() { Color = CreateColor(255, 255, 255) };
            _chunks = new List<Chunk>();
        }

        public Point GetSpawnPosition()
        {
            return _mapLoader.GetSpawnPosition();
        }


        private Task InitializeDisplay()
        {
            return Task.Run(() =>
            {
                try
                {
                    _mapData = _mapLoader.InitializeMap(0, 0);
                    _lastPositionProcessed = new Point(0, 0);
                    PhysicalEngine.Instance.DeclareMapData(_mapData);

                    // InitialSetup
                    foreach (var chunkData in _mapData.Chunks)
                    {
                        var chunk = new Chunk(chunkData);
                        _chunksContainer.AddContent(chunk);
                        _chunks.Add(chunk);
                    }

                    _isUpdateting = false;
                    _logger.Log("Initialized Map");
                }
                catch (Exception e)
                {
                    _logger.Log("Failed to Initialize Map");
                    _logger.Log(e.ToString());
                }

            });
        }

        private Task UpdateDisplay(int x, int y)
        {
            return Task.Run(() =>
            {
                try
                {
                    _isUpdateting = true;
                    var mapDataUpdate = _mapLoader.ActualizeMap(x, y, _mapData);
                    if (mapDataUpdate.LoadedChunks.Any() || mapDataUpdate.UnloadedChunks.Any())
                    {
                        PhysicalEngine.Instance.DeclareMapData(_mapData);
                        int recycledCount = 0;

                        foreach (var loadedChunk in mapDataUpdate.LoadedChunks)
                        {
                            var chunk = new Chunk(loadedChunk);
                            _chunksContainer.AddContent(chunk);
                            _chunks.Add(chunk);
                            _logger.Log($"Added chunk at X:{chunk.X} Y:{chunk.Y}");
                        }


                        foreach (var unloadedChunk in mapDataUpdate.UnloadedChunks)
                        {
                            var chunk = _chunks.FirstOrDefault(c => c.ChunkData == unloadedChunk);
                            if (chunk != null)
                            {
                                _chunksContainer.RemoveContentContent(chunk);
                                _chunks.Remove(chunk);
                                _logger.Log($"Removed chunk at X:{chunk.X} Y:{chunk.Y}");
                            }
                        }

                        _isUpdateting = false;
                    }

                }
                catch (Exception e)
                {
                    _logger.Log($"Failed to Update Display : {e.Message}");
                    _isUpdateting = false;
                }

            });
        }

        public override void Render()
        {
            _logger.UpdatePermanentText3($"used blocks {_blocksList.Count(b => !b.IsRecycled())}");

            var blockSize = GameRoot.ScreenWidth * AppSettings.BlockScreenRatioX;

            var newX = (int)((-X + Width / 2) / blockSize);
            var newY = (int)((-Y + Height / 2) / blockSize);


            var distance = Math.Sqrt((newX - _lastPositionProcessed.X) * (newX - _lastPositionProcessed.X)
                                     + (newY - _lastPositionProcessed.Y) * (newY - _lastPositionProcessed.Y));


            if (distance >= 5 && !_isUpdateting)
            {
                UpdateDisplay(newX, newY);
                _lastPositionProcessed = new Point(newX, newY);
            }
            _logger.UpdatePermanentText3($"x:{X}-y:{Y}");
            Canvas.DrawImage(_skImage, X, Y - 1500, _mapPaint);
        }

        private SKPoint GetBlockVisualPosition(BlockData blockData)
        {
            var x = blockData.X * _blockSize;
            var y = blockData.Y * _blockSize;

            return new SKPoint(x, y);
        }

        public Task Initialize()
        {
            return Task.Run(async () =>
           {
               _chunksContainer = new Container();

               await InitializeDisplay();
           });
        }
    }
}
