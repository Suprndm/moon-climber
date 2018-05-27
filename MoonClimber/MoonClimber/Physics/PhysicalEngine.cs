using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoonClimber.Blocks;
using MoonClimber.Blocks.Models;
using MoonClimber.Maths.Segments;
using MoonClimber.Physics.Forces;
using Odin.Maths;
using Odin.Services;
using Xamarin.Forms;

namespace MoonClimber.Physics
{

    /// <summary>
    /// Single Instance physical engine that handle the physics interactions between objects ( like collision)
    /// </summary>
    public class PhysicalEngine
    {
        private static PhysicalEngine _instance;
        private readonly ConcurrentBag<IPhysicalView> _physicalViews;
        private float _groundWidth;
        private float _groundHeight;
        private float _groundX;
        private float _groundY;
        private MapData _mapData;
        private float _blockFriction = 0.1f;
        private float _blockBounceAbsorbsion = 0.5f;
        private readonly Logger _logger;
        private float _blockSize;
        private int _fps = 0;
        private Stopwatch _stopwatch;
        private long _dt;
        private const int _minPhysicalRefreshMs = 15;
        private PhysicalEngine()
        {
            _logger = GameServiceLocator.Instance.Get<Logger>();
            _blockSize = GameRoot.ScreenWidth * AppSettings.BlockScreenRatioX;
            _stopwatch = new Stopwatch();
            _physicalViews = new ConcurrentBag<IPhysicalView>();

            //StartPhysicalLoop();

            //Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            //{
            //    _logger.UpdatePermanentText1(_fps.ToString());
            //    _fps = 0;

            //    return true;
            //});

        }

        public Task StartPhysicalLoop()
        {
            return Task.Run(() =>
            {
                _stopwatch.Start();
                while (true)
                {
                    try
                    {
                        var currentElapsed = _stopwatch.ElapsedMilliseconds;
                        RefreshPhysics();
                        Task.Delay((int)(_minPhysicalRefreshMs)).Wait();
                        _fps++;
                        _dt = _stopwatch.ElapsedMilliseconds - currentElapsed;
                        _logger.Log($"Elapsedtime :{_dt}ms");

                    }
                    catch (Exception e)
                    {
                        _logger.Log($"Physical Error: {e.Message}");
                    }
                }
            });
        }

        public void Setup(float x, float y, float width, float height)
        {
            _groundWidth = width;
            _groundHeight = height;
            _groundX = x;
            _groundY = y;
        }

        public static PhysicalEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PhysicalEngine();
                }
                return _instance;
            }
        }

        public void RegisterPhysicalElement(IPhysicalView physicalView)
        {
            _physicalViews.Add(physicalView);
            physicalView.ApplyForce(new Force(0.5f, Math.PI / 2, new ExplosiveForceType(), -1));
        }

        public void DeclareMapData(MapData mapData)
        {
            _mapData = mapData;
        }

        public void RefreshPhysics()
        {
            foreach (var physicalView in _physicalViews)
            {
                if (physicalView.IsInAir)
                {

                }
                var nextPos = physicalView.ComputeNextPosition();
                var physicalMovement = new PhysicalMovement(
                        new Point(physicalView.RealX, physicalView.RealY),
                        nextPos,
                        physicalView.CollisionPoints);

                //  Logger.Log($"InitalX :{physicalMovement.InitialPos.X} FinalX : {physicalMovement.FinalPos.X}");
                var movementsCollisions = GetMovementCollisions(physicalMovement);
                var count = 0;
                var inTheAir = true;
                while (movementsCollisions.Any())
                {
                    if (count > 0)
                    {
                        int test = 0;
                    }
                    var closerCollision = movementsCollisions.OrderBy(c => c.DistanceToInitialS1).First();

                    //Logger.Log($"Collision detected {closerCollision.Point.X} | {closerCollision.Point.Y}");

                    double reflectionDistance = 0.1;
                    var reflectionAngle = closerCollision.ReflectionAngle;

                    if (physicalView.CanBounce)
                    {
                        reflectionDistance = closerCollision.S1.Distance - closerCollision.DistanceToInitialS1;
                    }

                    var newCornerPosX = closerCollision.Point.X + Math.Cos(reflectionAngle) * reflectionDistance * 1;
                    var newCornerPosY = closerCollision.Point.Y + Math.Sin(reflectionAngle) * reflectionDistance * 1;

                    var xTraveledBeforeImpact = closerCollision.Point.X - closerCollision.S1.P1.X;
                    var yTraveledBeforeImpact = closerCollision.Point.Y - closerCollision.S1.P1.Y;

                    var posXAtImpact = physicalMovement.InitialPos.X + xTraveledBeforeImpact * 0.99f;
                    var posYAtImpact = physicalMovement.InitialPos.Y + yTraveledBeforeImpact * 0.99f;


                    var newPosX = posXAtImpact + Math.Cos(reflectionAngle) * reflectionDistance;
                    var newPosY = posYAtImpact + Math.Sin(reflectionAngle) * reflectionDistance;

                    var previousMovement = physicalMovement;

                    if (physicalView.CanBounce)
                    {
                        var deltaX = closerCollision.S1.P2.X - newCornerPosX;
                        var deltaY = closerCollision.S1.P2.Y - newCornerPosY;
                        newPosX += -deltaX;
                        newPosY += -deltaY;

                        physicalView.V.Amount = physicalView.V.Amount * 0.5f;
                        physicalView.V.Angle = reflectionAngle;
                    }
                    else
                    {
                        // Horizontal
                        if (closerCollision.S2.P1.Y == closerCollision.S2.P2.Y)
                        {
                            newPosX += previousMovement.FinalPos.X - previousMovement.InitialPos.X;
                            physicalView.V.Amount = 0;
                            inTheAir = false;
                        }
                        // Vertical surface
                        else
                        {
                            physicalView.V.X = 0;
                            newPosY += previousMovement.FinalPos.Y - previousMovement.InitialPos.Y;
                        }
                    }
                    physicalMovement = new PhysicalMovement(
                        new Point(posXAtImpact, posYAtImpact),
                        new Point(newPosX, newPosY),
                        physicalView.CollisionPoints);





                    movementsCollisions = GetMovementCollisions(physicalMovement);

                    //Logger.Log($"new pos {physicalMovement.FinalPos.X} | {physicalMovement.FinalPos.Y}");
                    count++;
                }

                physicalView.IsInAir = inTheAir;
                physicalView.RealX = physicalMovement.FinalPos.X;
                physicalView.RealY = physicalMovement.FinalPos.Y;


            }
        }

        private IList<Collision> GetMovementCollisions(PhysicalMovement physicalMovement)
        {
            var movementsCollisions = new List<Collision>();
            foreach (var movement in physicalMovement.CornersMovements)
            {
                if (movement == physicalMovement.CornersMovements[2] && movement.P2.Y > -35)
                {
                    //Logger.Log($"from {movement.P1.Y} to {movement.P2.Y}");
                }
                var eligibleBlocks =
                    BlockGeometryHelper.GetAllBlocksCrossedBySegment(movement, _mapData, _blockSize);

                foreach (var eligibleBlock in eligibleBlocks)
                {
                    var blockSegments = BlockGeometryHelper.GetBlockSegments(eligibleBlock, _blockSize);
                    foreach (var blockSegment in blockSegments)
                    {
                        if (PhysicalHelper.GetCollisionPoint(movement, blockSegment, out var collision))
                        {
                            movementsCollisions.Add(collision);
                        }
                    }
                }
            }

            return movementsCollisions;
        }

        /// <summary>
        /// Could be done at the same time as objects collisions, 
        /// A wall could be an object with a resistance force emited on impact 
        /// </summary>
        private void DetectWallColisions()
        {
            foreach (var physicalView in _physicalViews)
            {
                foreach (var collisionPoint in physicalView.CollisionPoints)
                {
                    var cornerX = physicalView.RealX + collisionPoint.X;
                    var cornerY = physicalView.RealY + collisionPoint.Y;
                    var blockX = (int)Math.Round(cornerX / _blockSize);
                    var blockY = (int)Math.Round(cornerY / _blockSize);

                    var block = _mapData.GetBlockByCoordinates(blockX, blockY);

                    if (block != null)
                    {
                        var previousCornerX = cornerX - physicalView.V.X;
                        var previousCornerY = cornerY - physicalView.V.Y;

                        var finalCornerVector = new SVector(cornerX, cornerY);
                        var previousVectorCornerVector = new SVector(previousCornerX, previousCornerY);

                        var topLeftVector = new SVector((blockX - 0.5) * _blockSize, (blockY - 0.5) * _blockSize);
                        var topRightVector = new SVector((blockX + 0.5) * _blockSize, (blockY - 0.5) * _blockSize);
                        var bottomRightVector = new SVector((blockX + 0.5) * _blockSize, (blockY + 0.5) * _blockSize);
                        var bottomLeftVector = new SVector((blockX - 0.5) * _blockSize, (blockY + 0.5) * _blockSize);

                        SVector intersectionVector;

                        if (SegmentHelper.LineSegementsIntersect(previousVectorCornerVector, finalCornerVector,
                            topLeftVector, topRightVector, out intersectionVector))
                        {
                            _logger.Log($"intersection ! {intersectionVector.X} | {intersectionVector.Y}");
                            HandleCollisionWithBlock(physicalView, intersectionVector);
                            physicalView.V.Y = -physicalView.V.Y;
                            physicalView.RealX += (float)(finalCornerVector.X - intersectionVector.X);
                            physicalView.RealY += (float)(finalCornerVector.Y - intersectionVector.Y);
                        }
                        else if (SegmentHelper.LineSegementsIntersect(previousVectorCornerVector, finalCornerVector,
                            topRightVector, bottomRightVector, out intersectionVector))
                        {
                            _logger.Log($"intersection ! {intersectionVector.X} | {intersectionVector.Y}");
                            HandleCollisionWithBlock(physicalView, intersectionVector);
                            physicalView.V.X = -physicalView.V.X;
                            physicalView.RealX += (float)(finalCornerVector.X - intersectionVector.X);
                            physicalView.RealY += (float)(finalCornerVector.Y - intersectionVector.Y);
                        }
                        else if (SegmentHelper.LineSegementsIntersect(previousVectorCornerVector, finalCornerVector,
                            bottomRightVector, bottomLeftVector, out intersectionVector))
                        {
                            _logger.Log($"intersection ! {intersectionVector.X} | {intersectionVector.Y}");
                            HandleCollisionWithBlock(physicalView, intersectionVector);
                            physicalView.V.Y = -physicalView.V.Y;
                            physicalView.RealX += (float)(finalCornerVector.X - intersectionVector.X);
                            physicalView.RealY += (float)(finalCornerVector.Y - intersectionVector.Y);
                        }
                        else if (SegmentHelper.LineSegementsIntersect(previousVectorCornerVector, finalCornerVector,
                            bottomLeftVector, topLeftVector, out intersectionVector))
                        {
                            _logger.Log($"intersection ! {intersectionVector.X} | {intersectionVector.Y}");
                            HandleCollisionWithBlock(physicalView, intersectionVector);
                            physicalView.V.X = -physicalView.V.X;
                            physicalView.RealX += (float)(finalCornerVector.X - intersectionVector.X);
                            physicalView.RealY += (float)(finalCornerVector.Y - intersectionVector.Y);
                        }
                    }
                }

            }
        }

        private void HandleCollisionWithBlock(IPhysicalView physicalView, SVector intersectionVector)
        {
        }


    }
}
