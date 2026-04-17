using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathWindowUI : MonoBehaviour
{
    [SerializeField] private bool resetTimeScaleOnRestart = true;

    public void Restart()
    {
        if (resetTimeScaleOnRestart)
            Time.timeScale = 1f;

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}

