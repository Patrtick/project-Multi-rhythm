using System.Collections;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private GameObject deathWindow;
    [SerializeField] private string deadBoolName = "Dead";
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float deathGravityScale = 3f;
    [SerializeField] private float delayBeforeDeathWindow = 1.2f;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D[] colliders;
    private PlayerControl playerControl;

    private bool isDead;
    private bool isDying;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        playerControl = GetComponent<PlayerControl>();

        if (deathWindow != null)
            deathWindow.SetActive(false);
    }

    public void Kill()
    {
        if (isDead || isDying) return;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        isDying = true;

        if (playerControl != null)
            playerControl.enabled = false;

        if (animator != null)
            animator.SetBool(deadBoolName, true);

        foreach (var col in colliders)
            col.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            rb.gravityScale = deathGravityScale;
        }

        yield return new WaitForSeconds(delayBeforeDeathWindow);

        Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathWindow != null)
            deathWindow.SetActive(true);

        Time.timeScale = 0f;
    }
}