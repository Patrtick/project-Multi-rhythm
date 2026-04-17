using System.Collections.Generic;
using UnityEngine;

public static class BfsPathfinder2D
{
    public static bool TryFindPath(
        GridGraph2D grid,
        Vector2Int start,
        Vector2Int goal,
        List<Vector2Int> outPath,
        LayerMask avoidMask,
        float avoidCheckInset = 0.05f
    )
    {
        outPath.Clear();

        if (!grid.InBounds(start) || !grid.InBounds(goal)) return false;
        if (!grid.IsWalkable(start) || !grid.IsWalkable(goal)) return false;

        if (start == goal)
        {
            outPath.Add(start);
            return true;
        }

        if (TryFindPathInternal(grid, start, goal, outPath, avoidMask, avoidCheckInset))
            return true;

        // Fallback: if avoidance made it impossible, try again without avoidance.
        if (avoidMask.value != 0)
            return TryFindPathInternal(grid, start, goal, outPath, default, avoidCheckInset);

        return false;
    }

    private static bool TryFindPathInternal(
        GridGraph2D grid,
        Vector2Int start,
        Vector2Int goal,
        List<Vector2Int> outPath,
        LayerMask avoidMask,
        float avoidCheckInset
    )
    {
        var q = new Queue<Vector2Int>();
        var visited = new HashSet<Vector2Int>();
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        q.Enqueue(start);
        visited.Add(start);

        var neighborBuf = new Vector2Int[4];

        while (q.Count > 0)
        {
            var current = q.Dequeue();
            if (current == goal)
                return ReconstructPath(start, goal, cameFrom, outPath);

            grid.GetNeighbors4(current, neighborBuf, out var nCount);

            // Prefer safe neighbors first (still BFS, just ordering).
            if (avoidMask.value != 0 && nCount > 1)
                SortNeighborsByAvoid(grid, neighborBuf, nCount, avoidMask, avoidCheckInset);

            for (int i = 0; i < nCount; i++)
            {
                var next = neighborBuf[i];
                if (visited.Contains(next)) continue;
                if (!grid.IsWalkable(next)) continue;
                if (avoidMask.value != 0 && IsAvoidCell(grid, next, avoidMask, avoidCheckInset)) continue;

                visited.Add(next);
                cameFrom[next] = current;
                q.Enqueue(next);
            }
        }

        return false;
    }

    private static bool ReconstructPath(
        Vector2Int start,
        Vector2Int goal,
        Dictionary<Vector2Int, Vector2Int> cameFrom,
        List<Vector2Int> outPath
    )
    {
        outPath.Clear();
        outPath.Add(goal);

        var cur = goal;
        while (cur != start)
        {
            if (!cameFrom.TryGetValue(cur, out var prev))
                return false;
            cur = prev;
            outPath.Add(cur);
        }

        outPath.Reverse();
        return true;
    }

    private static void SortNeighborsByAvoid(GridGraph2D grid, Vector2Int[] buf, int count, LayerMask avoidMask, float inset)
    {
        // Small fixed-size sort (count <= 4).
        for (var i = 0; i < count - 1; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                var ai = IsAvoidCell(grid, buf[i], avoidMask, inset);
                var aj = IsAvoidCell(grid, buf[j], avoidMask, inset);
                if (ai && !aj)
                {
                    (buf[i], buf[j]) = (buf[j], buf[i]);
                }
            }
        }
    }

    private static bool IsAvoidCell(GridGraph2D grid, Vector2Int cell, LayerMask avoidMask, float inset)
    {
        var center = grid.CellToWorldCenter(cell);
        var half = Mathf.Max(0.01f, (grid.cellSize * 0.5f) - inset);
        return Physics2D.OverlapBox(center, new Vector2(half * 2f, half * 2f), 0f, avoidMask) != null;
    }
}

