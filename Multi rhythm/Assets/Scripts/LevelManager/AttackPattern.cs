using UnityEngine;
using System.Collections;

public abstract class AttackPattern : ScriptableObject
{
    public abstract float Duration { get; }
    public abstract IEnumerator Execute(Transform origin);
}