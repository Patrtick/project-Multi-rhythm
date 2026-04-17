using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Circle")]
public class CircleAttack : AttackPattern
{
    public GameObject bulletPrefab;
    public int bulletCount = 20;
    public float speed = 5f;

    public override IEnumerator Execute(Transform origin)
    {
        var angleStep = 360f / bulletCount;

        for (var i = 0; i < bulletCount; i++)
        {
            var angle = i * angleStep;
            var dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(bulletPrefab, origin.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
        }

        yield return new WaitForSeconds(duration);
    }
}