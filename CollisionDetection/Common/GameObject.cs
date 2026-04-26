using System.Drawing;
using System.Numerics;

namespace CollisionDetection.Common;

public class GameObject
{
    private static int _nextId;
    private Rectangle _boundingBox = new(-16, -16, 32, 32);
    private int _lastCollisionId = -1;

    public int Id { get; } = Interlocked.Increment(ref _nextId);

    public Vector2 Position {
        get;
        set {
            field = value;
            _boundingBox.X = (int)field.X - 16;
            _boundingBox.Y = (int)field.Y - 16;
        }
    }

    public Rectangle BoundingBox => _boundingBox;

    public void Update()
    {
        // Update Logic
    }

    public void Draw()
    {
        // Draw
    }

    public void OnCollision(GameObject other)
    {
        _lastCollisionId = other.Id;
    }
}
