using UnityEngine;
using UnityEngine.UI;


public class LevelProgress : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private Text progressText;

    private float levelDuration;
    private float timer;

    private void Start()
    {
        levelDuration = manager.GetLevelDuration();
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        var percent = GetProgressPercent();
        progressText.text = $"{percent:0}%";
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