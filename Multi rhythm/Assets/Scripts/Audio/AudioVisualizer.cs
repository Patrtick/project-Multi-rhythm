using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();
    private List<Vector3> initialScales = new List<Vector3>();
    private AudioSource audioSource;
    private float[] samples = new float[512];
    [SerializeField] private float sensitivity = 0.05f;
    void Start()
    {
        foreach (var background in backgrounds)
        {
            initialScales.Add(background.transform.localScale);
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        for (var i = 0; i < backgrounds.Count; i++)
        {
            if (samples[1] > sensitivity)
            {
                backgrounds[i].transform.localScale = initialScales[i] * (1 + samples[1]);
            }
            else
            {
                backgrounds[i].transform.localScale = initialScales[i];
            }
        }
    }
}