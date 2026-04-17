using System.Collections.Generic;
using UnityEngine;

public static class BfsPathfinder2D
{
    public static bool TryFindPath(
        GridGraph2D grid,
        Vector2Int start,
        Vector2Int goal,
        List<Vector2Int> outPath
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

        return TryFindPathInternal(grid, start, goal, outPath);
    }

    private static bool TryFindPathInternal(
        GridGraph2D grid,
        Vector2Int start,
        Vector2Int goal,
        List<Vector2Int> outPath
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

            for (int i = 0; i < nCount; i++)
            {
                var next = neighborBuf[i];
                if (visited.Contains(next)) continue;
                if (!grid.IsWalkable(next)) continue;

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

}

