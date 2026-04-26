namespace CollisionDetection.SpatialHash;

public class CollisionDetectorSpatialHash(GameObjectManagerSpatialHash gameObjectManager)
{
    private readonly HashSet<(int, int)> _collisions = [];

    public void DetectCollisions()
    {
        _collisions.Clear();

        var cells = gameObjectManager.SpatialHashGrid.GetCells();

        foreach (var gameObjects in cells) {
            for (var j = 0; j < gameObjects.Count; j++) {
                var a = gameObjects[j];
                for (var k = j + 1; k < gameObjects.Count; k++) {
                    var b = gameObjects[k];

                    var gameObjectPairKey = a.Id < b.Id ? (a.Id, b.Id) : (b.Id, a.Id);
                    if (_collisions.Contains(gameObjectPairKey)) {
                        continue;
                    }

                    if (!a.BoundingBox.IntersectsWith(b.BoundingBox)) {
                        continue;
                    }

                    _collisions.Add(gameObjectPairKey);

                    a.OnCollision(b);
                    b.OnCollision(a);
                }
            }
        }
    }
}
