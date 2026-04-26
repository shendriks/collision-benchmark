using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using CollisionDetection.BruteForce;
using CollisionDetection.Common;
using CollisionDetection.SpatialHash;

namespace CollisionBenchmark;

[Config(typeof(Config))]
public class CollisionBenchmark
{
    private CollisionDetectorBruteForce _detectorBruteForce = null!;
    private CollisionDetectorSpatialHash _detectorSpatialHash = null!;

    private GameObjectManager _gameObjectManager = null!;
    private GameObjectManagerSpatialHash _gameObjectManagerSpatialHash = null!;
    private Random _random = new();

    public int ScreenX { get; set; } = 640;
    public int ScreenY { get; set; } = 360;

    [Params(10, 20, 30, 40, 50, 60, 70, 80, 90, 100)]
    // [Params(100, 200, 300, 400, 500, 600, 700, 800, 900, 1000)]
    public int ObjectCount { get; set; } = 100;
    public int SpatialHashCellSize { get; set; } = 64;
    public int RandomSeed { get; set; } = 42;

    [GlobalSetup(Targets = [nameof(BruteForce)])]
    public void SetupBruteForce()
    {
        _gameObjectManager = new GameObjectManager();

        _random = new Random(RandomSeed);
        for (var i = 0; i < ObjectCount; i++) {
            var go = new GameObject {
                Position = new Vector2(_random.Next(0, ScreenX), _random.Next(0, ScreenY))
            };

            _gameObjectManager.AddGameObject(go);
        }

        // Process additions
        _gameObjectManager.Update();

        _detectorBruteForce = new CollisionDetectorBruteForce(_gameObjectManager);
    }

    [GlobalSetup(Targets = [nameof(SpatialHash)])]
    public void SetupSpatialHash()
    {
        _gameObjectManagerSpatialHash = new GameObjectManagerSpatialHash(SpatialHashCellSize);

        _random = new Random(RandomSeed);
        for (var i = 0; i < ObjectCount; i++) {
            var go = new GameObject {
                Position = new Vector2(_random.Next(0, ScreenX), _random.Next(0, ScreenY))
            };

            _gameObjectManagerSpatialHash.AddGameObject(go);
        }

        // Process additions
        _gameObjectManagerSpatialHash.Update();

        _detectorSpatialHash = new CollisionDetectorSpatialHash(_gameObjectManagerSpatialHash);
    }

    [Benchmark(Description = "Brute Force")]
    public void BruteForce()
    {
        MoveGameObjects();
        _detectorBruteForce.DetectCollisions();
    }

    [Benchmark(Description = "Spatial Hash")]
    public void SpatialHash()
    {
        MoveGameObjectsSpatialHash();
        _detectorSpatialHash.DetectCollisions();
    }

    private void MoveGameObjects()
    {
        for (var i = 0; i < ObjectCount; i++) {
            var go = _gameObjectManager.GameObjects[i];
            go.Position = go.Position with { X = _random.Next(0, ScreenX), Y = _random.Next(0, ScreenY) };
        }
    }

    private void MoveGameObjectsSpatialHash()
    {
        for (var i = 0; i < ObjectCount; i++) {
            var go = _gameObjectManagerSpatialHash.GameObjects[i];
            go.Position = go.Position with { X = _random.Next(0, ScreenX), Y = _random.Next(0, ScreenY) };
        }

        _gameObjectManagerSpatialHash.Update(); // Update spatial hash grid
    }

    private class Config : ManualConfig
    {
        public Config()
        {
            var job = new Job {
                Environment = {
                    Gc = { Force = true }
                }
            };
            AddJob(job);
        }
    }
}
