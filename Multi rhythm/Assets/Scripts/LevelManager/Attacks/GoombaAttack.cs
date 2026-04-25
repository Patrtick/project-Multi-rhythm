using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Goomba")]
public class GoombaAttack : AttackPattern
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float speed = 3f;

    private float changeDirectionCooldown = 0.25f;
    private float axisThreshold = 0.1f;

    public override IEnumerator Execute(Transform origin)
    {
        var enemy = Instantiate(enemyPrefab, origin.position, Quaternion.identity);

        var player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            yield break;
        }

        var rb = enemy.GetComponent<Rigidbody2D>();
        var animator = enemy.GetComponent<Animator>();
        var collider = enemy.GetComponent<PolygonCollider2D>();

        var timer = 0f;
        var lastChangeTime = -999f;
        var direction = Vector2.zero;

        while (timer < duration)
        {         
            var delta = player.transform.position - enemy.transform.position;

            if (Time.time >= lastChangeTime + changeDirectionCooldown)
            {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) + axisThreshold)
                {
                    direction = new Vector2(Mathf.Sign(delta.x), 0);
                }
                else if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x) + axisThreshold)
                {
                    direction = new Vector2(0, Mathf.Sign(delta.y));
                }

                lastChangeTime = Time.time;
            }

            rb.linearVelocity = direction * speed;

            timer += Time.deltaTime;
            yield return null;              
        }

        enemy.tag = "Untagged";
        collider.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        animator.SetBool("Death", true);

        yield return new WaitForSeconds(1.5f);
        Object.Destroy(enemy);
    }
}