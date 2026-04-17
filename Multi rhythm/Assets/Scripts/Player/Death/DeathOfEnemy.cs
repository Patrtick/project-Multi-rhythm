using UnityEngine;

public class DeathOfEnemy : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject deathWindow;
    [SerializeField] private bool pauseTimeOnDeath = true;

    [Header("What kills the player")]
    [SerializeField] private string enemyAttackTag = "Enemy";

    private bool isDead;

    private void Awake()
    {
        if (deathWindow != null)
            deathWindow.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (!other.CompareTag(enemyAttackTag)) return;

        Die();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathWindow != null)
            deathWindow.SetActive(true);

        if (pauseTimeOnDeath)
            Time.timeScale = 0f;
    }
}
