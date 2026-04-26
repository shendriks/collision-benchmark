using System.Drawing;
using CollisionDetection.Common;

namespace CollisionDetection.SpatialHash;

public class GameObjectManagerSpatialHash(int spatialHashCellSize = 64)
{
    private readonly List<GameObject> _gameObjects = [];
    private readonly List<GameObject> _gameObjectsToAdd = [];
    private readonly List<GameObject> _gameObjectsToRemove = [];
    private readonly Dictionary<int, Rectangle> _previousBoundingBoxes = [];

    public IReadOnlyList<GameObject> GameObjects => _gameObjects;
    public RectSpatialHashGrid<GameObject> SpatialHashGrid { get; } = new(spatialHashCellSize, gameObject => gameObject.BoundingBox);

    public void AddGameObject(GameObject gameObject)
    {
        _gameObjectsToAdd.Add(gameObject);
    }

    public void RemoveGameObject(GameObject gameObject)
    {
        _gameObjectsToRemove.Add(gameObject);
    }

    public void Update()
    {
        _previousBoundingBoxes.Clear();
        for (var i = 0; i < _gameObjects.Count; i++) {
            var gameObject = _gameObjects[i];
            _previousBoundingBoxes[gameObject.Id] = gameObject.BoundingBox;
        }

        for (var i = 0; i < _gameObjects.Count; i++) {
            var gameObject = _gameObjects[i];
            gameObject.Update();
        }

        for (var i = 0; i < _gameObjects.Count; i++) {
            var gameObject = _gameObjects[i];
            SpatialHashGrid.Move(gameObject, _previousBoundingBoxes[gameObject.Id]);
        }

        for (var i = 0; i < _gameObjectsToRemove.Count; i++) {
            var gameObject = _gameObjectsToRemove[i];
            _gameObjects.Remove(gameObject);
            SpatialHashGrid.Remove(gameObject);
        }

        _gameObjectsToRemove.Clear();

        for (var i = 0; i < _gameObjectsToAdd.Count; i++) {
            var gameObject = _gameObjectsToAdd[i];
            _gameObjects.Add(gameObject);
            SpatialHashGrid.Add(gameObject);
        }

        _gameObjectsToAdd.Clear();
    }
}
