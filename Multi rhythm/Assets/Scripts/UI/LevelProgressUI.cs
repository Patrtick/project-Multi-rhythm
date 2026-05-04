using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] private Manager levelManager;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;

    private void OnEnable()
    {
        if (levelManager != null)
            levelManager.OnProgressChanged += HandleProgress;
    }

    private void OnDisable()
    {
        if (levelManager != null)
            levelManager.OnProgressChanged -= HandleProgress;
    }

    private void Start()
    {
        if (levelManager != null)
            HandleProgress(levelManager.Progress01);
    }

    private void HandleProgress(float p01)
    {
        p01 = Mathf.Clamp01(p01);

        if (progressSlider != null)
            progressSlider.normalizedValue = p01;

        if (progressText != null)
            progressText.text = $"{Mathf.RoundToInt(p01 * 100f)}%";
    }
}
