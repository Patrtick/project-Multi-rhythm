using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "BulletHell/Attacks/Koopa Shell")]
public class KoopaShellAttack : AttackPattern
{
    [Header("�������")]
    [SerializeField] private GameObject koopaPrefab;
    [SerializeField] private GameObject koopaShellPrefab;

    [Header("�����������")]
    [SerializeField] private float walkSpeed = 1.8f;
    [SerializeField] private float shellSpeedMultiplier = 1.35f;
    [SerializeField] private bool startMovingRight = true;

    [Header("�����")]
    [SerializeField] private float walkDuration = 1.5f;
    [SerializeField] private float shellDuration = 2f;

    public override IEnumerator Execute(Transform origin)
    {
        if (koopaPrefab == null || koopaShellPrefab == null)
            yield break;

        var directionX = startMovingRight ? 1f : -1f;

        var koopa = Instantiate(koopaPrefab, origin.position, Quaternion.identity);
        var koopaRb = koopa.GetComponent<Rigidbody2D>();

        yield return MoveHorizontalForTime(koopa.transform, koopaRb, directionX, walkSpeed, walkDuration);

        if (koopa == null)
            yield break;

        var shellSpawnPosition = koopa.transform.position;
        Destroy(koopa);

        var shell = Instantiate(koopaShellPrefab, shellSpawnPosition, Quaternion.identity);
        var shellRb = shell.GetComponent<Rigidbody2D>();
        var shellSpeed = walkSpeed * shellSpeedMultiplier;
        var shellDirectionX = -directionX;

        yield return MoveHorizontalForTime(shell.transform, shellRb, shellDirectionX, shellSpeed, shellDuration);

        if (shellRb != null)
            shellRb.linearVelocity = new Vector2(0f, shellRb.linearVelocity.y);

        if (shell != null)
            Destroy(shell);
    }

    private IEnumerator MoveHorizontalForTime(
        Transform target,
        Rigidbody2D rb,
        float directionX,
        float speed,
        float moveTime
    )
    {
        if (target == null || moveTime <= 0f)
            yield break;

        var timer = 0f;
        while (timer < moveTime)
        {
            if (target == null)
                yield break;
            rb.linearVelocity = new Vector2(directionX * speed, rb.linearVelocity.y);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
