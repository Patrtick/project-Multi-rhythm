using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "BulletHell/Attacks/FireStick")]
public class FireStickAttack : AttackPattern
{
    [SerializeField] private string rootName;
    [SerializeField] private GameObject firePrefab;

    [Header("Настройки спавна")]
    [SerializeField] private int maxSegments = 6;
    [SerializeField] private float spawnDelay = 0.1f;
    [SerializeField] private float segmentSpacing = 0.7f;

    [Header("Настройки поворота")]
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float duration = 5f;

    public override float Duration =>
    (maxSegments * spawnDelay) +
    duration +
    (maxSegments * spawnDelay);

    public override IEnumerator Execute(Transform origin)
    {
        _ = ExecuteAsync(origin);
        yield break;
    }

    private async Task ExecuteAsync(Transform origin)
    {
        var root = GameObject.Find(rootName);
        if (root == null || firePrefab == null)
            return;

        if (root.transform.childCount == 0)
            return;

        var spawnParent = root.transform.GetChild(0);

        var stickPivot = new GameObject("FireStickPivot").transform;
        stickPivot.SetParent(spawnParent);
        stickPivot.localPosition = Vector3.zero;

        var segments = new List<Transform>(maxSegments);

        var spawnIndex = 0;
        var nextSpawnTime = Time.time + spawnDelay;

        var endTime = Time.time + duration;
        var despawnStarted = false;
        var nextDespawnTime = 0f;

        while (true)
        {
            var now = Time.time;

            stickPivot.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            if (spawnIndex < maxSegments && now >= nextSpawnTime)
            {
                nextSpawnTime += spawnDelay;

                var segment = Object.Instantiate(firePrefab, stickPivot);

                segment.transform.localPosition = new Vector3(
                    0f,
                    segmentSpacing * spawnIndex,
                    0f
                );

                segments.Add(segment.transform);
                spawnIndex++;
            }

            if (!despawnStarted && spawnIndex >= maxSegments && now >= endTime)
            {
                despawnStarted = true;
                nextDespawnTime = now + spawnDelay;
            }

            if (despawnStarted && segments.Count > 0 && now >= nextDespawnTime)
            {
                nextDespawnTime += spawnDelay;

                var seg = segments[segments.Count - 1];

                if (seg != null)
                    Object.Destroy(seg.gameObject);

                segments.RemoveAt(segments.Count - 1);
            }

            if (despawnStarted && segments.Count == 0)
                break;

            await Task.Yield();
        }

        Object.Destroy(stickPivot.gameObject);
    }
}