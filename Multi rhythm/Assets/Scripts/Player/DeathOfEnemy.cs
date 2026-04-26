using UnityEngine;

public class DeathOfEnemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject deathWindow;

    [Header("Animator")]
    [SerializeField] private string deadBoolName = "Dead";

    [Header("Что убивает игрока")]
    [SerializeField] private string enemyAttackTag = "Enemy";

    private Animator animator;
    private bool isDead;
    private bool ignoreTriggerDeath;

    private void Awake()
    {
        if (deathWindow != null)
            deathWindow.SetActive(false);
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreTriggerDeath) return;
        if (isDead) return;
        if (!other.CompareTag(enemyAttackTag)) return;

        Die();
    }

    public void SetIgnoreTriggerDeath(bool shouldIgnore)
    {
        ignoreTriggerDeath = shouldIgnore;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null)
            animator.SetBool(deadBoolName, true);

        if (deathWindow != null)
            deathWindow.SetActive(true);
    }
}
