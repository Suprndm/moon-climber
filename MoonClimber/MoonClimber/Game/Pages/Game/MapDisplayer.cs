using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using MoonClimber.Blocks;
using MoonClimber.Blocks.Models;
using MoonClimber.Blocks.Services;
using MoonClimber.Data.ChunkData;
using MoonClimber.Physics;
using MoonClimber.Services;
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
        private Container _blocksContainer;
        private MapData _mapData;

        private Point _lastPositionProcessed;

        private bool _isUpdateting;

        public MapDisplayer()
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _mapLoader = GameServiceLocator.Instance.Get<IMapLoader>();
            _isUpdateting = true;
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
                    _mapData = _mapLoader.ActualizeMap(0, 0);
                    _lastPositionProcessed = new Point(0, 0);
                    PhysicalEngine.Instance.DeclareMapData(_mapData);
                    // InitialSetup
                    for (int i = 0; i < _mapData.Blocks.Count; i++)
                    {
                        var blockData = _mapData.Blocks[i];
                        var block = _blocksList[i];

                        var blockVisualPosition = GetBlockVisualPosition(blockData);
                        block.Setup(blockData, blockVisualPosition.X, blockVisualPosition.Y, new Deblocks.WhiteBlockType());
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
                    var newMapData = _mapLoader.ActualizeMap(x, y);
                    var previousMapData = _mapData;
                    PhysicalEngine.Instance.DeclareMapData(newMapData);
                    int recycledCount = 0;
                    foreach (var block in _blocksList)
                    {
                        if (!block.IsRecycled() && !newMapData.IsInsideMap(block.Data.X, block.Data.Y))
                        {
                            block.Recycle();
                            recycledCount++;
                        }
                        else
                        {
                            //var updatedBlockData = newMapData.GetBlockByCoordinates(block.Data.X, block.Data.Y);
                            //block.Setup(updatedBlockData); =
                        }
                    }

                    _logger.Log($"Recycled {recycledCount} blocks");
                    var recycledBlocks = _blocksList.Where(b => b.IsRecycled()).ToList();
                    var newBlockDatas = new List<BlockData>();

                    foreach (var blockData in newMapData.Blocks)
                    {
                        if (!previousMapData.IsInsideMap(blockData.X, blockData.Y))
                            newBlockDatas.Add(blockData);
                    }

                    _logger.Log($"Added {newBlockDatas.Count} blocks");


                    for (int i = 0; i < newBlockDatas.Count; i++)
                    {
                        var blockData = newBlockDatas[i];
                        var blockVisualPosition = GetBlockVisualPosition(blockData);
                        recycledBlocks[i].Setup(blockData, blockVisualPosition.X, blockVisualPosition.Y, new Deblocks.WhiteBlockType());
                    }

                    _mapData = newMapData;

                    _isUpdateting = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
               _blockSize = GameRoot.ScreenWidth * AppSettings.BlockScreenRatioX;
               var blocksCount = GameRoot.ScreenWidth / _blockSize * GameRoot.ScreenWidth / _blockSize * AppSettings.MapPreloadedRatio * 2;
               _blocksList = new List<Block>();

               _blocksContainer = new Container();
               for (int i = 0; i < blocksCount; i++)
               {
                   var block = new Block(_blockSize, _blockSize);
                   _blocksList.Add(block);
                   _blocksContainer.AddContent(block);
               }

               AddChild(_blocksContainer);

               _logger.Log($"Setup {_blocksList.Count} blockViews");

               await InitializeDisplay();
           });
        }
    }
}
