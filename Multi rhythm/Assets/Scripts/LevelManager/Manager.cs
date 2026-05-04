using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Manager : MonoBehaviour
{
    [SerializeField] private LevelData level;
    [SerializeField] private Transform[] spawnPoints;

    private Dictionary<SpawnPosition, Transform> spawnMap;

    private int totalAttacks;
    private int completedAttacks;

    public float Progress01 => totalAttacks > 0 ? Mathf.Clamp01((float)completedAttacks / totalAttacks) : 1f;

    public event Action<float> OnProgressChanged;
    public event Action OnLevelComplete;

    private void Awake()
    {
        BuildSpawnMap();
    }

    private void Start()
    {
        if (level == null || level.steps == null)
        {
            Debug.LogError("Manager: не назначен LevelData или список steps пуст.");
            return;
        }

        totalAttacks = CountPlannedAttacks();
        completedAttacks = 0;
        NotifyProgress();

        if (totalAttacks == 0)
        {
            OnLevelComplete?.Invoke();
            return;
        }

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

    private int CountPlannedAttacks()
    {
        var n = 0;
        foreach (var step in level.steps)
        {
            if (step.attack == null || step.spawnPosition == null)
                continue;
            foreach (var pos in step.spawnPosition)
            {
                if (spawnMap.ContainsKey(pos))
                    n++;
            }
        }

        return n;
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

        if (step.attack == null || step.spawnPosition == null)
            yield break;

        foreach (var pos in step.spawnPosition)
        {
            if (spawnMap.TryGetValue(pos, out var origin))
            {
                StartCoroutine(RunAttackTracked(step.attack, origin));
            }
        }
    }

    IEnumerator RunAttackTracked(AttackPattern attack, Transform origin)
    {
        yield return attack.Execute(origin);

        completedAttacks++;
        NotifyProgress();

        if (completedAttacks >= totalAttacks)
            OnLevelComplete?.Invoke();
    }

    private void NotifyProgress()
    {
        OnProgressChanged?.Invoke(Progress01);
    }
}