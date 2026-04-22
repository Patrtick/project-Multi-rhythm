using UnityEngine;
using System;

public class PipeWarning : MonoBehaviour
{
    public float duration = 1f;
    public float blinkSpeed = 10f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float timer;
    private Color originalColor;

    public Action OnWarningFinished;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        var t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        spriteRenderer.color = Color.Lerp(originalColor, Color.red, t);

        if (timer <= 0)
        {
            spriteRenderer.color = originalColor;
            gameObject.SetActive(false);

            OnWarningFinished?.Invoke();
        }
    }

    public void StartWarning()
    {
        gameObject.SetActive(true);
        timer = duration;
    }
}
