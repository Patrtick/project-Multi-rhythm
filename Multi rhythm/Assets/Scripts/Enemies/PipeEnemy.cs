using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PipeEnemy : MonoBehaviour
{
    [Header("Emerge")]
    [FormerlySerializedAs("riseSpeed")]
    [SerializeField] private float emergeSpeed = 6f;
    [SerializeField] private float emergeFromBelowOffset = 2f;

    [Header("Damage")]
    [SerializeField] private Collider2D damageCollider;

    [Header("Despawn")]
    [FormerlySerializedAs("activeTime")]
    [SerializeField] private float lifetimeAfterEmerge = 3f;
    [SerializeField] private bool destroyOnDespawn = true;

    private Vector2 basePosition;
    private bool isActive;
    private bool isEmerged;

    private void Awake()
    {
        if (damageCollider != null)
            damageCollider.enabled = false;
    }

    public void ActivatePipe(Vector2 emergeToPosition)
    {
        isActive = true;
        isEmerged = false;

        basePosition = emergeToPosition;

        var startPos = basePosition + Vector2.down * Mathf.Max(0f, emergeFromBelowOffset);
        transform.position = startPos;

        if (damageCollider != null)
            damageCollider.enabled = false;

        StopAllCoroutines();
        StartCoroutine(EmergeRoutine());
    }

    private IEnumerator EmergeRoutine()
    {
        while (Vector2.Distance(transform.position, basePosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                basePosition,
                emergeSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = basePosition;
        isEmerged = true;

        if (damageCollider != null)
            damageCollider.enabled = true;

        if (lifetimeAfterEmerge > 0f)
        {
            yield return new WaitForSeconds(lifetimeAfterEmerge);
            Despawn();
        }
    }

    private void Despawn()
    {
        isActive = false;
        isEmerged = false;

        if (damageCollider != null)
            damageCollider.enabled = false;

        if (destroyOnDespawn)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || !isEmerged)
            return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок получил урон");
        }
    }
}
