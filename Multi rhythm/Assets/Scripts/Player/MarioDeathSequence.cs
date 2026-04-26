using System.Collections;
using UnityEngine;

public class MarioDeathSequence : MonoBehaviour
{
    [Header("От кого умираем")]
    [SerializeField] private string enemyAttackTag = "Enemy";

    [Header("Прыжок перед смертью")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float deathGravityScale = 3f;
    [SerializeField] private float delayBeforeDeathWindow = 1.2f;

    [Header("Анимация")]
    [SerializeField] private string deadBoolName = "Dead";

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D[] colliders;
    private PlayerControl playerControl;
    private DeathOfEnemy deathOfEnemy;
    private bool isDying;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider2D>();
        playerControl = GetComponent<PlayerControl>();
        deathOfEnemy = GetComponent<DeathOfEnemy>();

        deathOfEnemy.SetIgnoreTriggerDeath(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;
        if (!other.CompareTag(enemyAttackTag)) return;

        StartCoroutine(PlayMarioDeathAndOpenWindow());
    }

    private IEnumerator PlayMarioDeathAndOpenWindow()
    {
        isDying = true;
        playerControl.enabled = false;

        if (animator != null)
            animator.SetBool(deadBoolName, true);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        rb.linearVelocity = Vector2.up * jumpForce;
        rb.gravityScale = deathGravityScale;

        yield return new WaitForSeconds(delayBeforeDeathWindow);

        deathOfEnemy.SetIgnoreTriggerDeath(false);
        deathOfEnemy.Die();
    }
}
