using UnityEngine;
using System.Collections;

public class EnemyDeathByTime : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 1.5f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;

    private bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {

        if (col != null)
            col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        if (animator != null)
            animator.SetBool("Death", true);

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
