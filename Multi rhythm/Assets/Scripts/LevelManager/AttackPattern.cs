using UnityEngine;
using System.Collections;

public abstract class AttackPattern : ScriptableObject
{
    public float duration;

    public abstract IEnumerator Execute(Transform origin);
}