using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private Text progressText;

    [SerializeField] private GameObject winPanel;

    private float levelDuration;
    private float timer;

    private bool finished;

    private void Start()
    {
        levelDuration = manager.GetLevelDuration();
        timer = 0f;

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void Update()
    {
        if (finished)
            return;

        timer += Time.deltaTime;

        var percent = GetProgressPercent();
        progressText.text = $"{percent:0}%";

        if (timer >= levelDuration)
        {
            FinishLevel();
        }
    }

    private void FinishLevel()
    {
        finished = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public float GetProgress01()
    {
        if (levelDuration <= 0f) return 0f;
        return Mathf.Clamp01(timer / levelDuration);
    }

    public float GetProgressPercent()
    {
        return GetProgress01() * 100f;
    }
}