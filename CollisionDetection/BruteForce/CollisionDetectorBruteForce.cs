namespace CollisionDetection.BruteForce;

public class CollisionDetectorBruteForce(GameObjectManager gameObjectManager)
{
    public void DetectCollisions()
    {
        var gameObjects = gameObjectManager.GameObjects;

        for (var i = 0; i < gameObjects.Count; i++) {
            var a = gameObjects[i];
            for (var j = i + 1; j < gameObjects.Count; j++) {
                var b = gameObjects[j];

                if (!a.BoundingBox.IntersectsWith(b.BoundingBox)) {
                    continue;
                }

                a.OnCollision(b);
                b.OnCollision(a);
            }
        }
    }
}
