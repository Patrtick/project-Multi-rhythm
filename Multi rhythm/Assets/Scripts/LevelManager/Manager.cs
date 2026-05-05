using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Manager : MonoBehaviour
{
    [SerializeField] private LevelData level;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<SpawnPosition, Transform> spawnMap;

    private void Awake()
    {
        BuildSpawnMap();
    }

    private void Start()
    {
        StartCoroutine(RunLevel());
    }

    private void BuildSpawnMap()
    {
        spawnMap = new Dictionary<SpawnPosition, Transform>();

        foreach (SpawnPosition pos in System.Enum.GetValues(typeof(SpawnPosition)))
        {
            var point = spawnPoints.FirstOrDefault(p => p.name == pos.ToString());

            if (point != null)
            {
                spawnMap[pos] = point;
            }
            else
            {
                Debug.LogWarning($"Точка спавна не найдена для: {pos}");
            }
        }
    }

    IEnumerator RunLevel()
    {
        foreach (var step in level.steps)
        {
            StartCoroutine(LaunchAttackAfterDelay(step));
        }

        yield break;
    }

    IEnumerator LaunchAttackAfterDelay(AttackStep step)
    {
        yield return new WaitForSeconds(step.delayBefore);

        foreach (var pos in step.spawnPosition)
        {
            if (spawnMap.TryGetValue(pos, out var origin))
            {
                StartCoroutine(step.attack.Execute(origin));
            }
        }
    }

    public float GetLevelDuration()
    {
        float maxTime = 0f;

        foreach (var step in level.steps)
        {
            float endTime = step.delayBefore + step.attack.Duration;
            if (endTime > maxTime)
                maxTime = endTime;
        }

        return maxTime;
    }
}

 