using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Cone")]
public class ConeAttack : AttackPattern
{
    public GameObject bulletPrefab;
    public int bulletCount = 10;
    public float angle = 60f;
    public float speed = 5f;

    public override IEnumerator Execute(Transform origin)
    {
        var startAngle = -angle / 2;
        var step = angle / (bulletCount - 1);

        for (var i = 0; i < bulletCount; i++)
        {
            var currentAngle = startAngle + step * i;

            var dir = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(bulletPrefab, origin.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
        }

        yield return new WaitForSeconds(duration);
    }
}