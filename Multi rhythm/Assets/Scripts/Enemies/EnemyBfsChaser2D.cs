using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBfsChaser2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GridGraph2D grid;
    [SerializeField] private Transform player;

    [Header("Detection")]
    [Min(0f)] [SerializeField] private float detectionRadius = 6f;
    [Min(0.05f)] [SerializeField] private float repathIntervalSeconds = 1.5f;

    [Header("Movement")]
    [Min(0.01f)] [SerializeField] private float moveSpeed = 3f;
    [Min(0.001f)] [SerializeField] private float cellArrivalDistance = 0.05f;

    [Header("Tags")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string bulletTag = "Enemy";
    [SerializeField] private bool dieToBullets = true;

    [Header("Debug")]
    [SerializeField] private bool drawPathGizmos = true;

    private Rigidbody2D rb;
    private float nextRepathTime;
    private Vector2Int lastPlayerCell = new(int.MinValue, int.MinValue);

    private readonly List<Vector2Int> path = new();
    private int pathIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Reset()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (grid == null || player == null) return;

        if (!IsPlayerInDetectionRange())
            return;

        var playerCell = grid.WorldToCell(player.position);
        if (Time.time >= nextRepathTime || playerCell != lastPlayerCell)
        {
            Repath(playerCell);
            nextRepathTime = Time.time + repathIntervalSeconds;
            lastPlayerCell = playerCell;
        }
    }

    private void FixedUpdate()
    {
        if (path.Count == 0 || pathIndex >= path.Count) return;

        var targetCell = path[pathIndex];
        var targetPos = grid.CellToWorldCenter(targetCell);

        var pos = rb.position;
        var next = Vector2.MoveTowards(pos, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        if (Vector2.Distance(next, targetPos) <= cellArrivalDistance)
            pathIndex++;
    }

    private bool IsPlayerInDetectionRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectionRadius;
    }

    private void Repath(Vector2Int playerCell)
    {
        var start = grid.WorldToCell(transform.position);
        var goal = playerCell;

        path.Clear();
        pathIndex = 0;

        if (BfsPathfinder2D.TryFindPath(grid, start, goal, path))
        {
            if (path.Count > 0 && path[0] == start)
                pathIndex = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            var death = other.GetComponent<DeathOfEnemy>();
            if (death != null) death.Die();
            return;
        }

        if (dieToBullets && !string.IsNullOrEmpty(bulletTag) && other.CompareTag(bulletTag))
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (!drawPathGizmos) return;
        if (grid == null) return;

        Gizmos.color = new Color(1f, 0.1f, 0.1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (path == null || path.Count == 0) return;
        Gizmos.color = new Color(0.1f, 1f, 0.1f, 0.9f);

        var prev = transform.position;
        for (int i = pathIndex; i < path.Count; i++)
        {
            var p = (Vector3)grid.CellToWorldCenter(path[i]);
            Gizmos.DrawLine(prev, p);
            Gizmos.DrawSphere(p, 0.08f);
            prev = p;
        }
    }
}

