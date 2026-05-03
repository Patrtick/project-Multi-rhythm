using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public override IEnumerator Execute(Transform origin)
    {
        var root = GameObject.Find(rootName);
        if (root == null || firePrefab == null) yield break;
        if (root.transform.childCount == 0) yield break;

        var spawnParent = root.transform.GetChild(0);

        var pivot = new GameObject("FireStickPivot").transform;
        pivot.SetParent(spawnParent);
        pivot.localPosition = Vector3.zero;

        var segments = new List<Transform>();

        for (var i = 0; i < maxSegments; i++)
        {
            var seg = Instantiate(firePrefab, pivot);
            seg.transform.localPosition = new Vector3(0f, segmentSpacing * i, 0f);
            segments.Add(seg.transform);

            yield return WaitWithRotation(pivot, spawnDelay);
        }

        yield return WaitWithRotation(pivot, duration);

        for (var i = segments.Count - 1; i >= 0; i--)
        {
            var seg = segments[i];
            if (seg != null) Destroy(seg.gameObject);

            yield return WaitWithRotation(pivot, spawnDelay);
        }

        Destroy(pivot.gameObject);
    }

    private IEnumerator WaitWithRotation(Transform pivot, float time)
    {
        var t = 0f;
        while (t < time)
        {
            var dt = Time.deltaTime;
            pivot.Rotate(Vector3.forward, rotationSpeed * dt);
            t += dt;
            yield return null;
        }
    }
}