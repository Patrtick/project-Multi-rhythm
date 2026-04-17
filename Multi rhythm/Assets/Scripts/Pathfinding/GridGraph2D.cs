using UnityEngine;

public class GridGraph2D : MonoBehaviour
{
    [Header("Grid")]
    [Min(1)] public int width = 20;
    [Min(1)] public int height = 12;
    [Min(0.01f)] public float cellSize = 1f;
    public Vector2 gridStart = Vector2.zero;

    [Header("Collision")]
    public LayerMask wallMask;
    public float wallCheckInset = 0.05f;

    private static readonly Collider2D[] _overlapBuffer = new Collider2D[16];

    public bool InBounds(Vector2Int c) => c.x >= 0 && c.y >= 0 && c.x < width && c.y < height;

    public Vector2Int WorldToCell(Vector2 world)
    {
        var local = world - gridStart;
        return new Vector2Int(
            Mathf.FloorToInt(local.x / cellSize),
            Mathf.FloorToInt(local.y / cellSize)
        );
    }

    public Vector2 CellToWorldCenter(Vector2Int cell)
    {
        return gridStart + new Vector2((cell.x + 0.5f) * cellSize, (cell.y + 0.5f) * cellSize);
    }

    public bool IsWalkable(Vector2Int cell)
    {
        if (!InBounds(cell)) return false;

        var center = CellToWorldCenter(cell);
        var half = Mathf.Max(0.01f, (cellSize * 0.5f) - wallCheckInset);
        var size = new Vector2(half * 2f, half * 2f);

        var count = Physics2D.OverlapBoxNonAlloc(center, size, 0f, _overlapBuffer, wallMask);
        for (int i = 0; i < count; i++)
        {
            var col = _overlapBuffer[i];
            if (col == null) continue;
            if (col.isTrigger) continue;
            return false;
        }

        return true;
    }

    public void GetNeighbors4(Vector2Int cell, Vector2Int[] buffer, out int count)
    {
        count = 0;

        var up = new Vector2Int(cell.x, cell.y + 1);
        var down = new Vector2Int(cell.x, cell.y - 1);
        var left = new Vector2Int(cell.x - 1, cell.y);
        var right = new Vector2Int(cell.x + 1, cell.y);

        if (InBounds(up)) buffer[count++] = up;
        if (InBounds(down)) buffer[count++] = down;
        if (InBounds(left)) buffer[count++] = left;
        if (InBounds(right)) buffer[count++] = right;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
        Gizmos.DrawWireCube(gridStart + new Vector2(width, height) * (cellSize * 0.5f), new Vector3(width * cellSize, height * cellSize, 0f));
    }
}

