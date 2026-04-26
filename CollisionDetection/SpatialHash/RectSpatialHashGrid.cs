using System.Drawing;

namespace CollisionDetection.SpatialHash;

public sealed class RectSpatialHashGrid<T>(float cellSize, Func<T, Rectangle> getBounds)
{
    private readonly Dictionary<(int, int), List<T>> _cells = new();

    private (int x, int y) GetCell(float x, float y)
    {
        return (
            (int)MathF.Floor(x / cellSize),
            (int)MathF.Floor(y / cellSize)
        );
    }

    public void Add(T item)
    {
        var bounds = getBounds(item);
        var min = GetCell(bounds.Left, bounds.Top);
        var max = GetCell(bounds.Right, bounds.Bottom);

        for (var x = min.x; x <= max.x; x++) {
            for (var y = min.y; y <= max.y; y++) {
                if (!_cells.TryGetValue((x, y), out var list)) {
                    list = [];
                    _cells[(x, y)] = list;
                }

                list.Add(item);
            }
        }
    }

    public void Remove(T item)
    {
        var bounds = getBounds(item);
        RemoveAt(item, bounds);
    }

    private void RemoveAt(T item, Rectangle bounds)
    {
        var min = GetCell(bounds.Left, bounds.Top);
        var max = GetCell(bounds.Right, bounds.Bottom);

        for (var x = min.x; x <= max.x; x++) {
            for (var y = min.y; y <= max.y; y++) {
                if (!_cells.TryGetValue((x, y), out var list)) {
                    continue;
                }

                list.Remove(item);
            }
        }
    }

    public void Move(T item, Rectangle previousBounds)
    {
        var newBounds = getBounds(item);
        if (newBounds == previousBounds) {
            return;
        }

        var oldMin = GetCell(previousBounds.Left, previousBounds.Top);
        var oldMax = GetCell(previousBounds.Right, previousBounds.Bottom);
        var newMin = GetCell(newBounds.Left, newBounds.Top);
        var newMax = GetCell(newBounds.Right, newBounds.Bottom);

        if (oldMin == newMin && oldMax == newMax) {
            return;
        }

        RemoveAt(item, previousBounds);
        Add(item);
    }

    public ICollection<List<T>> GetCells()
    {
        return _cells.Values;
    }
}
