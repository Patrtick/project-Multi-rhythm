using UnityEngine;

public class PipeEnemy : MonoBehaviour
{
    public float riseSpeed = 5f;
    public float riseHeight = 2f;
    public float activeTime = 3f;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    private float timer;

    private bool isRising = false;
    private bool isIdle = false;
    private bool isDealingDamage = false;

    [SerializeField] private PipeWarning warning;
    [SerializeField] private Collider2D damageCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        startPosition = transform.position;
        targetPosition = startPosition + Vector2.up * riseHeight;
        warning.OnWarningFinished += StartRising;

        damageCollider.enabled = false;
    }

    private void Start()
    {
        ActivatePipe(transform.position);
    }


    // Update is called once per frame
    private void Update()
    {
        if (isRising)
        {
            Rise();
        }
        else if (isIdle)
        {
            HandleIdle();
        }
    }

    private void Rise()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            riseSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            isRising = false;
            isIdle = true;

            isDealingDamage = false;

            damageCollider.enabled = false;

            timer = activeTime;
        }
    }

    private void StartRising()
    {
        isRising = true;

        isDealingDamage = true;

        if (damageCollider != null)
            damageCollider.enabled = true;
    }

    private void HandleIdle()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            //пока выключаю, обязательно сделаю уход обратно
            gameObject.SetActive(false);
        }
    }

    public void ActivatePipe(Vector2 position)
    {
        transform.position = position;
        startPosition = position;
        targetPosition = startPosition + Vector2.up * riseHeight;
        warning.transform.position = position;

        isRising = false;
        isIdle = false;
        isDealingDamage = false;

        warning.StartWarning();
    }

    private void OnDestroy()
    {
        warning.OnWarningFinished -= StartRising;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDealingDamage)
            return;

        if (other.CompareTag("Player"))
        {
            //Как реализуем хпшки будем отнимать их
            Debug.Log("Труба ударила игрока");
        }
    }
}
