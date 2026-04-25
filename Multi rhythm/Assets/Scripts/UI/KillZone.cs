using UnityEngine;
public class KillZone : MonoBehaviour
{
    [SerializeField] private string bulletTag = "Enemy";
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(bulletTag)) return;
        Destroy(other.gameObject);
    }
}