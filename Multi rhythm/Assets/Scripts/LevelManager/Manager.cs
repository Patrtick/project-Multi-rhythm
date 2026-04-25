using UnityEngine;
using System.Collections;
using System.Linq;

public class Manager : MonoBehaviour
{
    [SerializeField] private LevelData level;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        StartCoroutine(RunLevel());
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

        var origin = spawnPoints.First(x => x.name == step.spawnPointId);

        yield return StartCoroutine(step.attack.Execute(origin));
    }
}