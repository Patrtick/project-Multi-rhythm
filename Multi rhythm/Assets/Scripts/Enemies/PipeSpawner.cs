using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private PipeEnemy pipePrefab;
    [SerializeField] private PipeWarning warningPrefab;

    [Header("Timing")]
    [SerializeField] private float delayBeforeFirstSpawn = 1f;
    [SerializeField] private float timeBetweenSpawns = 2.5f;
    [SerializeField] private float warningDuration = 1f;

    [Header("Spawn position")]
    [SerializeField] private Vector2 spawnViewportXRange = new Vector2(0.15f, 0.85f);
    [SerializeField] private float pipeBaseYOffsetFromBottom = 0.25f;
    [SerializeField] private float spawnZ = 0f;

    [Header("References")]
    [SerializeField] private Camera targetCamera;

    private Coroutine loop;

    private void OnEnable()
    {
        if (loop == null)
            loop = StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        yield return new WaitForSeconds(Mathf.Max(0f, delayBeforeFirstSpawn));

        while (true)
        {
            yield return StartCoroutine(SpawnOne());
            yield return new WaitForSeconds(Mathf.Max(0f, timeBetweenSpawns));
        }
    }

    private IEnumerator SpawnOne()
    {
        var xViewport = Random.Range(spawnViewportXRange.x, spawnViewportXRange.y);
        var bottomWorld = targetCamera.ViewportToWorldPoint(new Vector3(xViewport, 0f, targetCamera.nearClipPlane));

        var basePos = new Vector3(bottomWorld.x, bottomWorld.y + pipeBaseYOffsetFromBottom, spawnZ);

        var warning = Instantiate(warningPrefab, basePos, Quaternion.identity, transform);
        warning.duration = warningDuration;
        warning.StartWarning();

        yield return new WaitForSeconds(Mathf.Max(0f, warningDuration));

        var pipe = Instantiate(pipePrefab, basePos, Quaternion.identity, transform);
        pipe.ActivatePipe(basePos);
    }
}

