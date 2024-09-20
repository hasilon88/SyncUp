using UnityEngine;

public class AnimationSync : MonoBehaviour
{

    public AudioManager AudioManager;
    public float AnimationSpeed;
    public FPSManager FPSManager;
    [Range(0, 32)]
    public int Tempo = 4;

    void Update()
    {
        if (this.FPSManager.FrameCount % this.Tempo == 0)
            this.AnimationSpeed = 1.0f + this.AudioManager.CurrentToMaxFrequencyPercentage / 100f;
    }
}
