using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Pipe")]
public class PipeAttack : AttackPattern
{
    [Header("Prefabs")]
    public PipeEnemy pipePrefab;
    public PipeWarning warningPrefab;

    [Header("Spawn cadence")]
    public int spawnCount = 1;
    public float spawnInterval = 1.5f;

    [Header("Warning")]
    public float warningDuration = 1f;

    [Header("Spawn position")]
    public Vector2 spawnViewportXRange = new Vector2(0.15f, 0.85f);
    public float pipeBaseYOffsetFromBottom = 0.25f;
    public float spawnZ = 0f;

    public override IEnumerator Execute(Transform origin)
    {
        if (pipePrefab == null || warningPrefab == null)
            yield break;

        var cam = Camera.main;
        if (cam == null)
            yield break;

        var elapsed = 0f;

        var spawned = 0;
        var nextSpawnAt = 0f;

        while (elapsed < duration)
        {
            var shouldSpawn =
                (spawnCount > 0 && spawned < spawnCount) ||
                (spawnCount <= 0 && elapsed >= nextSpawnAt);

            if (!shouldSpawn)
            {
                yield return null;
                elapsed += Time.deltaTime;
                continue;
            }

            var xViewport = Random.Range(spawnViewportXRange.x, spawnViewportXRange.y);
            var bottomWorld = cam.ViewportToWorldPoint(new Vector3(xViewport, 0f, cam.nearClipPlane));
            var basePos = new Vector3(bottomWorld.x, bottomWorld.y + pipeBaseYOffsetFromBottom, spawnZ);

            var warning = Instantiate(warningPrefab, basePos, Quaternion.identity);
            warning.duration = warningDuration;
            warning.StartWarning();

            var wait = Mathf.Max(0f, warningDuration);
            yield return new WaitForSeconds(wait);
            elapsed += wait;

            if (warning != null)
                Destroy(warning.gameObject);

            var pipe = Instantiate(pipePrefab, basePos, Quaternion.identity);
            pipe.ActivatePipe(basePos);

            spawned++;

            if (spawnCount <= 0)
                nextSpawnAt = elapsed + Mathf.Max(0f, spawnInterval);
        }
    }
}

