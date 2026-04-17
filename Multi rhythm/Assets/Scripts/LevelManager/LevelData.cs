using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "BulletHell/Level")]
public class LevelData : ScriptableObject
{
    public List<AttackStep> steps;
}