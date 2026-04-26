using CollisionDetection.Common;

namespace CollisionDetection.BruteForce;

public class GameObjectManager
{
    private readonly List<GameObject> _gameObjects = [];
    private readonly List<GameObject> _gameObjectsToAdd = [];
    private readonly List<GameObject> _gameObjectsToRemove = [];

    public IReadOnlyList<GameObject> GameObjects => _gameObjects;

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
        for (var i = 0; i < _gameObjects.Count; i++) {
            var gameObject = _gameObjects[i];
            gameObject.Update();
        }

        for (var i = 0; i < _gameObjectsToRemove.Count; i++) {
            var gameObject = _gameObjectsToRemove[i];
            _gameObjects.Remove(gameObject);
        }

        _gameObjectsToRemove.Clear();

        for (var i = 0; i < _gameObjectsToAdd.Count; i++) {
            var gameObject = _gameObjectsToAdd[i];
            _gameObjects.Add(gameObject);
        }

        _gameObjectsToAdd.Clear();
    }
}
