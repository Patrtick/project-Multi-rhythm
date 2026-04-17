using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    public LevelData level;
    public Transform attackOrigin;

    private void Start()
    {
        StartCoroutine(RunLevel());
    }

    IEnumerator RunLevel()
    {
        foreach (var step in level.steps)
        {
            yield return new WaitForSeconds(step.delayBefore);

            yield return StartCoroutine(step.attack.Execute(attackOrigin));
        }
    }
}
